﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.Command.Models
{
    public class CommandArgument
    {
        public string Namespace { get; set; }
        public string Command { get; set; }
        public string Argument { get; set; }
    }
}
