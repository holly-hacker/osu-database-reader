using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_database_reader
{
    public enum GameMode : byte
    {
        Standard = 0,
        Taiko = 1,
        CatchTheBeat = 2,
        Mania = 3
    }

    public enum Ranking : byte
    {
        XH = 0,     //silver SS
        SH = 1,     //silver S
        X = 2,      //SS
        S = 3,
        A = 4,
        B = 5,
        C = 6,
        D = 7,
        F = 8,      //Failed
        N = 9       //None
    }

    [Flags]
    public enum PlayerRank : byte
    {
        None = 0,       //not logged in?
        Default = 1,
        Bat = 2,        //not a vampire :3
        Supporter = 4,
        Mod = 8,
        SuperMod = 16   //peppy, blue color
    }

    public enum SubmissionStatus : byte
    {
        Unknown,
        NotSubmitted,
        Pending,        //both pending and graveyarded
        EditableCutoff, //not used anymore?
        Ranked,
        Approved
    }

    public enum Mods : int
    {
        None            = 0,
        NoFail          = 1,
        Easy            = 2,
        //NoVideo       = 4,
        Hidden          = 8,
        HardRock        = 16,
        SuddenDeath     = 32,
        DoubleTime      = 64,
        Relax           = 128,
        HalfTime        = 256,
        Nightcore       = 512, // Only set along with DoubleTime. i.e: NC only gives 576
        Flashlight      = 1024,
        Autoplay        = 2048,
        SpunOut         = 4096,
        Relax2          = 8192,  // Autopilot?
        Perfect         = 16384,
        Key4            = 32768,
        Key5            = 65536,
        Key6            = 131072,
        Key7            = 262144,
        Key8            = 524288,
        KeyMod          = Key4 | Key5 | Key6 | Key7 | Key8,
        FadeIn          = 1048576,
        Random          = 2097152,
        LastMod         = 4194304,
        FreeModAllowed  = NoFail | Easy | Hidden | HardRock | SuddenDeath | Flashlight | FadeIn | Relax | Relax2 | SpunOut | KeyMod,
        Key9            = 16777216,
        Key10           = 33554432,
        Key1            = 67108864,
        Key3            = 134217728,
        Key2            = 268435456
    }
}
