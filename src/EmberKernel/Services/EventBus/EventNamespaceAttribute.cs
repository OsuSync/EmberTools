﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.EventBus
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EventNamespaceAttribute : Attribute
    {
        public EventNamespaceAttribute(string @namespace)
        {
            this.Namespace = @namespace;
        }
        public string Namespace { get; }
    }
}
