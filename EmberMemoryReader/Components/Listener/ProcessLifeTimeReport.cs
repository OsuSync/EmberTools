using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Listener
{
    public struct ProcessLifeTimeReport<T>
    {
        public bool Terminated { get; set; }
        public T Report { get; set; }
    }
}
