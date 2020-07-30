using EmberKernel.Services.EventBus;

namespace Statistic.Abstract.Events
{
    public class FormatCreatedEvent : Event<FormatCreatedEvent>
    {
        public string Name { get; set; }
        public string Format { get; set; }
    }
}
