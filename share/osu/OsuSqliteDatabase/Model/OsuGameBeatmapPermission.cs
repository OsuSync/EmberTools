﻿namespace OsuSqliteDatabase.Model
{
    public enum OsuGameBeatmapPermission
    {
        None = 0,
        Normal = 1,
        BAT = 2,
        Supporter = 4,
        Friend = 8,
        Peppy = 16,
        Tournament = 32
    }
}
