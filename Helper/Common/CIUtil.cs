using Helper.Extendsions;
using System.Globalization;

namespace Helper.Common
{
    public static class CIUtil
    {
        //Calculate age from yyyymmdd format
        private const int HEISEI_START_YEAR = 1989;

        private const int SHOWA_START_YEAR = 1926;
        private const int TAISHO_START_YEAR = 1912;
        private const int MEIJI_START_YEAR = 1868;
        private const int REIWA_START_YEAR = 2019;

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
        private static string SDateToShowWDate(int ymd, WarekiFormat warekiFormat)
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

        public static int DateTimeToInt(DateTime dateTime, string format = DateTimeFormat.yyyyMMdd)
        {
            int result = 0;
            try
            {
                result = dateTime.ToString(format).AsInteger();
            }
            catch
            {
                return result;
            }
            return result;
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

        //西暦を表示用和暦(x yy/mm/dd)に変換
        //Convert format yyyyymmdd to Japanese style x yy/mm/dd
        public static string SDateToShowWDate(int ymd)
        {
            return SDateToShowWDate(ymd, WarekiFormat.Short);
        }

        /// <summary>
        /// yy/MM/dd to yyyyMMdd
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ShowSDateToSDate3(string input)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(input, "yy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return DateTimeToInt(dateTime);
            }
            return 0;
        }

