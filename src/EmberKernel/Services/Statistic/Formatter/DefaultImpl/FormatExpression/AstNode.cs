using System;
using System.Collections.Generic;

namespace EmberKernel.Services.Statistic.Formatter.DefaultImpl.FormatExpression
{

    public interface IAstNode
    {

    }

    public class AstOpNode : IAstNode
    {
        public string Op { get;}
        public IAstNode RNode { get; set; }
        public IAstNode LNode { get; set; }

        public AstOpNode(string op)
        {
            Op = op;
        }
    }

    public class AstVariableNode : IAstNode
    {
        public string Id { get; }

        public AstVariableNode(string id)
        {
            Id = id;
        }
    }

    public class AstFunctionNode : IAstNode
    {
        public string Id { get; }
        public List<IAstNode> Args { get; set; }

        public AstFunctionNode(string id,List<IAstNode> args)
        {
            Id = id;
            Args = args;
        }
    }

    public class AstNumberNode : IAstNode
    {
        public double Number { get; }

        public AstNumberNode(double num)
        {
            Number = num;
        }
    }
}

