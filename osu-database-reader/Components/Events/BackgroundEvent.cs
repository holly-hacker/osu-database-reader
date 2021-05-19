namespace osu_database_reader.Components.Events
{
    public class BackgroundEvent : EventBase
    {
        internal BackgroundEvent()
        {
        }

        public string Path { get; internal set; }
    }
}