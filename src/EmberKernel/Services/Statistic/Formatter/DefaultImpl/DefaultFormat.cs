using System;
using System.Collections.Generic;

namespace EmberKernel.Services.Statistic.Formatter.DefaultImpl
{
    public struct DefaultFormat
    {

        public DefaultFormat(string id, HashSet<string> requestVariables, Func<string> formatFunction)
        {
            Id = id;
            RequestVariables = requestVariables;
            FormatFunction = formatFunction;
        }

        public string Id { get; }
        public HashSet<string> RequestVariables { get; set; }
        public Func<string> FormatFunction { get; set; }
    }
}
