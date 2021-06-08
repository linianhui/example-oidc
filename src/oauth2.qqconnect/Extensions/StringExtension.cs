namespace OAuth2.QQConnect.Extensions
{
    public static class StringExtension
    {
        public static string EmptyIfNull(this string @this)
        {
            return @this ?? string.Empty;
        }
    }
}
