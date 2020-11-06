using System.Globalization;
using System.Text.RegularExpressions;

namespace EmberMemoryReader.Abstract.Data
{
    public static class StringPatternHelper
    {
        public static byte[] ToBytes(this string s)
        {

            byte[] buffer = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                buffer[i] = (byte)s[i];
            }
            return buffer;
        }

        public static double ToComparableVersion(this string stringVersion)
        {
            if (double.TryParse(Regex.Match(stringVersion, @"\d+(\.\d*)?").Value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var ver))
            {
                return ver;
            }

            return -1;
        }
    }
}
