using System;
using System.Collections.Generic;

namespace EmberKernel.Services.Statistic.Formatter.DefaultImpl
{
    public struct DefaultFormat
    {
        private static int GENID = 0;

        public DefaultFormat(HashSet<string> requestVariables, Func<string> formatFunction)
        {
            Id = GENID++;
            RequestVariables = requestVariables;
            FormatFunction = formatFunction;
        }

        public int Id { get; }
        public HashSet<string> RequestVariables { get; set; }
        public Func<string> FormatFunction { get; set; }
    }
}
