using System;
using System.Globalization;

namespace osu_database_reader.Components.Player
{
    public struct ReplayFrame
    {
        public float X;
        public float Y;
        public int TimeDiff;
        public int TimeAbs;
        public Keys Keys;

        internal ReplayFrame(string line, ref int offset)
        {
            var splitted = line.Split('|');

            TimeDiff = int.Parse(splitted[0], CultureInfo.InvariantCulture);
            TimeAbs = offset += TimeDiff;
            X = float.Parse(splitted[1], CultureInfo.InvariantCulture);
            Y = float.Parse(splitted[2], CultureInfo.InvariantCulture);
            Keys = (Keys)int.Parse(splitted[3]);
        }

        public override string ToString() => $"{TimeDiff}|{X.ToString(CultureInfo.InvariantCulture)}|{Y.ToString(CultureInfo.InvariantCulture)}|{(int)Keys}";

        internal static ReplayFrame[] FromStrings(ref string str)
        {
            var splitted = str.Split(',');
            var arr = new ReplayFrame[splitted.Length];

            int lastOffset = 0;
            for (int i = 0; i < splitted.Length; i++)
                if (!string.IsNullOrEmpty(splitted[i]))
                    arr[i] = new ReplayFrame(splitted[i], ref lastOffset);

            return arr;
        }
    }
}
