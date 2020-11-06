using System;

namespace EmberStatisticDatabase.Model
{
    public class RegisteredFormat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
        public string LastValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
