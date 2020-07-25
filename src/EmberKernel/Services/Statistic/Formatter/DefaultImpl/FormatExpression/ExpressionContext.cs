using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Services.Statistic.Formatter.DefaultImpl.FormatExpression
{
    public class ExpressionContext
    {
        private Random _random=new Random();

        public ConcurrentDictionary<string, double> Constants { get; } = new ConcurrentDictionary<string, double>();
        public ConcurrentDictionary<string, double> Variables { get; } = new ConcurrentDictionary<string, double>();
        public ConcurrentDictionary<string, Func<List<double>, double>> Functions = new ConcurrentDictionary<string, Func<List<double>, double>>();

        public ExpressionContext()
        {
            Constants["pi"] = Math.PI;
            Constants["e"] = Math.E;
            Constants["inf"] = double.PositiveInfinity;

            Functions["sin"] = (args) => Math.Sin(args[0]);
            Functions["cos"] = (args) => Math.Cos(args[0]);
            Functions["tan"] = (args) => Math.Tan(args[0]);
            Functions["asin"] = (args) => Math.Asin(args[0]);
            Functions["acos"] = (args) => Math.Acos(args[0]);
            Functions["atan"] = (args) => Math.Atan(args[0]);
            Functions["pow"] = (args) => Math.Pow(args[0], args[1]);
            Functions["sqrt"] = (args) => Math.Sqrt(args[0]);
            Functions["abs"] = (args) => Math.Abs(args[0]);
            Functions["max"] = (args) => Math.Max(args[0], args[1]);
            Functions["min"] = (args) => Math.Min(args[0], args[1]);
            Functions["exp"] = (args) => Math.Exp(args[0]);
            Functions["log"] = (args) => Math.Log(args[0]);
            Functions["log10"] = (args) => Math.Log10(args[0]);
            Functions["floor"] = (args) => Math.Floor(args[0]);
            Functions["ceil"] = (args) => Math.Ceiling(args[0]);
            Functions["round"] = (args) => Math.Round(args[0], (int)args[1], MidpointRounding.AwayFromZero);
            Functions["sign"] = (args) => Math.Sign(args[0]);
            Functions["truncate"] = (args) => Math.Truncate(args[0]);
            Functions["clamp"] = (args) => Math.Max(Math.Min(args[0], args[2]), args[1]);
            Functions["lerp"] = (args) => (1 - args[2]) * args[0] + args[2] * args[1];
            Functions["random"] = (args) =>
                args.Count >= 2 ? args[0] + _random.NextDouble() * (args[1] - args[0]) : _random.NextDouble();
            Functions["getTime"] = (args) =>
                DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
            Functions["mod"] = (args) => args[0] % args[1];
            Functions["isnan"] = (args) => double.IsNaN(args[0]) ? 1 : 0;
            Functions["isinf"] = (args) => double.IsInfinity(args[0]) ? 1 : 0;
        }
    
        private static bool IsNotZero(double a)
        {
            return (Math.Abs(a) > 1e-5);
        }

        public double ExecAst(IAstNode root)
        {
            switch (root)
            {
                case AstNumberNode numberNode:
                    return numberNode.Number;
                case AstVariableNode varNode:
                    if (Constants.TryGetValue(varNode.Id, out var val))
                    {
                        return (double)val;
                    }
                    else if (Variables.TryGetValue(varNode.Id, out val))
                    {
                        return (double)val;
                    }
                    else
                    {
                        //Sync.Tools.IO.CurrentIO.WriteColor($"[RTPP:Expression]No Variable found (return zero). Variable name: { varNode.Id }", ConsoleColor.Yellow);
                        
                        return 0;
                    }

                case AstOpNode opNode:
                    switch (opNode.Op)
                    {
                        case "+":
                            return ExecAst(opNode.LNode) + ExecAst(opNode.RNode);
                        case "-":
                            return ExecAst(opNode.LNode) - ExecAst(opNode.RNode);
                        case "*":
                            return ExecAst(opNode.LNode) * ExecAst(opNode.RNode);
                        case "/":
                            return ExecAst(opNode.LNode) / ExecAst(opNode.RNode);
                        case "%":
                            return ExecAst(opNode.LNode) % ExecAst(opNode.RNode);
                        case "^":
                            return Math.Pow(ExecAst(opNode.LNode), ExecAst(opNode.RNode));
                        case ">":
                            return (ExecAst(opNode.LNode) > ExecAst(opNode.RNode)) ? 1 : 0;
                        case "<":
                            return (ExecAst(opNode.LNode) < ExecAst(opNode.RNode)) ? 1 : 0;
                        case ">=":
                            return (ExecAst(opNode.LNode) >= ExecAst(opNode.RNode)) ? 1 : 0;
                        case "<=":
                            return (ExecAst(opNode.LNode) <= ExecAst(opNode.RNode)) ? 1 : 0;
                        case "==":
                            return IsNotZero(ExecAst(opNode.LNode) - ExecAst(opNode.RNode)) ? 0 : 1;
                        case "!=":
                            return  IsNotZero(ExecAst(opNode.LNode) - ExecAst(opNode.RNode)) ? 1 : 0;
                        case "!":
                            return IsNotZero(ExecAst(opNode.LNode)) ? 0 : 1;
                        case "&&":
                            return (IsNotZero(ExecAst(opNode.LNode)) && IsNotZero(ExecAst(opNode.RNode))) ? 1 : 0;
                        case "||":
                            return (IsNotZero(ExecAst(opNode.LNode)) || IsNotZero(ExecAst(opNode.RNode))) ? 1 : 0;
                    }
                    break;

                case AstFunctionNode funcNode:
                    try
                    {
                        if (funcNode.Id == "set")
                        {
                            AstVariableNode varNode = funcNode.Args[0] as AstVariableNode;
                            string varName = varNode?.Id ?? throw new ExpressionException($"The \"{funcNode.Id}()\"  first parameter is the variable name.");

                            double varVal = ExecAst(funcNode.Args[1]);
                            Variables[varName] = varVal;
                            return 0;
                        }
                        else if (funcNode.Id == "if")
                        {
                            IAstNode condNode = funcNode.Args[0];
                            double condNodeResult = ExecAst(condNode);
                            if (Math.Abs(condNodeResult) <= 1e-5)
                            {
                                return ExecAst(funcNode.Args[2]);
                            }
                            else
                            {
                                return ExecAst(funcNode.Args[1]);
                            }
                        }
                        else if(funcNode.Id == "smooth")
                        {
                            /* Todo?
                            AstVariableNode varNode = funcNode.Args[0] as AstVariableNode;
                            string varName = varNode?.Id ?? throw new ExpressionException($"The \"{funcNode.Id}()\" first parameter is the variable name.");
                            double varVal = ExecAst(funcNode.Args[0]);

                            return SmoothMath.SmoothVariable(varName, varVal);
                            */
                            throw new ExpressionException($"not support function smooth() yet");
                        }
                        else
                        {
                            if (Functions.TryGetValue(funcNode.Id, out var func))
                            {
                                return func(funcNode.Args.Select(x=>ExecAst(x)).ToList()) is double d ? d : double.NaN;
                            }
                            else
                            {
                                throw new ExpressionException($"No function found. Fucntion: {funcNode.Id}");
                            }
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new ExpressionException($"The function is missing a parameter. Fucntion: {funcNode.Id}");
                    }
            }

            return double.NaN;
        }

        public Func<double> ConvertAstToComplexLambda(IAstNode root,Func<string, double> variableRequireFunc,Func<string,List<double>, double> funcRequireFunc)
        {
            switch (root)
            {
                case AstNumberNode numberNode:
                    return () => numberNode.Number;
                case AstVariableNode varNode:
                    if (Constants.TryGetValue(varNode.Id, out var val))
                        return () => variableRequireFunc(varNode.Id);
                    else if (Variables.TryGetValue(varNode.Id, out val))
                        return () => variableRequireFunc(varNode.Id);
                    else
                        return () => 0;

                case AstOpNode opNode:
                    var leftValue = ConvertAstToComplexLambda(opNode.LNode, variableRequireFunc, funcRequireFunc);
                    var rightValue = ConvertAstToComplexLambda(opNode.RNode, variableRequireFunc, funcRequireFunc);

                    switch (opNode.Op)
                    {
                        case "+":
                            return () => leftValue() + rightValue();
                        case "-":
                            return () => leftValue() - rightValue();
                        case "*":
                            return () => leftValue() * rightValue();
                        case "/":
                            return () => leftValue() / rightValue();
                        case "%":
                            return () => leftValue() % rightValue();
                        case "^":
                            return () => Math.Pow(leftValue() , rightValue());
                        case ">":
                            return () => leftValue() > rightValue() ? 1 : 0;
                        case "<":
                            return () => leftValue() < rightValue() ? 1 : 0;
                        case ">=":
                            return () => leftValue() >= rightValue() ? 1 : 0;
                        case "<=":
                            return () => leftValue() <= rightValue() ? 1 : 0;
                        case "==":
                            return () => leftValue() == rightValue() ? 1 : 0;
                        case "!=":
                            return () => leftValue() != rightValue() ? 1 : 0;
                        case "!":
                            return () => IsNotZero(leftValue()) ? 1 : 0;
                        case "&&":
                            return () => IsNotZero(leftValue()) && IsNotZero(rightValue()) ? 1 : 0;
                        case "||":
                            return () => IsNotZero(leftValue()) || IsNotZero(rightValue()) ? 1 : 0;
                    }
                    break;

                case AstFunctionNode funcNode:
                    try
                    {
                        if (funcNode.Id == "set")
                        {
                            AstVariableNode varNode = funcNode.Args[0] as AstVariableNode;
                            string varName = varNode?.Id ?? throw new ExpressionException($"The \"{funcNode.Id}()\"  first parameter is the variable name.");

                            var varVal = ConvertAstToComplexLambda(funcNode.Args[1],variableRequireFunc, funcRequireFunc)();
                            Variables[varName] = varVal;
                            return () => 0;
                        }
                        else if (funcNode.Id == "if")
                        {
                            var condExpr = ConvertAstToComplexLambda(funcNode.Args[0], variableRequireFunc, funcRequireFunc);
                            var falseResult = ConvertAstToComplexLambda(funcNode.Args[2], variableRequireFunc, funcRequireFunc);
                            var trueResult = ConvertAstToComplexLambda(funcNode.Args[1], variableRequireFunc, funcRequireFunc);

                            var condResult = condExpr();

                            return () => (condResult is double d ? (d <= 1e-5) : (bool.Parse(condResult.ToString()))) ? falseResult() : trueResult();
                        }
                        else if (funcNode.Id == "smooth")
                        {
                            /* Todo?
                            AstVariableNode varNode = funcNode.Args[0] as AstVariableNode;
                            string varName = varNode?.Id ?? throw new ExpressionException($"The \"{funcNode.Id}()\" first parameter is the variable name.");
                            double varVal = ExecAst(funcNode.Args[0]);

                            return SmoothMath.SmoothVariable(varName, varVal);
                            */
                            throw new ExpressionException($"not support function smooth() yet");
                        }
                        else
                        {
                            return () => funcRequireFunc(funcNode.Id,funcNode.Args.Select(arg=> ConvertAstToComplexLambda(arg, variableRequireFunc, funcRequireFunc)()).ToList());
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new ExpressionException($"The function is missing some parameters. Fucntion: {funcNode.Id}");
                    }
            }

            return () => double.NaN;
        }

        public Func<double> ConvertAstToComplexLambdaWithDefault(IAstNode root)
            => ConvertAstToComplexLambda(
                root,
                var => Variables.TryGetValue(var, out var val) ? val : double.NaN,
                (funcName, args) => Functions.TryGetValue(funcName, out var func) ? func(args) : double.NaN);
    }
}
