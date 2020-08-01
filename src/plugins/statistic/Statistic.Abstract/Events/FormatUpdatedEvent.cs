namespace Statistic.Abstract.Events
{
    public class FormatUpdatedEvent : FormatEvent<FormatUpdatedEvent>
    {
        public string NewName { get; set; }
    }
}
