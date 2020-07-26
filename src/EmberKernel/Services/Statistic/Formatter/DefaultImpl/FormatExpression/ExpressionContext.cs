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

        public ConcurrentDictionary<string, ValueBase> Constants { get; } = new ConcurrentDictionary<string, ValueBase>();
        public ConcurrentDictionary<string, ValueBase> Variables { get; } = new ConcurrentDictionary<string, ValueBase>();
        public ConcurrentDictionary<string, Func<List<ValueBase>, ValueBase>> Functions = new ConcurrentDictionary<string, Func<List<ValueBase>, ValueBase>>();

        public ExpressionContext()
        {
            Constants["pi"] = ValueBase.Create(Math.PI);
            Constants["e"] = ValueBase.Create(Math.E);
            Constants["inf"] = ValueBase.Create(double.PositiveInfinity);

            Functions["sin"] = (args) => ValueBase.Create(Math.Sin(args[0]));
            Functions["cos"] = (args) => ValueBase.Create(Math.Cos(args[0]));
            Functions["tan"] = (args) => ValueBase.Create(Math.Tan(args[0]));
            Functions["asin"] = (args) => ValueBase.Create(Math.Asin(args[0]));
            Functions["acos"] = (args) => ValueBase.Create(Math.Acos(args[0]));
            Functions["atan"] = (args) => ValueBase.Create(Math.Atan(args[0]));
            Functions["pow"] = (args) => ValueBase.Create(Math.Pow(args[0], args[1]));
            Functions["sqrt"] = (args) => ValueBase.Create(Math.Sqrt(args[0]));
            Functions["abs"] = (args) => ValueBase.Create(Math.Abs(args[0]));
            Functions["max"] = (args) => ValueBase.Create(Math.Max(args[0], args[1]));
            Functions["min"] = (args) => ValueBase.Create(Math.Min(args[0], args[1]));
            Functions["exp"] = (args) => ValueBase.Create(Math.Exp(args[0]));
            Functions["log"] = (args) => ValueBase.Create(Math.Log(args[0]));
            Functions["log10"] = (args) => ValueBase.Create(Math.Log10(args[0]));
            Functions["floor"] = (args) => ValueBase.Create(Math.Floor(args[0]));
            Functions["ceil"] = (args) => ValueBase.Create(Math.Ceiling(args[0]));
            Functions["round"] = (args) => ValueBase.Create(Math.Round(args[0], (int)args[1], MidpointRounding.AwayFromZero));
            Functions["sign"] = (args) => ValueBase.Create(Math.Sign(args[0]));
            Functions["truncate"] = (args) => ValueBase.Create(Math.Truncate(args[0]));
            Functions["clamp"] = (args) => ValueBase.Create(Math.Max(Math.Min(args[0], args[2]), args[1]));
            Functions["lerp"] = (args) => ValueBase.Create((1 - args[2]) * args[0] + args[2] * args[1]);
            Functions["random"] = (args) =>
                ValueBase.Create(args.Count >= 2 ? args[0] + _random.NextDouble() * (args[1] - args[0]) : _random.NextDouble());
            Functions["getTime"] = (args) =>
                ValueBase.Create(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
            Functions["mod"] = (args) => ValueBase.Create(args[0] % args[1]);
            Functions["isnan"] = (args) => ValueBase.Create(double.IsNaN(args[0]) ? 1 : 0);
            Functions["isinf"] = (args) => ValueBase.Create(double.IsInfinity(args[0]) ? 1 : 0);
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
                            Variables[varName] = ValueBase.Create(varVal);
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
                                return func(funcNode.Args.Select(x=>ValueBase.Create(ExecAst(x))).OfType<ValueBase>().ToList());
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

        public Func<ValueBase> ConvertAstToComplexLambda(IAstNode root,Func<string, ValueBase> variableRequireFunc,Func<string,List<ValueBase>, ValueBase> funcRequireFunc)
        {
            switch (root)
            {
                case AstStringNode stringNode:
                    return () => ValueBase.Create(stringNode.String);
                case AstNumberNode numberNode:
                    return () => ValueBase.Create(numberNode.Number);
                case AstVariableNode varNode:
                    if (Constants.TryGetValue(varNode.Id, out var val))
                        return () => variableRequireFunc(varNode.Id);
                    else if (Variables.TryGetValue(varNode.Id, out val))
                        return () => variableRequireFunc(varNode.Id);
                    else
                        return () => NumberValue.Zero;

                case AstOpNode opNode:
                    var leftValue = ConvertAstToComplexLambda(opNode.LNode, variableRequireFunc, funcRequireFunc);
                    var rightValue = ConvertAstToComplexLambda(opNode.RNode, variableRequireFunc, funcRequireFunc);

                    ValueBase NumberOpCheck(Func<double,double,double> func)
                    {
                        var a = leftValue();
                        if (a.ValueType != ValueBase.Type.Number)
                            throw new ExpressionException("Invaild Number Operations.");
                        var b = rightValue();
                        if (b.ValueType != ValueBase.Type.Number)
                            throw new ExpressionException("Invaild Number Operations.");

                        return ValueBase.Create(func(a, b));
                    }

                    ValueBase BoolOpCheck(Func<double, double, bool> func)
                    {
                        var a = leftValue();
                        if (a.ValueType != ValueBase.Type.Number)
                            throw new ExpressionException("Invaild Number Operations.");
                        var b = rightValue();
                        if (b.ValueType != ValueBase.Type.Number)
                            throw new ExpressionException("Invaild Number Operations.");

                        return func(a, b) ? NumberValue.One : NumberValue.Zero;
                    }

                    ValueBase NumberOpCheck2(Func<double, double> func)
                    {
                        var a = leftValue();
                        if (a.ValueType != ValueBase.Type.Number)
                            throw new ExpressionException("Invaild Number Operations.");
                        
                        return ValueBase.Create(func(a));
                    }

                    switch (opNode.Op)
                    {
                        case "+":
                            return () =>
                            {
                                var a = leftValue();
                                var b = rightValue();

                                if (a.ValueType == ValueBase.Type.String || b.ValueType == ValueBase.Type.String)
                                    return ValueBase.Create(a.ValueToString() + b.ValueToString());

                                return ValueBase.Create(((NumberValue)a).Value + ((NumberValue)b).Value);
                            };
                        case "-":
                            return () => NumberOpCheck((a,b) => a - b);
                        case "*":
                            return () => NumberOpCheck((a, b) => a * b);
                        case "/":
                            return () => NumberOpCheck((a, b) => a / b);
                        case "%":
                            return () => NumberOpCheck((a, b) => a % b);
                        case "^":
                            return () => NumberOpCheck((a, b) => Math.Pow(a, b));
                        case ">":
                            return () => BoolOpCheck((a, b) => a > b);
                        case "<":
                            return () => BoolOpCheck((a, b) => a < b);
                        case ">=":
                            return () => BoolOpCheck((a, b) => a >= b);
                        case "<=":
                            return () => BoolOpCheck((a, b) => a <= b);
                        case "==":
                            return () => BoolOpCheck((a, b) => a == b);
                        case "!=":
                            return () => BoolOpCheck((a, b) => a != b);
                        case "!":
                            return () => NumberOpCheck2(a => IsNotZero(a) ? NumberValue.One : NumberValue.Zero);
                        case "&&":
                            return () => BoolOpCheck((a, b)=>IsNotZero(a));
                        case "||":
                            return () => BoolOpCheck((a, b) => IsNotZero(a));
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
                            return () => NumberValue.Zero;
                        }
                        else if (funcNode.Id == "if")
                        {
                            var condExpr = ConvertAstToComplexLambda(funcNode.Args[0], variableRequireFunc, funcRequireFunc);
                            var falseResult = ConvertAstToComplexLambda(funcNode.Args[2], variableRequireFunc, funcRequireFunc);
                            var trueResult = ConvertAstToComplexLambda(funcNode.Args[1], variableRequireFunc, funcRequireFunc);

                            return () => condExpr() <= 1e-5 ? falseResult() : trueResult();
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

            return () => NumberValue.Nan;
        }

        public Func<ValueBase> ConvertAstToComplexLambdaWithDefault(IAstNode root)
            => ConvertAstToComplexLambda(
                root,
                var => Variables.TryGetValue(var, out var val) ? val : NumberValue.Nan,
                (funcName, args) => Functions.TryGetValue(funcName, out var func) ? func(args) : NumberValue.Nan);
    }
}
