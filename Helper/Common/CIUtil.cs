using Helper.Constants;
using Helper.Extension;
using System.Globalization;
using System.Text;

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

        public static int FullStartDate(int startDate)
        {
            if (startDate == 0) return 0;

            int startDateLength = startDate.AsString().Length;
            if (startDateLength == 8)
            {
                //Format of StartDate is yyyymmdd
                return startDate;
            }
            else if (startDateLength == 6)
            {
                //Format of StartDate is yyyymm
                //Need to convert to yyyymm01
                return startDate * 100 + 1;
            }
            else
            {
                return 0;
            }
        }

        public static int FullEndDate(int endDate)
        {
            if (endDate.AsString().Count() == 8)
            {
                //Format of EndDate is yyyymmdd
                return endDate;
            }
            //Format of EndDate is yyyymm
            //Need to convert to yyyymm31
            return endDate * 100 + 31;
        }
        public static bool IsDigitsOnly(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;

            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        // 切り上げ

        public static double RoundUp(double x, int Factor)
        {
            double dFactor = IntPower(10, Factor);
            if (Factor < 0) Factor = Factor + 1;
            if (x >= 0)
                return Math.Round((double)((x * dFactor + 0.9) / dFactor), 3);
            else
                return Math.Round((double)(x * dFactor / dFactor), 3);
        }
        private static double IntPower(int baseValue, int exponent)
        {
            return Math.Pow(baseValue, exponent);
        }

        // 切捨て
        public static double RoundDown(double x, int Factor)
        {
            Double dFactor;
            Factor = Factor - 1;
            dFactor = IntPower(10, Factor);
            if (Factor < 0) Factor = Factor + 1;
            if (x >= 0)
                return Math.Round((double)(x * dFactor / dFactor), 3);
            else
                return Math.Round((double)((x * dFactor + 0.9) / dFactor), 3);
        }
        public static string ToHalfsize(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            string kanaString = RomajiString.Instance.RomajiToKana(value);
            string fullToHalf = HalfsizeString.Instance.ToHalfsize(kanaString);

            return fullToHalf;
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

        //日付チェック(西暦yyyymmdd)
        public static bool CheckSDate(string input)
        {
            return DateTime.TryParseExact(input, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeResult);
        }

        /// <summary>
        /// 四捨五入
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="digit">桁数</param>
        /// <returns></returns>
        public static int RoundInt(double value, int digit)
        {
            if (digit == 0)
            {
                return (int)Math.Round(value, MidpointRounding.AwayFromZero);
            }
            else
            {
                int wrkRate = Math.Abs(digit) * 10;
                return (int)Math.Round(value / wrkRate, MidpointRounding.AwayFromZero) * wrkRate;
            }
        }

        //西暦(yyyymmdd)から年齢を計算する
        //Calculate age from yyyymmdd format
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
                DateTime.TryParseExact(WrkStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime BirthDate);

                WrkStr = ToYmd.ToString("D8");
                DateTime.TryParseExact(WrkStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ToDate);

                Age = ToDate.Year - BirthDate.Year;

                if (BirthDate.Month > ToDate.Month)
                    Age = Age - 1;

                if ((BirthDate.Month == ToDate.Month) &&
                    (BirthDate.Day > ToDate.Day))
                    Age = Age - 1;
            }
            catch
            {
                Age = -1;
            }

            return Age;
        }

        public static bool Is70Zenki_20per(int nBirthYmd, int nSinYmd)
        {
            //《2014年05月改正 前期高齢者の負担割合(1割->2割)》
            const int Con70KaiseiDay_20per = 20140501;   //改正日
            const int Con70Birth_20per = 19440402;   //誕生日が昭和19年4月2日以降は2割負担
            return (nBirthYmd >= Con70Birth_20per) && (nSinYmd >= Con70KaiseiDay_20per);
        }

        public static bool AgeChk(int nBirthYmd, int nSinYmd, int nTgtAge)
        {
            if (nBirthYmd == 0)
            {
                return false;
            }
            DateTime dtBuf;
            int nYear = 0, nMonth = 0, nDay = 0;
            //** 誕生日が1日でない⇒翌月1日に置き換え **
            if (Copy(nBirthYmd.ToString(), 7, 2).AsInteger() != 1)
            {
                dtBuf = DateTime.ParseExact(((nBirthYmd / 100) * 100 + 1).ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                nBirthYmd = dtBuf.AddMonths(1).ToString("yyyyMMdd", CultureInfo.InvariantCulture).AsInteger();
            }
            //** 誕生日が1日⇒そのまま **

            //年齢算出
            SDateToDecodeAge(nBirthYmd, nSinYmd, ref nYear, ref nMonth, ref nDay);
            return (nYear >= nTgtAge);
        }

        public static bool IsStudent(int BirthDay, int Sinday)
        {
            int nEnterSchoolDate;
            //就学日を取得する
            if (((BirthDay % 10000) >= 402) && ((BirthDay % 10000) <= 1231))
            {
                // 4/2 ～ 12/31生まれ
                nEnterSchoolDate = (BirthDay / 10000 + 7) * 10000 + 401;
            }
            else
            {
                // 1/1 ～ 4/1生まれ
                nEnterSchoolDate = (BirthDay / 10000 + 6) * 10000 + 401;
            }

            //比較対象日と比較する
            return (Sinday >= nEnterSchoolDate);
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
                Console.WriteLine(e);
                result = 0;
            }
            return result;
        }

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
        public static string SDateToShowWDate(int ymd, WarekiFormat warekiFormat = WarekiFormat.Short)
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
                    var ex = new Exception();
                    throw ex;
            }
        }

        public static int GetByteCountFromString(string str)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            return Encoding.GetEncoding("shift_jis").GetByteCount(str);
        }

        public static int SDateToWDate(int ymd)
        {
            int ret = 0;
            string retDate = SDateToShowWDate(ymd);

            if (retDate.Length == 10)
            {
                ret = ObjectExtension.StrToIntDef
                        (_GengoId(retDate.Substring(0, 1)) +
                         retDate.Substring(2, 2) +
                         retDate.Substring(5, 2) +
                         retDate.Substring(8, 2), 0);
            }

            return ret;

            #region Local Method
            string _GengoId(string gengo)
            {
                string gengoId = "";

                switch (gengo)
                {
                    case "明":
                        gengoId = "1";
                        break;
                    case "大":
                        gengoId = "2";
                        break;
                    case "昭":
                        gengoId = "3";
                        break;
                    case "平":
                        gengoId = "4";
                        break;
                    default:
                        gengoId = "5";
                        break;
                }

                return gengoId;
            }
            #endregion
        }

        public static string FormatTimeHHmmss(string sTime)
        {
            if (string.IsNullOrWhiteSpace(sTime))
            {
                return string.Empty;
            }

            if (sTime.Length > 6)
            {
                return string.Empty;
            }

            // HHmm or Hmm
            // eg: 
            // input 2 (2minutes) => 0002 => 000200
            // input 23 (23minutes) => 0023 => 002300
            // input 935 (9h35min) => 0935 => 093500
            // input 1237 (12h37) => 1237 => 123700
            if (sTime.Length <= 4)
            {
                sTime = sTime.PadLeft(4, '0').PadRight(6, '0');
            }
            else // Hmmss
            {
                // eg:
                // input 41521 (4h15m21s) => 041521
                // input 161101(16h11m01s) => 161101
                sTime = sTime.PadLeft(6, '0');
            }
            string sHour = Copy(sTime, 1, 2);
            if (sHour.AsInteger() < 0 || sHour.AsInteger() >= 24)
            {
                return string.Empty;
            }
            string sMin = Copy(sTime, 3, 2);
            if (sMin.AsInteger() < 0 || sMin.AsInteger() >= 60)
            {
                return string.Empty;
            }
            string sSec = Copy(sTime, 5, 2);
            if (sSec.AsInteger() < 0 || sSec.AsInteger() >= 60)
            {
                return string.Empty;
            }
            return sTime;
        }


        /// <summary>
        /// 西暦を表示用和暦(ggg yy年mm月dd日)に変換
        /// Example 平成 30年07月18日
        /// </summary>
        /// <param name="ymd"></param>
        /// <returns></returns>
        public static WarekiYmd SDateToShowWDate3(int ymd)
        {
            string workString;
            int workInt;
            DateTime workDate;

            WarekiYmd warekiYmd = new WarekiYmd();
            warekiYmd.Ymd = "";
            warekiYmd.GYmd = "";
            warekiYmd.Gengo = "";
            warekiYmd.GengoId = 0;
            warekiYmd.Year = 0;
            warekiYmd.Month = 0;
            warekiYmd.Day = 0;

            // Do not convert before 1968/09/08
            if (ymd < 18680908 || ymd == 99999999)
            {
                return warekiYmd;
            }

            // Zero padding if neccessary
            workString = ymd.ToString("D8");
            if (!DateTime.TryParseExact(workString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out workDate))
            {
                // Input string is not a validated date
                return warekiYmd;
            }

            //明治
            if (ymd < 19120730)
            {
                workInt = ymd - 18670000;
                warekiYmd.Gengo = "明治";
                warekiYmd.GengoId = 1;
            }
            //大正
            else if (ymd < 19261225)
            {
                workInt = ymd - 19110000;
                warekiYmd.Gengo = "大正";
                warekiYmd.GengoId = 2;
            }
            //昭和
            else if (ymd < 19890108)
            {
                workInt = ymd - 19250000;
                warekiYmd.Gengo = "昭和";
                warekiYmd.GengoId = 3;
            }
            //平成
            else if (ymd < 20190501)
            {
                workInt = ymd - 19880000;
                warekiYmd.Gengo = "平成";
                warekiYmd.GengoId = 4;
            }
            //令和
            else
            {
                workInt = ymd - 20180000;
                warekiYmd.Gengo = "令和";
                warekiYmd.GengoId = 5;
            }

            workString = workInt.ToString("D6");

            warekiYmd.Year = workString.Substring(0, 2).AsInteger();
            warekiYmd.Month = workString.Substring(2, 2).AsInteger();
            warekiYmd.Day = workString.Substring(4, 2).AsInteger();

            warekiYmd.Ymd = string.Format("{0} {1:D2}年{2:D2}月{3:D2}日", warekiYmd.Gengo, warekiYmd.Year, warekiYmd.Month, warekiYmd.Day);
            warekiYmd.GYmd = string.Format("{0}{1:D2}{2:D2}{3:D2}", warekiYmd.GengoId, warekiYmd.Year, warekiYmd.Month, warekiYmd.Day);

            return warekiYmd;
        }
        public static bool HokenNumberCheckDigits(int hokenNumber)
        {
            int WHokenNumber = hokenNumber / 10;
            return hokenNumber == PtIDChkDgtMakeM10W21(WHokenNumber);
        }

        public static int PtIDChkDgtMakeM10W21(int PtID)
        {
            int digit = 0;
            string ptStr = PtID.ToString("D7");
            int wWait = 2;

            for (int i = 0; i <= ptStr.Length - 1; i++)
            {
                int sumOf2Digits = (ptStr[i] - '0') * wWait;
                sumOf2Digits = sumOf2Digits % 10 + sumOf2Digits / 10;
                digit = digit + sumOf2Digits;
                if (wWait == 2)
                    wWait = 1;
                else
                    wWait = 2;
            }

            digit = digit % 10;
            if (digit != 0)
                digit = 10 - digit;

            return (PtID * 10 + digit);
        }
        public static void GetHokensyaHoubetu(string hokensyaNo, ref string hokensyaNoSearch, ref string houbetuNo)
        {
            //法別番号を求める
            houbetuNo = "0";
            hokensyaNoSearch = hokensyaNo;
            switch (hokensyaNo?.Length)
            {
                case 8:
                    houbetuNo = Copy(hokensyaNo, 1, 2);
                    break;
                case 6:
                    houbetuNo = "100";
                    break;
                case 4:
                    houbetuNo = "01";
                    break;
                default:
                    break;
            }
        }

        public static ReleasedDrugType SyohoToSempatu(int syohoKbn, int syohoLimitKbn)
        {
            switch (syohoKbn)
            {
                case 0:
                    // 後発品のない先発品
                    if (syohoLimitKbn == 0) return ReleasedDrugType.None;
                    break;
                case 1:
                    // 後発品への変更不可
                    if (syohoLimitKbn == 0) return ReleasedDrugType.Unchangeable;
                    break;
                case 2:
                    // 後発品への変更可
                    if (syohoLimitKbn == 0) return ReleasedDrugType.Changeable;
                    // 剤形不可
                    if (syohoLimitKbn == 1) return ReleasedDrugType.Changeable_DoNotChangeTheDosageForm;
                    // 含量規格不可
                    if (syohoLimitKbn == 2) return ReleasedDrugType.Changeable_DoesNotChangeTheContentStandard;
                    // 含量規格・剤形不可
                    if (syohoLimitKbn == 3) return ReleasedDrugType.Changeable_DoesNotChangeTheContentStandardOrDosageForm;
                    break;
                case 3:
                    // 一般名処方への変更可
                    if (syohoLimitKbn == 0) return ReleasedDrugType.CommonName;
                    // 剤形不可
                    if (syohoLimitKbn == 1) return ReleasedDrugType.CommonName_DoNotChangeTheDosageForm;
                    // 含量規格不可
                    if (syohoLimitKbn == 2) return ReleasedDrugType.CommonName_DoesNotChangeTheContentStandard;
                    // 含量規格・剤形不可
                    if (syohoLimitKbn == 3) return ReleasedDrugType.CommonName_DoesNotChangeTheContentStandardOrDosageForm;
                    break;
            }
            return ReleasedDrugType.None;
        }
        
        public static string DateTimeToTime(DateTime dateTime)
        {
            return dateTime.ToString("HHmmss");

        }
        
        public static int DayOfWeek(DateTime dateTime)
        {
            int result = 0;
            switch (dateTime.DayOfWeek)
            {
                case System.DayOfWeek.Sunday:
                    result = 1;
                    break;
                case System.DayOfWeek.Monday:
                    result = 2;
                    break;
                case System.DayOfWeek.Tuesday:
                    result = 3;
                    break;
                case System.DayOfWeek.Wednesday:
                    result = 4;
                    break;
                case System.DayOfWeek.Thursday:
                    result = 5;
                    break;
                case System.DayOfWeek.Friday:
                    result = 6;
                    break;
                case System.DayOfWeek.Saturday:
                    result = 7;
                    break;
            }
            return result;
        }

        public static string TimeToShowTime(int timeValue)
        {
            var result = string.Empty;
            var wrkStr = string.Empty;
            if (timeValue.ToString().Length > 4)
                wrkStr = timeValue.ToString("D6");
            else
                wrkStr = timeValue.ToString("D4");
            result = Copy(wrkStr, 1, 2) + ":" + Copy(wrkStr, 3, 2);
            return result;
        }

        public enum WarekiFormat
        {
            Short,
            Full,
            Mix
        }
        
    public struct WarekiYmd
    {
#pragma warning disable S1104 // Fields should not have public accessibility
            public string Ymd;
            public string GYmd;
            public string Gengo;
            public int GengoId;
            public int Year;
            public int Month;
            public int Day;
#pragma warning restore S1104 // Fields should not have public accessibility
        }


    }
}
