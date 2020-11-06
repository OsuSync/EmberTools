﻿///use recursive descent
using System.Collections.Generic;

namespace EmberKernel.Services.Statistic.Formatter.DefaultImpl.FormatExpression
{
    /// <summary>
    ///
    /// E_1 => E_1 && E0
    /// E_1 => E_1 || E0
    ///
    /// E0 -> E0 > E1
    /// E0 -> E0 < E1
    /// E0 -> E0 >= E1
    /// E0 -> E0 <= E1
    /// E0 -> E0 == E1
    /// E0 -> E0 != E1
    /// 
    /// E1 -> E1 + E2
    /// E1 -> E1 - E2
    ///
    /// E2 -> E2 * E3
    /// E2 -> E2 / E3
    /// E2 -> E2 % E3
    ///
    /// E3 -> E3 ^ E4
    ///
    /// E4 -> E5 | -E5 | +E5 | NotExpr
    ///
    /// NotExpr -> !E_1
    /// 
    /// E5 -> (E_1) | F | Func | !E_1
    ///
    /// 
    ///
    /// Func -> Id(Args)
    /// Args -> Args , Args
    /// Args -> E0
    /// Args -> ε
    ///
    /// F -> Id | Number
    /// </summary>
    public class ExpressionParser
    {
        private readonly List<Token> _tokens = new List<Token>();
        private int _pos;

        public ExpressionParser()
        {
            _pos = 0;
        }

        public List<string> AnalyseMayRequestVariables(IAstNode root)
        {
            var result = new List<string>();
            var waitProcList = new Queue<IAstNode>();
            waitProcList.Enqueue(root);

            while (waitProcList.TryDequeue(out var cur))
            {
                switch (cur)
                {
                    case AstVariableNode variable:
                        result.Add(variable.Id);
                        break;
                    case AstOpNode op:
                        waitProcList.Enqueue(op.LNode);
                        waitProcList.Enqueue(op.RNode);
                        break;
                    case AstFunctionNode function:
                        foreach (var sub in function.Args)
                            waitProcList.Enqueue(sub);
                        break;
                    case AstNumberNode _:
                    default:
                        break;
                }
            }

            return result;
        }

        public IAstNode Parse(string expr)
        {
            _tokens.Clear();
            _pos = 0;

            char[] exprChars = expr.Trim().ToCharArray();
            int pos = 0;

            Token token;
            do
            {
                token = TokenHelper.ReadToken(exprChars,ref pos);
                if (token.Type == TokenType.Unknown)
                {
                    throw new ExprssionTokenException($"Unkown Token. Data: {token.Data}");
                }
                _tokens.Add(token);
            } while (token.Type != TokenType.EOF);

            var root = E_1();
            if (LookToken().Type == TokenType.EOF)
            {
                _tokens.Clear();
                return root;
            }

            _tokens.Clear();

            return null;
        }

        private Token GetToken()
        {
            if (_pos >= _tokens.Count) return new Token(TokenType.EOF);
            Token token = _tokens[_pos++];
            return token;
        }

        private Token LookToken()
        {
            if (_pos >= _tokens.Count) return new Token(TokenType.EOF);
            Token token = _tokens[_pos];
            return token;
        }

        private IAstNode E_1()
        {
            IAstNode expr = E0();
            if (expr != null)
            {
                while ((LookToken().Type == TokenType.Op && LookToken().Data == "&&") ||
                       (LookToken().Type == TokenType.Op && LookToken().Data == "||"))
                {
                    Token token = GetToken();
                    AstOpNode op = new AstOpNode(token.Data)
                    {
                        LNode = expr,
                        RNode = E0()
                    };
                    expr = op;
                }
            }

            return expr;
        }

        private IAstNode E0()
        {
            IAstNode expr = E1();
            if (expr != null)
            {
                while ((LookToken().Type == TokenType.Op && LookToken().Data == "<") ||
                       (LookToken().Type == TokenType.Op && LookToken().Data == ">") ||
                       (LookToken().Type == TokenType.Op && LookToken().Data == ">=") ||
                       (LookToken().Type == TokenType.Op && LookToken().Data == "<=") ||
                       (LookToken().Type == TokenType.Op && LookToken().Data == "==") ||
                       (LookToken().Type == TokenType.Op && LookToken().Data == "!="))
                {
                    Token token = GetToken();
                    AstOpNode op = new AstOpNode(token.Data)
                    {
                        LNode = expr,
                        RNode = E1()
                    };
                    expr = op;
                }
            }

            return expr;
        }

