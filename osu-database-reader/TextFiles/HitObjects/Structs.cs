using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_database_reader.TextFiles.HitObjects
{
    public struct Vector2
    {
        public int X, Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
