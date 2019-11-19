using System;
using System.Collections.Generic;
using System.Text;

namespace EmberCore
{
    public class CoreAppSetting
    {
        public bool CheckUpdate { get; set; }
        public bool CheckComponentUpdate { get; set; }
        public string Locale { get; set; }
        public string PluginsFolder { get; set; }
        public int CommandSourceOperationTimeLimit { get; set; }
    }

    public class Logging
    {
        public bool IncludeScopes { get; set; }
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string EmberCore { get; set; }
    }
}
