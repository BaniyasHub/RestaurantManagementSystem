using Google.Protobuf.WellKnownTypes;

namespace Server.Tools
{
    public class TimeTool : ITimeTool
    {
        public Timestamp ConvertDateTimeToTimeStamp(DateTime time)
        {
            var unixTimeMS = new DateTimeOffset(time.AddHours(2)).ToUnixTimeMilliseconds();

            var seconds = unixTimeMS / 1000;
            var nanos = (int)((unixTimeMS % 1000) * 1e6);
            var TimeStamp = new Timestamp() { Seconds = seconds, Nanos = nanos };
           
            return TimeStamp;

        }
    }
}
