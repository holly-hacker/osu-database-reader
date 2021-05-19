namespace osu_database_reader.Components.Events
{
    public class AnimationEvent : EventBase
    {
        internal AnimationEvent()
        {
        }

        public string Layer { get; internal set; }
        public string Origin { get; internal set; }
        public string Path { get; internal set; }
        public float X { get; internal set; }
        public float Y { get; internal set; }
        public int FrameCount { get; internal set; }
        public double FrameDelay { get; internal set; }
        public string LoopType { get; internal set; }
    }
}