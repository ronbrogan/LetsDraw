using System.Text;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static string ReduceWhitespace(this string value)
        {
            var newString = new StringBuilder();
            var previousIsWhitespace = false;
            foreach (var c in value)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (previousIsWhitespace)
                    {
                        continue;
                    }

                    previousIsWhitespace = true;
                }
                else
                {
                    previousIsWhitespace = false;
                }

                newString.Append(c);
            }

            return newString.ToString();
        }
    }
}
