namespace Mokus2D.Default.Namespace
{
    public class Strings
    {
        public static bool IsBlank(string stringP)
        {
            return string.IsNullOrEmpty(stringP);
        }

        public static bool ContainsSought(string source, string sought)
        {
            return source.Contains(sought);
        }

        public static string FromInt(int value)
        {
            return value.ToString();
        }

        public static bool StartsWithSought(string source, string sought)
        {
            return source.StartsWith(sought);
        }
    }
}
