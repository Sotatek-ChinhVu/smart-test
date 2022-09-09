using System.Globalization;
using System.Text;

namespace Helper.Extension
{
    public static class ObjectExtension
    {
        public static long AsLong(this object inputObject)
        {
            if (Int64.TryParse(inputObject.AsString(), out long result))
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
            if (Int32.TryParse(value, out int output))
            {
                return output;
            }
            else
            {
                return defaultValue;
            }
        }

        public static float StrToFloatDef(this string inputString, float defaultValue)
        {
            if (float.TryParse(inputString, out float output))
            {
                return output;
            }
            else
            {
                return defaultValue;
            }
        }

        public static int AsInteger(this object inputObject)
        {
            if (Int32.TryParse(inputObject.AsString(), out int result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static int AsIntegerFromHexString(this string inputString)
        {
            if (Int32.TryParse(inputString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static string AsString(this Object inputObject, string defaultValue = "")
        {
            if (inputObject == null || inputObject == DBNull.Value) return defaultValue;
            return inputObject.ToString() ?? string.Empty;
        }

        public static float AsFloat(this Object inputObject)
        {
            float result;
            if (float.TryParse(inputObject.AsString(), out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static double AsDouble(this Object inputObject)
        {
            double result;
            if (double.TryParse(inputObject.AsString(), out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static decimal AsDecimal(this Object inputObject)
        {
            decimal result;
            if (decimal.TryParse(inputObject.AsString(), out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static int AsInt(this Boolean value)
        {
            if (value)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static bool AsBool(this object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is int || value is Int16 || value is Int32 || value is Int64)
            {
                return (int)value == 1;
            }
            else if (value is double || value is Double)
            {
                return (double)value == 1;
            }
            else if (value is float)
            {
                return (float)value == 1;
            }
            else if (value is string || value is String)
            {
                return string.Equals(value.AsString().ToLower(), "true");
            }
            else if (value is bool || value is Boolean)
            {
                return (bool)value;
            }
            else
            {
                throw new NotSupportedException("Not supported for type: " + value.GetType().AsString());
            }
        }

        public static bool AsBool(this object value, bool defaulValue)
        {
            bool result = defaulValue;
            try
            {
                result = value.AsBool();
            }
            catch
            {
                result = defaulValue;
            }
            return result;
        }

        public static String IntToStr(int inputInt)
        {
            return inputInt.ToString();
        }

        public static String IntToStr(int? inputInt)
        {
            if (inputInt == null)
            {
                return "null";
            }
            return inputInt.ToString() ?? string.Empty;
        }

        public static String FloatToStr(float? inputFloat)
        {
            if (inputFloat == null)
            {
                return "null";
            }
            return inputFloat.ToString() ?? string.Empty;
        }

        public static void Add(this StringBuilder inputString, string addString)
        {
            if (!addString.StartsWith(" "))
            {
                addString = " " + addString;
            }
            inputString.Append(addString);
        }

        public static bool Assigned(this object AObject)
        {
            if (AObject != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static System.Drawing.Color AsColor(this string hexColor)
        {
            try
            {
                if (string.IsNullOrEmpty(hexColor))
                {
                    return System.Drawing.Color.Transparent;
                }

                if (!hexColor.StartsWith("#"))
                {
                    hexColor = "#" + hexColor;
                }
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(hexColor);
                return color;
            }
            catch
            {
                return System.Drawing.Color.Transparent;
            }
        }
        public static string AsString(this System.Drawing.Color color)
        {
            try
            {
                return color.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static double ToSize(this Int64 value, SizeUnits unit)
        {
            return (value / Math.Pow(1024, (Int64)unit));
        }
    }

    public enum SizeUnits
    {
        Byte, KB, MB, GB, TB, PB, EB, ZB, YB
    }
}
