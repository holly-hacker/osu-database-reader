using System.Collections.Generic;
using System.Diagnostics;

namespace osu_database_reader.Components.HitObjects
{
    public class HitObjectSlider : HitObject
    {
        public CurveType CurveType;
        /// <summary> The points on this slider's curve, excluding the starting points. </summary>
        public List<Vector2> Points = new();
        public int RepeatCount;
        /// <summary> Length of the slider in osu!pixels. </summary>
        public double Length;

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

                Points.Add(new Vector2(
                    (int)double.Parse(split2[0]),
                    (int)double.Parse(split2[1])));
            }
        }
    }
}
