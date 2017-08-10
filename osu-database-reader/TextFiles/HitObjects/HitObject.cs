﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu_database_reader.IO;

namespace osu_database_reader.TextFiles.HitObjects
{
    public abstract class HitObject
    {
        //nfi so parsing works on all cultures
        private static readonly NumberFormatInfo NumberFormat = new CultureInfo(@"en-US", false).NumberFormat;

        public int X, Y;  //based on a 512x384 field
        public int Time;
        public HitObjectType Type;
        public HitSound HitSound;

        public bool IsNewCombo => Type.HasFlag(HitObjectType.NewCombo);

        //automatically returns the correct type
        public static HitObject FromString(string s)
        {
            string[] split = s.Split(',');
            HitObjectType t = (HitObjectType) int.Parse(split[3]);

            HitObject h = null;
            switch (t & (HitObjectType)0b1000_1011) {
                case HitObjectType.Normal:
                    h = new HitObjectCircle();
                    if (split.Length > 5)
                        (h as HitObjectCircle).SoundSampleData = split[5];
                    break;
                case HitObjectType.Slider:
                    h = new HitObjectSlider();
                    (h as HitObjectSlider).ParseSliderSegments(split[5]);
                    (h as HitObjectSlider).RepeatCount = int.Parse(split[6]);
                    if (split.Length > 7)
                        (h as HitObjectSlider).Length = double.Parse(split[7], NumberFormat);
                    //if (split.Length > 8)
                    //    (h as HitObjectSlider).HitSoundData = split[8];
                    //if (split.Length > 9)
                    //    (h as HitObjectSlider).SoundSampleData = split[9];
                    //if (split.Length > 10)
                    //    (h as HitObjectSlider).MoreSoundSampleData = split[10];
                    break;
                case HitObjectType.Spinner:
                    h = new HitObjectSpinner();
                    (h as HitObjectSpinner).EndTime = int.Parse(split[5]);
                    if (split.Length > 6)
                        (h as HitObjectSpinner).SoundSampleData = split[6];
                    break;
                case HitObjectType.Hold:
                    throw new NotImplementedException("Hold notes are not yet parsed.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(t), "Bad hitobject type");
            }

            //note: parsed as decimal but cast to int in osu!
            if (h != null) {
                h.X = int.Parse(split[0], NumberFormat);
                h.Y = int.Parse(split[1], NumberFormat);
                h.Time = int.Parse(split[2], NumberFormat);
                h.Type = t;
                h.HitSound = (HitSound)int.Parse(split[4]);
            }
            else Debug.Fail("unhandled hitobject type");

            return h;
        }
    }
}
