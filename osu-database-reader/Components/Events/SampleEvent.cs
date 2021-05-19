namespace osu_database_reader.Components.Events
{
    public class SampleEvent : EventBase
    {
        internal SampleEvent()
        {
        }

        public double Time { get; internal set; }
        public string Layer { get; internal set; }
        public string Path { get; internal set; }
        public float? Volume { get; internal set; }
    }
}