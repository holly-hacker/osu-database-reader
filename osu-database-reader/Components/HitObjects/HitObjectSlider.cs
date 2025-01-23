﻿using System.Collections.Generic;
using System.Diagnostics;

namespace osu_database_reader.Components.HitObjects
{
    public class HitObjectSlider : HitObject
    {
        public CurveType CurveType;
        public List<Vector2> Points = new();  //does not include initial point!
        public int RepeatCount;
        public double Length; //seems to be length in o!p, so it doesn't have to be calculated?

        public void ParseSliderSegments(string sliderString)
        {
            string[] split = sliderString.Split('|');
            foreach (var s in split) {
                if (s.Length == 1)
                {
                    //curve type
                    CurveType = s[0] switch
                    {
                        'L' => CurveType.Linear,
                        'C' => CurveType.Catmull,
                        'P' => CurveType.Perfect,
                        'B' => CurveType.Bezier,
                        _ => CurveType,
                    };
                    continue;
                }
                string[] split2 = s.Split(':');
                Debug.Assert(split2.Length == 2);

                // beatmapID: 4615284 has float values for this
                int firstNum;
                int secondNum;
                if (split2[0].Contains(".")) firstNum = (int)double.Parse(split2[0]);
                else firstNum = int.Parse(split2[0]);
                if (split2[1].Contains(".")) secondNum = (int)double.Parse(split2[1]);
                else secondNum = int.Parse(split2[1]);
                Points.Add(new Vector2(
                    firstNum,
                    secondNum));
            }
        }
    }
}
