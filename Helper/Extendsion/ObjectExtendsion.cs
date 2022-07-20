namespace Helper.Extendsions
{
    public static class ObjectExtendsion
    {
        public static string AsString(this String inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return "";
            return inputString;
        }

        public static string AsString(this Object inputObject, string defaultValue = "")
        {
            var result = inputObject?.ToString();
            if (inputObject == null || inputObject == DBNull.Value || result == null) return defaultValue;
            return result;
        }

        public static int AsInteger(this object inputObject)
        {
            int result;
            if (Int32.TryParse(inputObject.AsString(), out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        public static long AsLong(this object inputObject)
        {
            long result;
            if (Int64.TryParse(inputObject.AsString(), out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static int StrToIntDef(String value, int defaultValue = 0)
        {
            int output;
            if (Int32.TryParse(value, out output))
            {
                return output;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
