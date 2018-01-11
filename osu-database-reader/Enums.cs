using System;

namespace osu_database_reader
{
    public enum CurveType
    {
        Linear,
        Catmull,
        Bezier,
        Perfect
    }

    public enum BeatmapSection
    {
        _EndOfFile,
        General,
        Editor,
        Metadata,
        Difficulty,
        Events,
        TimingPoints,
        Colours,
        HitObjects,
    }

    [Flags]
    public enum Keys
    {
        None = 0,
        M1 = (1 << 0),
        M2 = (1 << 1),
        K1 = (1 << 2) + M1,
        K2 = (1 << 3) + M2,
        Smoke = 16,
    }
}
