using System;

namespace osu_database_reader
{
    [Flags]
    public enum HitObjectType
    {
        Normal         = 0b0000_0001,   //I should really become consistent with these baseX styles
        Slider         = 0b0000_0010,
        NewCombo       = 0b0000_0100,
        Spinner        = 0b0000_1000,
        ColourHax      = 0b0111_0000,
        Hold           = 0b1000_0000,
    };

    [Flags]
    public enum HitSound
    {
        None    = 0,
        Normal  = 1,
        Whistle = 2,
        Finish  = 4,
        Clap    = 8,
    }

    public enum CurveType
    {
        Linear,
        Catmull,
        Bezier,
        Perfect
    }
}
