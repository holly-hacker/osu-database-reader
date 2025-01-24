using System;
using System.Diagnostics;
using osu.Shared;

namespace osu_database_reader.Components.HitObjects
{
    public abstract class HitObject
    {
        public int X, Y;  //based on a 512x384 field
        public int Time;
        public HitObjectType Type;
        public HitSound HitSound;

        public bool IsNewCombo => Type.HasFlag(HitObjectType.NewCombo);

        //automatically returns the correct type
        public static HitObject FromString(string s)
        {
            string[] split = s.Split(',');
            var t = (HitObjectType) int.Parse(split[3], Constants.NumberFormat);

            HitObject h;
            switch (t & (HitObjectType)0b1000_1011) {
                case HitObjectType.Normal:
                    h = new HitObjectCircle();
                    if (split.Length > 5)
                        ((HitObjectCircle) h).SoundSampleData = split[5];
                    break;
                case HitObjectType.Slider:
                    h = new HitObjectSlider();
                    ((HitObjectSlider) h).ParseSliderSegments(split[5]);
                    ((HitObjectSlider) h).RepeatCount = int.Parse(split[6], Constants.NumberFormat);
                    if (split.Length > 7)
                        ((HitObjectSlider) h).Length = double.Parse(split[7], Constants.NumberFormat);
                    //if (split.Length > 8)
                    //    (h as HitObjectSlider).HitSoundData = split[8];
                    //if (split.Length > 9)
                    //    (h as HitObjectSlider).SoundSampleData = split[9];
                    //if (split.Length > 10)
                    //    (h as HitObjectSlider).MoreSoundSampleData = split[10];
                    break;
                case HitObjectType.Spinner:
                    h = new HitObjectSpinner();
                    ((HitObjectSpinner) h).EndTime = int.Parse(split[5]);
                    if (split.Length > 6)
                        ((HitObjectSpinner) h).SoundSampleData = split[6];
                    break;
                case HitObjectType.Hold:
                    throw new NotImplementedException("Hold notes are not yet parsed.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(t), "Bad hitobject type");
            }

            // parsed as decimal but cast to int to match osu! behavior
            h.X = (int)double.Parse(split[0], Constants.NumberFormat);
            h.Y = (int)double.Parse(split[1], Constants.NumberFormat);
            h.Time = (int)double.Parse(split[2], Constants.NumberFormat);
            h.Type = t;
            h.HitSound = (HitSound)int.Parse(split[4]);

            return h;
        }
    }
}
