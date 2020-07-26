using EmberKernel.Services.Statistic.Format;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.Statistic.Formatter.DefaultImpl
{
    public class RegisterFormat
    {
        private static int GENID = 0;

        public RegisterFormat() => Id = GENID++;

        public int Id { get; }
        public string RawFormatContent { get; set; }
        public HashSet<string> RequestVariables { get; set; }
        public Func<string> FormatFunction { get; set; }
        public Type ContainerType { get; set; }
    }

    public class RegisterFormat<TContainer> : RegisterFormat where TContainer : IFormatContainer
    {
        public RegisterFormat():base() => ContainerType = typeof(TContainer);
    }
}
