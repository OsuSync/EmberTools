using System.Threading.Tasks;

namespace EmberKernel.Services.Statistic.Format
{
    public interface IFormatContainer
    {
        ValueTask FormatUpdated(string format, string value);
    }
}
