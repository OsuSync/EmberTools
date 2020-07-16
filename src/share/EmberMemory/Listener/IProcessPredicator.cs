using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EmberMemory.Listener
{
    public interface IProcessPredicator<T> where T : Event<T>
    {
        /// <summary>
        /// Set a process name for search
        /// </summary>
        string FilterProcessName { get; }
        /// <summary>
        /// Pass a process match the process name
        /// </summary>
        /// <param name="process">Process instance</param>
        /// <returns>Should not stop the searching</returns>
        T MatchProcess(Process process);
    }
}
