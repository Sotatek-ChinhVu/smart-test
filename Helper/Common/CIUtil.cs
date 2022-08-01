using Helper.Extendsions;
using System.Globalization;

namespace Helper.Common
{
    public static class CIUtil
    {
        public static string Copy(string input, int index, int lengthToCopy)
        {
            if (input == null) return string.Empty;

            if (index <= 0 || index > input.Length) return string.Empty;

            if (lengthToCopy <= 0) return string.Empty;

            if (index - 1 + lengthToCopy <= input.Length)
            {
                return input.Substring(index - 1, lengthToCopy);
            }
            else
            {
                return input.Substring(index - 1);
            }
        }

        // Format for param: yyyymmdd
        public static DateTime IntToDate(int iDateTime)
        {
            var result = SDateToDateTime(iDateTime);
            return result == null ? new DateTime() : result.Value;
        }
        public static DateTime? SDateToDateTime(int Ymd)
        {
            if (Ymd <= 0 || Ymd == 99999999)
            {
                return null;
            }

            try
            {
                // Padding zero first
                string s = Ymd.ToString("D8");

                // Then convert to date time
                return DateTime.ParseExact(s, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
        public static int DateTimeToInt(DateTime dateTime, string format = DateTimeFormat.yyyyMMdd)
        {
            int result;
            try
            {
                result = dateTime.ToString(format).AsInteger();
            }
            catch (Exception e)
            {
                result = 0;
            }
            return result;
        }
    }
}
