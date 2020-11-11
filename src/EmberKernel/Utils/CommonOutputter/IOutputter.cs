using System;
using System.Threading.Tasks;

namespace EmberKernel.Utils.CommonOutputter
{
    public interface IOutputter:IDisposable
    {
        string Name { get; }

        ValueTask WriteAsync(string content);
        ValueTask CleanAsync();
    }
}