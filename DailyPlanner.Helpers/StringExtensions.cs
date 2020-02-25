

namespace DailyPlanner.Helpers
{
    public static class StringExtensions
    {
        public static string ToReadableToken(this string str)
        {
            return str.Substring("Bearer ".Length).Trim();
        }
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            return str[0].ToString().ToLower() + str.Substring(1, str.Length - 1);
        }
    }

}