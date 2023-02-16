namespace StorageStrategy.Utils.Extensions
{
    public static class StringExtensions
    {
        public static bool Compare(this string a, string b)
        {
            return a.ToLower().Trim() == b.ToLower().Trim();
        }
    }
}
