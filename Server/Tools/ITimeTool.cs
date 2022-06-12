using Google.Protobuf.WellKnownTypes;

namespace Server.Tools
{
    public interface ITimeTool
    {
        Timestamp ConvertDateTimeToTimeStamp(DateTime time);
    }
}
