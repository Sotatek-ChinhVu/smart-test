using Helper.Extendsions;

namespace Helper.Common
{
    public class FormatType
    {
        public static string FormatViewLongToString(long param, long valueFormat = 0, string defaultValue = "")
        {
            if (param == valueFormat)
                return defaultValue;
            else
                return param.AsString();
        }

        public static int FormatBackStringDateTimeToInt(string param, int defaultValue = 0)
        {
            int dateIntFormat;
            try
            {
                dateIntFormat = CIUtil.ShowSDateToSDate(param);
                if (dateIntFormat == 0)
                {
                    dateIntFormat = CIUtil.ShowSDateToSDate3(param);
                }
                if (dateIntFormat == 0)
                {
                    dateIntFormat = CIUtil.ShowWDateToSDate(param);
                }
            }
            catch
            {
                dateIntFormat = defaultValue;
            }

            return dateIntFormat;
        }
    }
}