        /// <summary>
        /// This is the main and very important converter function
        /// Please be careful when edit/fix bugs
        /// 
        /// Test cases must be added and every path of the code
        /// must be covered
        /// </summary>
        /// <param name="ymd"></param>
        /// <returns></returns>
        //表示用和暦(x yy/mm/dd)を西暦に変換
        public static int ShowWDateToSDate(string ymd)
        {
            int result = 0;
            char wGengo;
            string wYmd = ymd.Trim();
            DateTime currentDate = DateTime.Now;

            // One character : Current date of current month
            // 一文字は当月のその日
            if (wYmd.Length < 2)
            {
                if (wYmd != "")
                {
                    wYmd = currentDate.Year + ""
                           + currentDate.Month.ToString("D2")
                           + wYmd.AsInteger().ToString("D2");
                    if (DateTime.TryParseExact(wYmd, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtResult))
                    {
                        return dtResult.ToString("yyyyMMdd").AsInteger();
                    }
                }
                return result;
            }

            int delimiterCount = 0;
            if (wYmd.IndexOf('.') >= 0 || wYmd.IndexOf('/') >= 0)
            {
                // 区切りが「.」のとき「/」に変換
                // Replace [.] character with [/]
                string tempWmd = wYmd.Replace(".", "/");
                foreach (char c in tempWmd)
                {
                    if (c == '/') delimiterCount++;
                }
            }

            bool canExtractGengoPart = false;
            if (delimiterCount == 3) canExtractGengoPart = true;
            if (delimiterCount == 1)
            {
                string tempWmd = wYmd.Substring(2);
                if (tempWmd.Length == 6)
                {
                    // allow patter 3.500101 = Showa(3), 50th year, January 1st
                    canExtractGengoPart = true;
                }
            }

            // Character the represented Japanese year is included
            // 元号付きバージョン
            if ((wYmd[0] == 'M') || (wYmd[0] == 'T') || (wYmd[0] == 'S') || (wYmd[0] == 'H') || (wYmd[0] == 'R') ||
                (wYmd[0] == 'm') || (wYmd[0] == 't') || (wYmd[0] == 's') || (wYmd[0] == 'h') || (wYmd[0] == 'r') ||
                (wYmd[0] == '明') || (wYmd[0] == '大') || (wYmd[0] == '昭') || (wYmd[0] == '平') || (wYmd[0] == '令') ||
                (wYmd[1] == ' ') ||
                (wYmd[1] == '.') && canExtractGengoPart)
            {
                switch (wYmd[0])
                {
                    case '1':
                    case 'm':
                    case 'M':
                    case '明':
                        wGengo = 'M';
                        break;

                    case '2':
                    case 't':
                    case 'T':
                    case '大':
                        wGengo = 'T';
                        break;

                    case '3':
                    case 's':
                    case 'S':
                    case '昭':
                        wGengo = 'S';
                        break;

                    case '4':
                    case 'h':
                    case 'H':
                    case '平':
                        wGengo = 'H';
                        break;

                    case '5':
                    case 'r':
                    case 'R':
                    case '令':
                        wGengo = 'R';
                        break;

                    default:
                        wGengo = '@';
                        break;
                }

                if ((wYmd[1] == ' ') || (wYmd[1] == '.'))
                {
                    // Substring from the 3rd character
                    // To do exception could be rised here
                    wYmd = wYmd.Substring(2);
                }
                else
                {
                    // Substring from the 2nd character
                    // To do exception could be rised here
                    wYmd = wYmd.Substring(1);
                }
            }
            else
            {
                wGengo = ' ';
            }

            // 区切りが「.」のとき「/」に変換
            // Replace [.] character with [/]
            wYmd = wYmd.Replace(".", "/");
            // Must count again here
            delimiterCount = 0;
            foreach (char c in wYmd)
            {
                if (c == '/') delimiterCount++;
            }

            // Delimter character exists
            if (delimiterCount > 0)
            {
                // wYmd can have multiple form here
                // m/d or mm/dd
                // yy/mm/dd or yy/m/d or yyyy/mm/dd
                string sTemp = string.Empty;
                if (delimiterCount == 2)
                {
                    // Character [/] exists
                    // Get year part
                    sTemp = wYmd.Substring(0, wYmd.IndexOf('/'));

                    // Year is equal to 0, then error
                    if (sTemp == "0" || sTemp == "00")
                    {
                        wYmd = "";
                    }
                    else
                    {
                        try
                        {
                            int sYear;
                            if (sTemp.Length <= 2)
                            {
                                int WYear = sTemp.AsInteger();
                                sYear = WYearToSYear(WYear, wGengo);
                            }
                            else
                            {
                                sYear = sTemp.AsInteger();
                            }

                            sTemp = wYmd.Substring(wYmd.IndexOf('/') + 1);
                            //Extract month and day part
                            string sMonth = sTemp.Substring(0, sTemp.IndexOf('/'));
                            string sDay = sTemp.Substring(sTemp.IndexOf('/') + 1);
                            //Zero padding
                            if (sMonth.Length < 2)
                            {
                                sMonth = "0" + sMonth;
                            }
                            if (sDay.Length < 2)
                            {
                                sDay = "0" + sDay;
                            }

                            wYmd = sYear + sMonth + sDay;
                        }
                        catch
                        {
                            wYmd = "";
                        }
                    }
                }

                if (delimiterCount == 1)
                {
                    int sYear = DateTime.Now.Year;
                    // m/d or mm/dd
                    //Extract month and day part
                    string sMonth = wYmd.Substring(0, wYmd.IndexOf('/'));
                    string sDay = wYmd.Substring(wYmd.IndexOf('/') + 1);
                    //Zero padding
                    if (sMonth.Length < 2)
                    {
                        sMonth = "0" + sMonth;
                    }
                    if (sDay.Length < 2)
                    {
                        sDay = "0" + sDay;
                    }

                    wYmd = sYear + sMonth + sDay;
                }
            }
            else
            {
                // Length = 7 or Length > 9 is error
                if (wYmd.Length == 7 || wYmd.Length >= 9)
                {
                    wYmd = "";
                }
                else if (wYmd.Length == 8)
                {
                    // Length = 8
                    // Do nothing, current format should be good enough to convert
                }
                else if (wYmd.Length == 6 || wYmd.Length == 5)
                {
                    // Length = 6 or Length = 5
                    // Zero padding first
                    if (wYmd.Length == 5)
                        wYmd = "0" + wYmd;

                    if (wYmd[0] == '0' && wYmd[1] == '0')
                    {
                        wYmd = "";
                    }
                    else
                    {
                        try
                        {
                            int WYear = wYmd.Substring(0, 2).AsInteger();
                            int SYear = WYearToSYear(WYear, wGengo);
                            wYmd = SYear.ToString() + wYmd.Substring(2, 4);
                        }
                        catch
                        {
                            wYmd = "";
                        }
                    }
                }
                else
                {
                    if (wGengo == ' ')
                    {
                        if (wYmd.Length == 4 || wYmd.Length == 3)
                        {
                            // Zero padding first
                            if (wYmd.Length == 3)
                                wYmd = "0" + wYmd;
                            // Length = 4
                            wYmd = currentDate.Year.ToString() +
                                    wYmd.Substring(0, 2) +
                                    wYmd.Substring(2);
                        }
                        else
                        {
                            // Length = 2
                            wYmd = currentDate.Year.ToString() +
                                    currentDate.Month.ToString("D2") +
                                    wYmd;
                        }
                    }
                    else
                    {
                        wYmd = "";
                    }
                }
            }

            if (DateTime.TryParseExact(wYmd, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parseResult))
            {
                result = parseResult.ToString("yyyyMMdd").AsInteger();
                // Do not convert before 1968/09/08
                if (result < 18680908 || result == 99999999)
                {
                    result = 0;
                }
                //Try to convert to wareki
                string wareki = SDateToShowWDate(result);
                if (string.IsNullOrEmpty(wareki))
                {
                    result = 0;
                }
            }
            else
            {
                result = 0;
            }

            return result;
        }

        private static int WYearToSYear(int WYear, char Gengo)
        {
            switch (Gengo)
            {
                case ' ':
                case 'R':
                case 'r':
                    return WYear + (REIWA_START_YEAR - 1);

                case 'H':
                case 'h':
                    return WYear + (HEISEI_START_YEAR - 1);

                case 'S':
                case 's':
                    return WYear + (SHOWA_START_YEAR - 1);

                case 'T':
                case 't':
                    return WYear + (TAISHO_START_YEAR - 1);

                case 'M':
                case 'm':
                    return WYear + (MEIJI_START_YEAR - 1);

                default:
                    Exception exception = new Exception();
                    throw exception;
            }
        }

