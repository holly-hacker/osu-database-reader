using System;
using System.Diagnostics;
using osu_database_reader.IO;

namespace osu_database_reader.BinaryFiles.Parts
{
    public class Player
    {
        public int PlayerId;
        public string PlayerName;
        public byte UtcOffset;  //need to substract 24 from this to be usable
        public byte CountryByte;
        public PlayerRank PlayerRank;
        public GameMode GameMode;
        public float Longitude, Latitude;   //position in the world
        public int GlobalRank;
        public DateTime Unknown1;   //TODO: name this. Last update time?

        public static Player ReadFromReader(CustomBinaryReader r) {
            Player p = new Player();

            p.PlayerId = r.ReadInt32();
            p.PlayerName = r.ReadString();
            p.UtcOffset = r.ReadByte();
            p.CountryByte = r.ReadByte();   //TODO: create Country enum

            byte b = r.ReadByte();
            p.PlayerRank = (PlayerRank)(b & 0b0001_1111);
            p.GameMode = (GameMode)((b & 0b1110_0000) >> 5);
            Debug.Assert((byte)p.GameMode <= 3, $"GameMode is byte {(byte)p.GameMode}, should be between 0 and 3");

            p.Longitude = r.ReadSingle();
            p.Latitude = r.ReadSingle();
            p.GlobalRank = r.ReadInt32();
            p.Unknown1 = r.ReadDateTime();

            return p;
        }
    }
}
