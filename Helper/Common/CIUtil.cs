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

        //表示用西暦(yyyy/mm/dd)を西暦に変換
        public static int ShowSDateToSDate(string Ymd)
        {
            int Result = 0;
            // Get current year, month, day
            DateTime currentDate = DateTime.Now;

            int numberOfSeparators = Ymd.Count(c => c == '/');
            if (numberOfSeparators == 0)
            {
                // parameter does not include / character
                switch (Ymd.Length)
                {
                    case 1:
                    case 2:
                        Ymd = Ymd.AsInteger().ToString("D2");
                        Ymd = currentDate.Year + "/" + currentDate.Month.ToString("D2") + "/" + Ymd;
                        break;
                    case 3:
                    case 4:
                        Ymd = Ymd.AsInteger().ToString("D4");
                        Ymd = currentDate.Year + "/" + Ymd.Substring(0, 2) + "/" + Ymd.Substring(2);
                        break;
                    case 8:
                        Ymd = Ymd.Substring(0, 4) + "/" + Ymd.Substring(4, 2) + "/" + Ymd.Substring(6);
                        break;
                    default:
                        break;
                }
            }

            if (numberOfSeparators == 1)
            {
                string temp = DateTime.Now.Year.AsString() + "/";
                int firstLocation = Ymd.IndexOf('/');

                temp += Ymd.Substring(0, firstLocation).PadLeft(2, '0') + "/";
                temp += Ymd.Substring(firstLocation + 1).PadLeft(2, '0');

                Ymd = temp;
            }

            if (numberOfSeparators == 2)
            {
                int firstLocation = Ymd.IndexOf('/');

                string temp = Ymd.Substring(0, firstLocation) + "/";
                string remainTemp = Ymd.Substring(firstLocation + 1);

                int secondLocation = remainTemp.IndexOf('/');
                temp += remainTemp.Substring(0, secondLocation).PadLeft(2, '0') + "/";
                temp += remainTemp.Substring(secondLocation + 1).PadLeft(2, '0');

                Ymd = temp;
            }



            bool parseResult = DateTime.TryParseExact(Ymd, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtResult);

            if (!parseResult)
            {
                return 0;
            }

            Result = dtResult.ToString("yyyyMMdd").AsInteger();

            // Try to convert to wareki
            string wareki = SDateToShowWDate(Result);
            if (string.IsNullOrEmpty(wareki))
            {
                return 0;
            }

            return Result;
        }

        /// <summary>
        /// format date string (e.g:19911224) to age in format: 
        /// [a] years old [b] month(s) [c] day(s)
        /// </summary>
        /// <param name="yyyymmdd"></param>
        /// <param name="ToYyyymmdd"></param>
        /// <returns></returns>
        public static string SDateToDecodeAge(string yyyymmdd, string ToYyyymmdd)
        {
            int age = 0, month = 0, day = 0;
            CIUtil.SDateToDecodeAge(yyyymmdd.AsInteger(), ToYyyymmdd.AsInteger(), ref age, ref month,
                ref day);
            return String.Format("{0}歳{1}ヶ月{2}日", age, month, day);

        }

        //西暦(yyyymmdd)から年齢(何歳、何ヶ月、何日)を計算する
        //Calculate age (age, month, day)
        //Parameter is passed by references
        //Function return boolean value
        public static Boolean SDateToDecodeAge(
            int Ymd, int ToYmd,
            ref int Age, ref int AMonth, ref int ADay
        )
        {
            string WrkStr = string.Empty;

            WrkStr = Ymd.ToString("D8");
            if (!DateTime.TryParseExact(WrkStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime BirthDate))
            {
                return false;
            }

            WrkStr = ToYmd.ToString("D8");
            if (!DateTime.TryParseExact(WrkStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ToDate))
            {
                return false;
            }

            // Calculate Age
            Age = ToDate.Year - BirthDate.Year;

            if (BirthDate.Month > ToDate.Month)
            {
                Age = Age - 1;
            }

            if ((BirthDate.Month == ToDate.Month) &&
                (BirthDate.Day > ToDate.Day))
            {
                Age = Age - 1;
            }

            // Calculate Month
            if (BirthDate.Month < ToDate.Month)
            {
                AMonth = ToDate.Month - BirthDate.Month;
                if (BirthDate.Day > ToDate.Day)
                {
                    AMonth = AMonth - 1;
                }
            }
            else
            {
                if ((BirthDate.Month == ToDate.Month) &&
                    (BirthDate.Day <= ToDate.Day))
                {
                    AMonth = 0;
                }
                else
                {
                    AMonth = ToDate.Month - BirthDate.Month + 12;
                    if (BirthDate.Day > ToDate.Day)
                    {
                        AMonth = AMonth - 1;
                    }
                }
            }

            // Calculate Day
            if (BirthDate.Day <= ToDate.Day)
            {
                ADay = ToDate.Day - BirthDate.Day;
            }
            else
            {
                DateTime WrkDate = new DateTime(ToDate.Year, ToDate.Month, 1);
                WrkDate = WrkDate.AddDays(-1);

                ADay = WrkDate.Day - BirthDate.Day;
                if (ADay < 0)
                {
                    ADay = ToDate.Day;
                }
                else
                {
                    ADay += ToDate.Day;
                }
            }
            return true;
        }

        /// <summary>
        /// 西暦を表示用和暦(xxxx yy年mm月dd日)に変換
        /// Example 平成 30年07月18日
        /// </summary>
        /// <param name="ymd"></param>
        /// <returns></returns>
        public static string SDateToShowWDate2(int ymd)
        {
            return SDateToShowWDate(ymd, WarekiFormat.Full);

        }

        #region Convert Datetime Helpers (private)
        private static string SDateToShowWDate(int ymd, WarekiFormat warekiFormat = WarekiFormat.Short)
        {
            string workString;

            // Do not convert before 1968/09/08
            if (ymd < 18680908 || ymd == 99999999)
            {
                return string.Empty;
            }

            // Zero padding if neccessary
            workString = ymd.ToString("D8");
            if (!DateTime.TryParseExact(workString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime workDate))
            {
                // Input string is not a validated date
                return string.Empty;
            }

            string warekiName = string.Empty;
            string warekiYearFormat = string.Empty;

            //明治
            if (ymd < 19120730)
            {
                switch (warekiFormat)
                {
                    case WarekiFormat.Short:
                        warekiName = "明 ";
                        break;
                    case WarekiFormat.Full:
                        warekiName = "明治 ";
                        break;
                    default:
                        warekiName = "明 ";
                        break;
                }
                warekiYearFormat = FormatWarekiYear(ymd - 18670000, warekiFormat);
            }

            //大正
            else if (ymd < 19261225)
            {
                switch (warekiFormat)
                {
                    case WarekiFormat.Short:
                        warekiName = "大 ";
                        break;
                    case WarekiFormat.Full:
                        warekiName = "大正 ";
                        break;
                    default:
                        warekiName = "大 ";
                        break;
                }
                warekiYearFormat = FormatWarekiYear(ymd - 19110000, warekiFormat);
            }

            //昭和
            else if (ymd < 19890108)
            {
                switch (warekiFormat)
                {
                    case WarekiFormat.Short:
                        warekiName = "昭 ";
                        break;
                    case WarekiFormat.Full:
                        warekiName = "昭和 ";
                        break;
                    default:
                        warekiName = "昭 ";
                        break;
                }
                warekiYearFormat = FormatWarekiYear(ymd - 19250000, warekiFormat);
            }

            //平成
            else if (ymd < 20190501)
            {
                switch (warekiFormat)
                {
                    case WarekiFormat.Short:
                        warekiName = "平 ";
                        break;
                    case WarekiFormat.Full:
                        warekiName = "平成 ";
                        break;
                    default:
                        warekiName = "平 ";
                        break;
                }
                warekiYearFormat = FormatWarekiYear(ymd - 19880000, warekiFormat);
            }

            //令和
            else
            {
                switch (warekiFormat)
                {
                    case WarekiFormat.Short:
                        warekiName = "令 ";
                        break;
                    case WarekiFormat.Full:
                        warekiName = "令和 ";
                        break;
                    default:
                        warekiName = "令 ";
                        break;
                }
                warekiYearFormat = FormatWarekiYear(ymd - 20180000, warekiFormat);
            }

            if (!string.IsNullOrEmpty(warekiYearFormat))
            {
                return warekiName + warekiYearFormat;
            }

            return string.Empty;
        }


        //西暦を表示用西暦+和暦（yyyy(gee)/mm/dd）に変換
        // fmtReki[-1: 表示しない 0: 和暦を英字 1: 和暦を漢字]
        // fmtWeek[0: 曜日なし 1: 曜日あり]
        // fmtDate[0: / 1: 年月日]
        public static string SDateToShowSWDate(int Ymd, int fmtReki = 0, int fmtWeek = 0, int fmtDate = 0)
        {
            string workString;
            DateTime workDate;
            string formatString;

            string Result = string.Empty;
            CultureInfo jaJP = new CultureInfo("ja-JP");

            if (Ymd <= 0)
            {
                return Result;
            }
            //Padding with 8 digits
            workString = Ymd.ToString("D8");

            if (!DateTime.TryParseExact(workString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out workDate))
            {
                return string.Empty;
            }

            // Construct Format string
            string wareki = getWareki(Ymd, fmtReki);
            if (fmtDate == 0)
            {
                // use / to separate year, month , day
                formatString = "yyyy/MM/dd";

            }
            else
            {
                // use 年月日
                formatString = "yyyy年MM月dd日";
            }

            if (fmtWeek == 1)
            {
                // Display week day
                formatString += "(ddd)";
            }

            if (fmtReki == -1)
            {
                return workDate.ToString(formatString, jaJP);
            }

            Result = workDate.ToString(formatString, jaJP);

            if (!string.IsNullOrEmpty(wareki))
            {
                // Concat result to display Wareki
                Result = Result.Substring(0, 4) +
                            "(" + wareki + ")" +
                            Result.Substring(4);
            }
            return Result;

        }

        //西暦を表示用西暦(yyyy/mm/dd)に変換
        //Convert format yyyymmdd to yyyy/mm/dd
        public static string SDateToShowSDate(int Ymd)
        {
            string WrkStr;
            DateTime WrkDate;

            if (Ymd <= 0 || Ymd == 99999999)
            {
                return string.Empty;
            }

            // Padding zero first
            WrkStr = Ymd.ToString("D8");

            bool convertDateResult = DateTime.TryParseExact(WrkStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out WrkDate);

            if (!convertDateResult)
            {
                // Fail to convert date time
                return string.Empty;
            }

            // Datetime is valid, but wanna check for Wareki Validate
            int dateIntValue = WrkDate.ToString("yyyyMMdd").AsInteger();
            // Try to convert to wareki
            string wareki = SDateToShowWDate(dateIntValue);

            if (!string.IsNullOrEmpty(wareki))
            {
                // Return normal Seireki format
                return WrkDate.ToString("yyyy/MM/dd");
            }

            return string.Empty;
        }

        /// <summary>
        /// 西暦を表示用西暦和暦(yyyy(gyy)/mm/dd)に変換
        /// Example 2019(H30)/07/18
        /// </summary>
        /// <param name="ymd"></param>
        /// <returns></returns>
        public static string SDateToShowWSDate(int ymd)
        {
            string workString;
            int workInt;
            DateTime workDate;

            WarekiYmd warekiYmd = new WarekiYmd();
            warekiYmd.Ymd = "";
            warekiYmd.Gengo = "";
            warekiYmd.Year = 0;
            warekiYmd.Month = 0;
            warekiYmd.Day = 0;

            // Do not convert before 1968/09/08
            if (ymd < 18680908 || ymd == 99999999)
            {
                return "";
            }

            // Zero padding if neccessary
            workString = ymd.ToString("D8");
            if (!DateTime.TryParseExact(workString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out workDate))
            {
                // Input string is not a validated date
                return "";
            }

            //明治
            if (ymd < 19120730)
            {
                workInt = ymd - 18670000;
                warekiYmd.Gengo = "M";
            }
            //大正
            else if (ymd < 19261225)
            {
                workInt = ymd - 19110000;
                warekiYmd.Gengo = "T";
            }
            //昭和
            else if (ymd < 19890108)
            {
                workInt = ymd - 19250000;
                warekiYmd.Gengo = "S";
            }
            //平成
            else if (ymd < 20190501)
            {
                workInt = ymd - 19880000;
                warekiYmd.Gengo = "H";
            }
            //令和
            else
            {
                workInt = ymd - 20180000;
                warekiYmd.Gengo = "R";
            }

            workString = workInt.ToString("D6");

            warekiYmd.Year = workString.Substring(0, 2).AsInteger();
            warekiYmd.Month = workString.Substring(2, 2).AsInteger();
            warekiYmd.Day = workString.Substring(4, 2).AsInteger();

            return $"{ymd / 10000}({warekiYmd.Gengo}{warekiYmd.Year})/{warekiYmd.Month}/{warekiYmd.Day}";
        }

        // Get Wareki name and Wareki year from Seireki
        // // fmtReki[0: 和暦を英字 1: 和暦を漢字]
        private static string getWareki(int Ymd, int fmtReki = 0)
        {
            int WrkInt;
            string Result = "";

            if (Ymd < 18680908 || Ymd == 99999999)
            {
                return Result;
            }


            //明治
            if (Ymd < 19120730)
            {
                // Get year only
                WrkInt = (Ymd - 18670000) / 10000;
                if (fmtReki == 1)
                {
                    Result = "明" + WrkInt.ToString("D2");
                }
                else
                {
                    Result = "M" + WrkInt.ToString("D2");
                }

                return Result;
            }

            //大正
            if (Ymd < 19261225)
            {
                // Get year only
                WrkInt = (Ymd - 19110000) / 10000;
                if (fmtReki == 1)
                {
                    Result = "大" + WrkInt.ToString("D2");
                }
                else
                {
                    Result = "T" + WrkInt.ToString("D2");
                }
                return Result;
            }

            //昭和
            if (Ymd < 19890108)
            {
                // Get year only
                WrkInt = (Ymd - 19250000) / 10000;
                if (fmtReki == 1)
                {
                    Result = "昭" + WrkInt.ToString("D2");
                }
                else
                {
                    Result = "S" + WrkInt.ToString("D2");
                }
                return Result;
            }

            //平成
            if (Ymd < 20190501)
            {
                // Get year only
                WrkInt = (Ymd - 19880000) / 10000;
                if (fmtReki == 1)
                {
                    Result = "平" + WrkInt.ToString("D2");
                }
                else
                {
                    Result = "H" + WrkInt.ToString("D2");
                }

                return Result;
            }

            //令和
            // Get year only
            WrkInt = (Ymd - 20180000) / 10000;
            if (fmtReki == 1)
            {
                Result = "令" + WrkInt.ToString("D2");
            }
            else
            {
                Result = "R" + WrkInt.ToString("D2");
            }

            return Result;
        }

        /// <summary>
        /// Return yyMMdd format, where yy is wareki year
        /// For example, reiwa 05/01/01 with input string 50101
        /// will return 05/01/01.
        /// 
        /// Basically japanese wareki is based on emperor's period
        /// and no-one has period longer than 99 years.
        /// So, for the input of more than six characters, or less
        /// than five characters will returns an invalid year
        /// (string.empty).
        /// 
        /// </summary>
        /// <param name="inputString">integer format</param>
        /// <returns></returns>
        private static string FormatWarekiYear(int inputInt, WarekiFormat warekiFormat = WarekiFormat.Short)
        {
            if (inputInt <= 0) return string.Empty;

            //Zero padding first
            string inputString = inputInt.ToString("D6");
            return FormatWarekiYear(inputString, warekiFormat);
        }

        /// <summary>
        /// Return yyMMdd format, where yy is wareki year
        /// For example, reiwa 05/01/01 with input string 50101
        /// will return 05/01/01.
        /// 
        /// Basically japanese wareki is based on emperor's period
        /// and no-one has period longer than 99 years.
        /// So, for the input of more than six characters, or less
        /// than five characters will returns an invalid year
        /// (string.empty).
        /// 
        /// </summary>
        /// <param name="inputString">Six-characters string</param>
        /// <returns></returns>
        private static string FormatWarekiYear(string inputString, WarekiFormat warekiFormat = WarekiFormat.Short)
        {
            if (inputString.Length > 6 || inputString.Length < 5) return string.Empty;
            string tempString = inputString.PadLeft(6);

            switch (warekiFormat)
            {
                case WarekiFormat.Short:
                    return tempString.Substring(0, 2) + "/" + tempString.Substring(2, 2) + "/" + tempString.Substring(4, 2);
                case WarekiFormat.Full:
                    return tempString.Substring(0, 2) + "年" + tempString.Substring(2, 2) + "月" + tempString.Substring(4, 2) + "日";
                default:
                    break;
            }
            return tempString.Substring(0, 2) + "/" + tempString.Substring(2, 2) + "/" + tempString.Substring(4, 2);
        }
        #endregion

        public static string GetComputerName()
        {
            return Environment.MachineName;
        }
    }

    
    public enum WarekiFormat
    {
        Short,
        Full,
        Mix
    }

    public struct WarekiYmd
    {
        public string Ymd;
        public string GYmd;
        public string Gengo;
        public int GengoId;
        public int Year;
        public int Month;
        public int Day;
    }
}
