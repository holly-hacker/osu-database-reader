using System;

namespace osu_database_reader.Components.Events
{
    public abstract class EventBase
    {
        public string Line { get; internal set; }

        public static EventBase FromString(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new ArgumentException(line);

            try
            {
                var split = line.Split(',');

                // https://github.com/ppy/osu/blob/7654df94f6f37b8382be7dfcb4f674e03bd35427/osu.Game/Beatmaps/Formats/LegacyStoryboardDecoder.cs#L103
                // https://github.com/ppy/osu/blob/7654df94f6f37b8382be7dfcb4f674e03bd35427/osu.Game/Beatmaps/Formats/LegacyBeatmapDecoder.cs#L301
                switch (split[0])
                {
                    case "0":
                    case "Background":
                    {
                        // fields 1, 3 and 4 are unknown
                        var path = split[2].Trim('"');

                        return new BackgroundEvent
                        {
                            Path = path,
                            Line = line,
                        };
                    }
                    case "1":
                    case "Video":
                    {
                        var offset = int.Parse(split[1], Constants.NumberFormat);
                        var path = split[2].Trim('"');

                        return new VideoEvent
                        {
                            Offset = offset,
                            Path = path,
                            Line = line,
                        };
                    }
                    case "2":
                    case "Break":
                    {
                        var start = double.Parse(split[1], Constants.NumberFormat);
                        var end = double.Parse(split[2], Constants.NumberFormat);

                        return new BreakEvent
                        {
                            StartTime = start,
                            EndTime = end,
                            Line = line,
                        };
                    }
                    /*
                    case "3":
                    case "Colour":
                    {
                        // TODO
                    }
                    */
                    case "4":
                    case "Sprite":
                    {
                        var layer = split[1];
                        var origin = split[2];
                        var path = split[3].Trim('"');
                        var x = float.Parse(split[4], Constants.NumberFormat);
                        var y = float.Parse(split[5], Constants.NumberFormat);

                        return new SpriteEvent
                        {
                            Layer = layer,
                            Origin = origin,
                            Path = path,
                            X = x,
                            Y = y,
                            Line = line,
                        };
                    }
                    case "5":
                    case "Sample":
                    {
                        var time = double.Parse(split[1], Constants.NumberFormat);
                        var layer = split[2];
                        var path = split[3];

                        var volume = split.Length > 3 ? float.Parse(split[4], Constants.NumberFormat) : (float?) null;

                        return new SampleEvent
                        {
                            Time = time,
                            Layer = layer,
                            Path = path,
                            Volume = volume,
                            Line = line,
                        };
                    }
                    case "6":
                    case "Animation":
                    {
                        var layer = split[1];
                        var origin = split[2];
                        var path = split[3].Trim('"');
                        var x = float.Parse(split[4], Constants.NumberFormat);
                        var y = float.Parse(split[5], Constants.NumberFormat);
                        var frameCount = int.Parse(split[6], Constants.NumberFormat);
                        var frameDelay = double.Parse(split[7], Constants.NumberFormat);

                        var loopType = split.Length > 8 ? split[8] : null;

                        return new AnimationEvent
                        {
                            Layer = layer,
                            Origin = origin,
                            Path = path,
                            X = x,
                            Y = y,
                            FrameCount = frameCount,
                            FrameDelay = frameDelay,
                            LoopType = loopType,
                            Line = line,
                        };
                    }
                    default:
                    {
                        return new FallbackEvent
                        {
                            Line = line,
                        };
                    }
                }
            }
            catch
            {
                return new FallbackEvent
                {
                    Line = line,
                };
            }
        }

        private class FallbackEvent : EventBase
        {
        }
    }
}