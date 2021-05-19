namespace osu_database_reader.Components.Events
{
    public class BreakEvent : EventBase
    {
        internal BreakEvent()
        {
        }

        public double StartTime { get; internal set; }
        public double EndTime { get; internal set; }

        public double BreakTime => EndTime - StartTime;
    }
}