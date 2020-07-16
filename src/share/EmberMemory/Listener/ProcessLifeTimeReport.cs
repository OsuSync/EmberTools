using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemory.Listener
{
    public struct ProcessLifeTimeReport<T>
    {
        public bool Terminated { get; set; }
        public T Report { get; set; }
    }
}
