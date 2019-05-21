

namespace DailyPlanner.Helpers
{
    public static class StringExtensions
    {
        public static string ToReadableToken(this string str)
        {
            return str.Substring("Bearer ".Length).Trim();
        }
    }
}