using System.Collections.Generic;

namespace ROP
{
    public static class IEnumerableUtils
    {
        /// <summary>
        /// Shorthand for string.Join(separator, strings)
        /// </summary>
        public static string JoinStrings(this IEnumerable<string> strings, string separator)
        {
            return string.Join(separator, strings);
        }

    }
}
