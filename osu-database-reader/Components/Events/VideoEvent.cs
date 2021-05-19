namespace osu_database_reader.Components.Events
{
    public class VideoEvent : EventBase
    {
        internal VideoEvent()
        {
        }

        public int Offset { get; internal set; }
        public string Path { get; internal set; }
    }
}