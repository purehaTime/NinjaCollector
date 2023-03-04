using Google.Protobuf.WellKnownTypes;

namespace GrpcHelper
{
    public static class DateTimeConvert
    {
        public static DateTime ToDateTime(this Timestamp timestamp)
        {
            return timestamp.ToDateTime();
        }

        public static Timestamp ToTimeStamp(this DateTime datetime)
        {
            return Timestamp.FromDateTime(datetime);
        }
    }
}
