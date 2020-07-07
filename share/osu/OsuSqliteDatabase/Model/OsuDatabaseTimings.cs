namespace OsuSqliteDatabase.Model
{
    public class OsuDatabaseTimings
    {
        public int Id { get; set; }
        public int OsuDatabaseBeatmapId { get; set; }
        public OsuDatabaseBeatmap OsuDatabaseBeatmap { get; set; }
        public double BeatPreMinute { get; set; }
        public double Offset { get; set; }
        public bool Inherited { get; set; }

    }
}