        private IAstNode E1()
        {
            IAstNode expr = E2();
            if (expr != null)
            {
                while ((LookToken().Type == TokenType.Op && LookToken().Data == "+") ||
                       (LookToken().Type == TokenType.Op && LookToken().Data == "-"))
                {
                    Token token = GetToken();
                    AstOpNode op = new AstOpNode(token.Data)
                    {
                        LNode = expr,
                        RNode = E2()
                    };
                    expr = op;
                }
            }

            return expr;
        }

        private IAstNode E2()
        {
            IAstNode expr = E3();
            if (expr != null)
            {
                while ((LookToken().Type == TokenType.Op && LookToken().Data == "*") ||
                       (LookToken().Type == TokenType.Op && LookToken().Data == "/") ||
                       (LookToken().Type == TokenType.Op && LookToken().Data == "%"))
                {
                    Token token = GetToken();
                    AstOpNode op = new AstOpNode(token.Data)
                    {
                        LNode = expr,
                        RNode = E3()
                    };
                    expr = op;
                }
            }

            return expr;
        }

        private IAstNode E3()
        {
            IAstNode expr = E4();
            if (expr != null)
            {
                while ((LookToken().Type == TokenType.Op && LookToken().Data == "^"))
                {
                    Token token = GetToken();
                    AstOpNode op = new AstOpNode(token.Data)
                    {
                        LNode = expr,
                        RNode = E4()
                    };
                    expr = op;
                }
            }

            return expr;
        }

        private IAstNode E4()
        {
            int oldPos = _pos;
            Token token = GetToken();
            if (token.Type == TokenType.Op && (token.Data == "-" || token.Data == "+"))
            {
                IAstNode node = E5();
                bool negative = (token.Data == "-");

                if (node is AstNumberNode numNode)
                {
                    return new AstNumberNode(negative?-numNode.Number:numNode.Number);
                }

                return new AstOpNode(token.Data)
                {
                    LNode = new AstNumberNode(0),
                    RNode = node
                };
            }
            _pos = oldPos;

            {
                IAstNode node = E5();
                if (node != null)
                    return node;
            }
            _pos = oldPos;

            {
                IAstNode node = NotExpr();
                if (node != null)
                    return node;
            }
            _pos = oldPos;

            return null;
        }

        private IAstNode NotExpr()
        {
            int oldPos = _pos;
            Token token = GetToken();
            if (token.Type == TokenType.Op && token.Data == "!")
            {
                AstOpNode expr = new AstOpNode(token.Data)
                {
                    LNode = E_1()
                };
                return expr;
            }
            _pos = oldPos;
            return null;
        }

        private IAstNode E5()
        {
            int oldPos = _pos;
            Token token = GetToken();
            IAstNode node;

            if (token.Type == TokenType.Op && token.Data == "(")
            {
                node = E_1();
                if (node!=null)
                {
                    token = GetToken();
                    if (token.Type == TokenType.Op && token.Data == ")")
                    {
                        return node;
                    }
                }
            }
            _pos = oldPos;

            {
                node = Func();
                if (node != null)
                {
                    return node;
                }
            }

            return F();
        }

        private IAstNode Func()
        {
            int oldPos = _pos;
            Token token = GetToken();

            if (token.Type == TokenType.Id)
            {
                string id = token.Data;
                token = GetToken();
                if (token.Type == TokenType.Op && token.Data == "(")
                {
                    List<IAstNode> nodes = FuncArgs();
                    if (nodes != null)
                    {
                        token= GetToken();
                        if (token.Type == TokenType.Op && token.Data == ")")
                        {
                            return new AstFunctionNode(id,nodes);
                        }
                    }
                }
            }
            _pos = oldPos;

            return null;
        }

        private List<IAstNode> FuncArgs()
        {
            {
                IAstNode node = E0();
                if (node != null)
                {
                    List<IAstNode> args = new List<IAstNode>
                    {
                        node
                    };
                    while (LookToken().Type == TokenType.Op && LookToken().Data == ",")
                    {
                        GetToken();
                        IAstNode expr = E0();
                        if(expr!=null)
                            args.Add(expr);
                    }

                    return args;
                }
            }

            return new List<IAstNode>();
        }

        private IAstNode F()
        {
            int oldPos = _pos;
            Token token = GetToken();

            IAstNode node = token.Type switch
            {
                TokenType.Id => new AstVariableNode(token.Data),
                TokenType.Number => new AstNumberNode(token.Number),
                TokenType.String => new AstStringNode(token.Data),
                _=> throw new ExpressionException("Bad generator : F -> " + token.Type)
            };

            if (node != null)
                return node;

            _pos = oldPos;
            return null;
        }
    }
}
