using System;
using System.Collections.Generic;

namespace EmberKernel.Services.Statistic.Formatter.DefaultImpl
{
    public class DefaultFormat
    {
        private static int GENID = 0;

        public DefaultFormat() => Id = GENID++;

        public int Id { get; }
        public HashSet<string> RequestVariables { get; set; }
        public Func<string> FormatFunction { get; set; }
    }
}
