using EmberKernel.Services.EventBus;
using System.Diagnostics;

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