        public static int SDateToAge(int Ymd, int ToYmd)
        {
            if (Ymd <= 0 || ToYmd <= 0)
            {
                return -1;
            }
            string WrkStr;
            int Age;

            try
            {
                WrkStr = Ymd.ToString("D8");
                DateTime.TryParseExact(WrkStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthDate);

                WrkStr = ToYmd.ToString("D8");
                DateTime.TryParseExact(WrkStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate);

                Age = toDate.Year - birthDate.Year;

                if (birthDate.Month > toDate.Month)
                    Age = Age - 1;

                if ((birthDate.Month == toDate.Month) &&
                    (birthDate.Day > toDate.Day))
                    Age = Age - 1;
            }
            catch
            {
                Age = -1;
            }

            return Age;
        }

        public static int GetSanteInfDayCount(int sinDate, int lastOdrDate, int alertTerm)
        {
            int targetDateInt = lastOdrDate;
            if (targetDateInt == 0)
            {
                return 0;
            }

            int dayCount = 0;
            // Current Date
            DateTime nowDate = IntToDate(sinDate);
            int nowDateInt = sinDate;
            // Target Date
            DateTime targetDate = IntToDate(targetDateInt);
            // Calculate base on AlertTerm
            switch (alertTerm)
            {
                case 2:
                    if (targetDateInt < nowDateInt)
                        dayCount = DaysBetween(targetDate, nowDate) + 1;
                    else
                        dayCount = DaysBetween(nowDate, targetDate) + 1;
                    break;
                case 3:
                    //曜日を取得
                    DayOfWeek day = targetDate.DayOfWeek;
                    if (day != System.DayOfWeek.Sunday)
                    {
                        //日曜始まりの週でカウント
                        int incDay = (int)day * -1;
                        targetDate = targetDate.AddDays(incDay);
                        _ = DateTimeToInt(targetDate);
                    }
                    dayCount = WeeksBetween(targetDate, nowDate) + 1;

                    break;
                case 4:
                    //月初から月単位でカウント
                    int startDateint = targetDate.Year * 10000 + targetDate.Month * 100 + 1;
                    DateTime startDate = IntToDate(startDateint);
                    dayCount = MonthsBetween(startDate, nowDate) + 1;
                    break;
                case 5:
                    dayCount = WeeksBetween(targetDate, nowDate) + 1;
                    break;
                case 6:
                    dayCount = MonthsBetween(targetDate, nowDate) + 1;
                    break;
            }
            if (dayCount < 0)
            {
                dayCount *= (-1);
            }
            return dayCount;
        }

        public static string TimeToShowTime(int TimeValue)
        {
            string Result = string.Empty;
            string WrkStr = string.Empty;
            if (TimeValue.ToString().Length > 4)
                WrkStr = TimeValue.ToString("D6");

            else
                WrkStr = TimeValue.ToString("D4");
            Result = Copy(WrkStr, 1, 2) + ":" + Copy(WrkStr, 3, 2);
            return Result;
        }

        public static int DaysBetween(DateTime from, DateTime to)
        {
            return (int)(to - @from).TotalDays;
        }

        public static int WeeksBetween(DateTime fromDate, DateTime endDate)
        {
            return DaysBetween(fromDate, endDate) / 7;
        }

        public static int MonthsBetween(DateTime startDate, DateTime now)
        {
            int monthDiff = ((now.Year - startDate.Year) * 12) + (now.Month - startDate.Month);

            if (monthDiff > 0 && startDate.Day > now.Day)
            {
                monthDiff--;
            }
            else if (monthDiff < 0 && startDate.Day < now.Day)
            {
                monthDiff++;
            }
            return monthDiff;
        }

        //西暦を表示用西暦(yyyy/mm/dd (ddd))に変換
        //Convert format yyyymmdd to yyyy/mm/dd(//Convert format yyyymmdd to yyyy/mm/ddd)
        public static string SDateToShowSDate2(int Ymd)
        {
            if (Ymd <= 0)
            {
                return string.Empty;
            }

            // Must use ja-JP culture info here
            // We need format date time as 2019/12/08(月)
            // instead of 2019/12/08(Mon)
            // Omit this culture info will cause wrong date/time format
            CultureInfo jaJP = new CultureInfo("ja-JP");

            // Zero padding
            string WrkStr = Ymd.ToString("D8");

            if (DateTime.TryParseExact(WrkStr, "yyyyMMdd", jaJP, DateTimeStyles.None, out DateTime WrkDate))
            {
                return WrkDate.ToString("yyyy/MM/dd(ddd)", jaJP);
            }

            return string.Empty;
        }

        public static string HourAndMinuteFormat(string value)
        {
            string sResult = "";
            if (!string.IsNullOrEmpty(value) && value.AsInteger() != 0)
            {
                if (value.Length > 4)
                {
                    sResult = CIUtil.Copy(value, 1, 4);
                }
                else
                {
                    sResult = value;
                }
                sResult = sResult.PadLeft(4, '0');
                sResult = sResult.Insert(2, ":");
            }
            return sResult;
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
        public string Ymd { get; set; }
        public string GYmd { get; set; }
        public string Gengo { get; set; }
        public int GengoId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
