using System;

namespace RedOwl.Engine
{
    public static class TimeExtensions
    {
        public static DateTime UnixTimeStampToDateTime( double unixTimeStamp ) {
            // Unix timestamp is seconds past epoch
            DateTime sinceEpoch = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
            return sinceEpoch.AddSeconds( unixTimeStamp ).ToLocalTime();
        }
		
        public static double DateTimeToUnixTimeStamp( DateTime dateTime ) {
            DateTime sinceEpoch = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
            return (dateTime.ToUniversalTime() - sinceEpoch).TotalSeconds;
        }
		
        public static string ToNiceDateTimeString( DateTime dateTime ) {
            return dateTime.ToString(@"MM\/dd\/yyyy hh:mm tt");
        }
		
        public static string ToNiceDateTimeString( double unixTimeStamp ) {
            DateTime dateTime = UnixTimeStampToDateTime(unixTimeStamp);
            return ToNiceDateTimeString(dateTime);
        }
		
        public static string TimeSinceToString(DateTime start, DateTime end) {
            TimeSpan span = end.ToUniversalTime() - start.ToUniversalTime();
            string output = string.Format("{0:00}:{1:00}", span.Minutes, span.Seconds);
            if (span.Hours > 0){
                output = string.Format("{0:00}:", span.Hours) + output;
            }
            if (span.Days > 0){
                output = string.Format("{0:0} ", span.Days) + output;
            }
            return output;
        }
		
        public static string TimeSinceToString(double start, double end) {
            DateTime startDate = UnixTimeStampToDateTime(start);
            DateTime endDate = UnixTimeStampToDateTime(end);
            return TimeSinceToString(startDate, endDate);
        }
		
        public static string TimeSinceToString(double timespan) {
            DateTime startDate = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
            DateTime endDate = startDate.AddSeconds( timespan );
            return TimeSinceToString(startDate, endDate);
        }
    }
}