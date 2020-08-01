using EmberKernel.Services.EventBus;

namespace Statistic.Abstract.Events
{
    public class FormatEvent<T> : Event<T>
        where T : FormatEvent<T>
    {
        public string Name { get; set; }
        public string Format { get; set; }
    }
}
