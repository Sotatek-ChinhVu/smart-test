using Helper.Extension;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Helper.Common
{
    public static class CIUtil
    {
        //Define japanese era start year
        private const int HEISEI_START_YEAR = 1989;
        private const int SHOWA_START_YEAR = 1926;
        private const int TAISHO_START_YEAR = 1912;
        private const int MEIJI_START_YEAR = 1868;
        private const int REIWA_START_YEAR = 2019;

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

        // 切捨て
        public static double RoundDown(double x, int Factor)
        {
            double dFactor;
            Factor = Factor - 1;
            dFactor = IntPower(10, Factor);
            if (x >= 0)
                return Math.Round((x * dFactor / dFactor), 3);
            else
                return Math.Round(((x * dFactor + 0.9) / dFactor), 3);
        }

        // 切り上げ
        public static double RoundUp(double x, int Factor)
        {
            double dFactor = IntPower(10, Factor);
            if (x >= 0)
                return Math.Round(((x * dFactor + 0.9) / dFactor), 3);
            else
                return Math.Round((x * dFactor / dFactor), 3);
        }

        private static double IntPower(int baseValue, int exponent)
        {
            return Math.Pow(baseValue, exponent);
        }

        // time: hhmm
        public static int AddMinute(int time, int minute)
        {
            int timeMinute = time % 100;
            int tempHour = time / 100 + (timeMinute + minute) / 60;
            int tempMinute = (timeMinute + minute) % 60;
            return tempHour * 100 + tempMinute;
        }

        // time2 - time1
        public static int MinusMinute(int time1, int time2)
        {
            return (time2 / 100) * 60 + time2 % 100 - (time1 / 100) * 60 - time1 % 100;
        }

        //------------------------------------------------------------------------------
        //  説明  ：  CIS日付をTDate型に変換
        //  引数  ：  ACIDate:         CIS日付
        //            ADate：          変換日付[out]
        //  戻り値：  True...変換成功 / False...変換できない値
        //------------------------------------------------------------------------------
        public static bool TryCIDateToDate(int ACIDate, ref DateTime ADate)
        {
            int iYear = ACIDate / 10000;
            int iMonth = ((ACIDate - iYear * 10000) / 100);
            int iDay = ACIDate - (iYear * 10000) - (iMonth * 100);
            return TryEncodeDate(iYear, iMonth, iDay, ref ADate);
        }

        public static bool TryEncodeDate(int iYear, int iMonth, int iDay, ref DateTime ADate)
        {
            string sYear = iYear.ToString();
            if (9999 < iYear || iYear < 1)
            {
                ADate = new DateTime();
                return false;
            }
            else
            {
                for (int i = 0; i < 4 - iYear.ToString().Length; i++)
                {
                    sYear = "0" + sYear;
                }
            }
            string sMonth = iMonth.ToString();
            if (1 <= iMonth && iMonth <= 12)
            {
                if (iMonth < 10)
                {
                    sMonth = "0" + sMonth;
                }
            }
            else
            {
                ADate = new DateTime();
                return false;
            }
            string sDay = iDay.ToString();
            if (1 <= iDay && iDay <= 31)
            {
                if (iDay < 10)
                {
                    sDay = "0" + sDay;
                }
            }
            else
            {
                ADate = new DateTime();
                return false;
            }
            try
            {
                ADate = DateTime.ParseExact(sYear + sMonth + sDay, "yyyyMMdd", null);
                return true;
            }
            catch
            {
                ADate = new DateTime();
                return false;
            }
        }

        public static bool IsImage(string pathFile)
        {
            string extension = Path.GetExtension(pathFile).AsString().ToLower();
            string[] images = { ".jpg", ".jpeg", ".jfif", ".png", ".gif", ".bmp", ".ico", ".tif", ".tiff", ".wmf" };

            return images.Contains(extension);
        }

        public static bool IsExcelDocument(string pathFile)
        {
            string extension = Path.GetExtension(pathFile).AsString().ToLower();
            string[] docs = { ".xls", ".xlt", ".xlsx", ".xltx", ".xls", ".xlt", ".xlsx", ".xltx" };

            return docs.Contains(extension);
        }

        public static bool IsWordDocument(string pathFile)
        {
            string extension = Path.GetExtension(pathFile).AsString().ToLower();
            string[] excels = { ".doc", ".dot", ".docx", ".dotx", ".doc", ".dot", ".docx", ".dotx" };

            return excels.Contains(extension);
        }

        public static bool IsCanPreviewFile(string pathFile)
        {
            bool result = IsImage(pathFile) || IsExcelDocument(pathFile) || IsWordDocument(pathFile);
            if (result)
            {
                return result;
            }

            string extension = Path.GetExtension(pathFile).AsString().ToLower();
            string[] listPreview = { ".txt", ".ini", ".pdf" };

            return listPreview.Contains(extension);
        }

        public static void DecodeDate(DateTime ATime, ref int iYear, ref int iMonth, ref int iDay)
        {
            iYear = ATime.Year;
            iMonth = ATime.Month;
            iDay = ATime.Day;
        }

        public static bool IsLeapYear(int iYeah)
        {
            return (0 < iYeah && iYeah < 9999);
        }

        public static DateTime EncodeDate(int iYear, int iMonth, int iDay)
        {
            DateTime result = new DateTime();
            TryEncodeDate(iYear, iMonth, iDay, ref result);
            return result;
        }

        //四捨五入関数（小数点以下第何位で四捨五入するか）
        public static double RoundoffNum(double X, int Num)
        {
            Double dRet;
            bool bDiv;

            double result = X;
            if (Num == 0)
            {
                return result;
            }

            if (Num > 0)
            {
                bDiv = true;
                Num = Num - 1;
                X = X * (Math.Pow(10, Num));
                //X = StrToFloat(FloatToStr(X)); //誤差を生まないために一度文字列にする
            }
            else
            {
                bDiv = false;
                Num = Num * (-1);
                X = X / (Math.Pow(10, Num));
                //X = StrToFloat(FloatToStr(X)); //誤差を生まないために一度文字列にする
            }

            if (X >= 0)
            {
                dRet = Math.Truncate(X + 0.5);
            }
            else
            {
                dRet = Math.Truncate(X - 0.5);
            }

            if (bDiv)
            {
                result = dRet / (Math.Pow(10, Num));
            }
            else
            {
                result = dRet * (Math.Pow(10, Num));
            }
            return result;
        }

        public static int GetGroupKoui(int kouiCode)
        {
            int result = kouiCode / 10 * 10;

            if (11 <= kouiCode && kouiCode <= 13)
            {
                // NuiTran recommend handle this case
                result = 11;
            }
            else if (kouiCode == 14)
            {
                // 在宅
                result = kouiCode;
            }
            else if (kouiCode >= 68 && kouiCode < 70)
            {
                // 画像
                result = kouiCode;
            }
            else if (kouiCode >= 95 && kouiCode < 99)
            {
                // コメント以外
                result = kouiCode;
            }
            else if (kouiCode == 100 || kouiCode == 101)
            {
                // コメント（処方箋）
                // コメント（処方箋備考）
                result = 20;
            }

            return result;
        }

        public static bool MecsIsFullWidth(char cValue)
        {
            var sjis = System.Text.Encoding.GetEncoding("shift_JIS");
            int byteCount = sjis.GetByteCount(cValue.ToString());
            return byteCount == 2;
        }

        public static int MecsStringWidth(string text)
        {
            int width = 0;
            if (!string.IsNullOrEmpty(text))
            {
                foreach (char c in text)
                {
                    if (MecsIsFullWidth(c))
                    {
                        width += 2;
                    }
                    else
                    {
                        width++;
                    }
                }
            }
            return width;
        }

        /// <summary>
        /// Return Delphi index
        /// If not found or invalid return 0
        /// </summary>
        /// <param name="subStr"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int Pos(string subStr, string s)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(subStr)) return 0;
            if (subStr.Length > s.Length) return 0;
            return s.IndexOf(subStr, StringComparison.Ordinal) + 1;
        }

        /// <summary>
        /// Pos関数拡張
        /// ※文字数（半角文字が1文字、全角文字が2文字）で計算
        /// </summary>
        /// <returns>delphi index</returns>
        public static int PosStrWidth(string subStr, string str)
        {
            int result = 0;
            int iBuff = Pos(subStr, str);
            if (iBuff > 0)
            {
                result = MecsStringWidth(Copy(str, 1, iBuff));
            }
            return result;
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

        /// <summary>
        /// format date string (e.g:19911224) to age in format: 
        /// [a] years old [b] month(s)
        /// </summary>
        /// <param name="yyyymmdd"></param>
        /// <param name="ToYyyymmdd"></param>
        /// <returns></returns>
        public static string SDateToDecodeAgeYM(int yyyymmdd, int ToYyyymmdd)
        {
            int age = 0, month = 0, day = 0;
            CIUtil.SDateToDecodeAge(yyyymmdd, ToYyyymmdd, ref age, ref month,
                ref day);
            return $"{(object)age}歳{(object)month}ヶ月";
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
            return string.Format("{0}歳{1}ヶ月{2}日", age, month, day);

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
            string WrkStr = Ymd.ToString("D8");
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
                DateTime BirthDate;
                DateTime ToDate;
                DateTime.TryParseExact(WrkStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate);

                WrkStr = ToYmd.ToString("D8");
                DateTime.TryParseExact(WrkStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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

        public static string GenerateFolderPtId(long ptID)
        {
            var iDown = ptID % 100;
            var iUp = (ptID % 10000 - iDown) / 100;
            string folderDown = iDown.ToString("00");
            string fFolderUp = iUp.ToString("00");

            return fFolderUp + @"\" + folderDown;
        }

        public static string GenerateFileNameToDay(long ptID, string extension)
        {
            string fileName = ptID.AsString().PadLeft(10, '0') + "_" +
                   DateTime.Now.ToString("yyyyMMddHHmmssfff");

            if (!string.IsNullOrEmpty(extension))
            {
                fileName += "." + extension.Replace(".", "");
            }

            return fileName;
        }

        //西暦を表示用和暦(x yy/mm/dd)に変換
        //Convert format yyyyymmdd to Japanese style x yy/mm/dd
        public static string SDateToShowWDate(int ymd)
        {
            return SDateToShowWDate(ymd, WarekiFormat.Short);
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
        #endregion

        /// <summary>
        /// 西暦を和暦数値（元号ID+YYMMDD）に変換
        /// </summary>
        /// <param name="ymd">西暦</param>
        /// <returns></returns>
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

        /// <summary>
        /// 西暦を和暦数値（元号ID+YYMMDD）に変換（労災レセ用）
        /// </summary>
        /// <param name="ymd">西暦</param>
        /// <returns></returns>
        public static int SDateToWDateForRousai(int ymd)
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
                        gengoId = "3";
                        break;
                    case "昭":
                        gengoId = "5";
                        break;
                    case "平":
                        gengoId = "7";
                        break;
                    default:
                        gengoId = "9";
                        break;
                }

                return gengoId;
            }
            #endregion
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
            if (wYmd.IndexOf('.') > 0 || wYmd.IndexOf('/') > 0)
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
                    // Todo exception could be rised here
                    wYmd = wYmd.Substring(2);
                }
                else
                {
                    // Substring from the 2nd character
                    // Todo exception could be rised here
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

        /// <summary>
        /// 和暦年号 + yy + mm + dd(計7桁)を西暦に変換する
        /// 4300101 → 平成30年1月1日 → 20180101
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int ReceWDateToSDate(int date)
        {
            int ret = date;

            if (date.ToString().Length == 7)
            {
                string sDate =
                    $"{date.ToString().Substring(0, 1)}." +
                    $"{date.ToString().Substring(1, 2)}." +
                    $"{date.ToString().Substring(3, 2)}." +
                    $"{date.ToString().Substring(5, 2)}";

                ret = ShowWDateToSDate(sDate);
            }
            return ret;
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
                    throw new Exception();
            }
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

        //西暦を表示用西暦(yy/mm/dd)に変換
        //Convert format yyyymmdd to yy/mm/dd
        public static string SDateToShowSDate3(int Ymd)
        {
            string WrkStr;
            DateTime WrkDate;

            if (Ymd <= 0 || Ymd == 99999999)
            {
                return "";
            }

            try
            {
                // Padding zero first
                WrkStr = Ymd.ToString("D8");

                // Then convert to date time
                WrkDate = DateTime.ParseExact(WrkStr, "yyyyMMdd", CultureInfo.InvariantCulture);
                return WrkDate.ToString("yy/MM/dd");
            }
            catch
            {
                return "";
            }
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

        public static int DateTimeToInt(DateTime dateTime, string format = "yyyyMMdd")
        {
            int result = 0;
            try
            {
                result = dateTime.ToString(format).AsInteger();
            }
            catch (Exception e)
            {
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>yyyy/MM/dd HH:mm</returns>
        public static string GetCIDateTimeStr(DateTime dateTime, bool isGetSec = false)
        {
            string result = string.Empty;
            string format = isGetSec ? "yyyy/MM/dd HH:mm:ss" : "yyyy/MM/dd HH:mm";
            try
            {
                result = dateTime.ToString(format);
            }
            catch (Exception e)
            {
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="format"></param>
        /// <returns> yyyymmddHHmmssfff by fomart </returns>
        public static string DateTimeToStr(DateTime dateTime, string format = "yyyy/MM/dd HH:mm:ss,fff")
        {
            string result = string.Empty;
            try
            {
                result = dateTime.ToString(format);
                if (!string.IsNullOrEmpty(result))
                {
                    result = new string(result.Where(c => char.IsDigit(c)).ToArray());
                }
            }
            catch (Exception e)
            {
            }
            return result;
        }

        //convert yyyyMMddHHmmss to yyyy/MM/dd HH:mm:ss
        public static DateTime StrDateToDate(string sDate, string format = "yyyyMMddHHmmss")
        {
            DateTime dateTimeResult = DateTime.Now;
            try
            {
                format = "yyyyMMddHHmmss";
                DateTime.TryParseExact(sDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeResult);
            }
            catch (Exception e)
            {
            }
            return dateTimeResult;
        }

        public static DateTime StrToDate(string dateTimeStr)
        {
            DateTime result = new DateTime();
            if (string.IsNullOrEmpty(dateTimeStr)) return result;
            try
            {
                result = DateTime.ParseExact(dateTimeStr, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
            }
            return result;
        }

        /// <summary>
        /// yyyy/MM/dd HH:mm or yyyy/MM/dd HH:mm:ss
        /// </summary>
        /// <param name="dateTimeStr"></param>
        /// <returns></returns>
        public static DateTime StrToDateTime(string dateTimeStr)
        {
            DateTime result = new DateTime();
            if (string.IsNullOrEmpty(dateTimeStr)) return result;
            try
            {
                result = DateTime.Parse(dateTimeStr);
            }
            catch (Exception e)
            {
            }
            return result;
        }

        public static DateTime StrToDateFomat(string dateTimeStr, string strFomat)
        {
            DateTime result = new DateTime();
            if (string.IsNullOrEmpty(dateTimeStr)) return result;
            try
            {
                result = DateTime.ParseExact(dateTimeStr, strFomat, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
            }
            return result;
        }


        // Format for param: yyyymmdd
        public static DateTime IntToDate(int iDateTime)
        {
            var result = SDateToDateTime(iDateTime);
            return result == null ? new DateTime() : result.Value;
        }

        public static DateTime IntToDateTime(int iDate, int iTime)
        {
            string sDate = iDate.ToString();
            string sTime = iTime.ToString().PadLeft(6, '0');
            DateTime dTime = new DateTime();

            if (!string.IsNullOrEmpty(sDate) && sDate.Length == 8)
            {

                string strTime = $"{sDate.Substring(0, 4)}/{sDate.Substring(4, 2)}/{sDate.Substring(6, 2)} {sTime.Substring(0, 2)}:{sTime.Substring(2, 2)}:{sTime.Substring(4, 2)}";
                string format = "yyyy/MM/dd HH:mm:ss";

                try
                {
                    dTime = DateTime.ParseExact(strTime, format, null);
                }
                catch
                {
                    dTime = new DateTime();
                }
            }
            return dTime;
        }
        /// <summary>
        /// HHmm
        /// </summary>
        /// <param name="iTime"></param>
        /// <returns></returns>
        public static int IntToTimeInt(int iTime)
        {
            if (iTime < 0 || iTime > 2400)
            {
                return -1;
            }

            if (iTime < 100)
            {
                return (iTime / 60) * 100 + iTime % 60;
            }

            var iHour = iTime / 100;
            var iMinute = iTime % 100;
            if (iMinute >= 60)
            {
                iMinute = iMinute % 60;
                iHour++;

                if (iHour >= 24 && iMinute > 0)
                {
                    iHour = 0;
                }
            }

            return iHour * 100 + iMinute;
        }

        public static string IntToTimeStr(int iTime)
        {
            if (iTime < 0 || iTime > 2400)
            {
                return string.Empty;
            }

            var time = IntToTimeInt(iTime);
            var str = (time / 100).AsString().PadLeft(2, '0') + ":" +
                      (time % 100).AsString().PadLeft(2, '0');

            return str;
        }

        // HH:mm => HHmm
        public static int StrTimeToInt(string sTime)
        {
            return sTime.Replace(":", "").AsInteger();
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

        public static bool AgeChk(int nBirthYmd, int nSinYmd, int nTgtAge)
        {
            if (nBirthYmd == 0)
            {
                return false;
            }
            DateTime dtBuf;
            int nYear = 0, nMonth = 0, nDay = 0;
            {
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
        }

        public static bool OverlapPeriod(int startDate1, int endDate1, int startDate2, int endDate2)
        {
            return (startDate1 <= startDate2 && endDate1 >= startDate2) ||
                   (startDate1 >= startDate2 && startDate1 <= endDate2);
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

        public static String GetComputerName()
        {
            return Environment.MachineName;
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

        /// <summary>
        /// Return 0 if parse failed, return yyyyMMdd as integer if input string is valid
        /// </summary>
        /// <param name="Ymd">Input date</param>
        /// <returns></returns>
        public static int ParseInputDateToSDate(string Ymd)
        {
            int result = ShowWDateToSDate(Ymd);
            if (result == 0)
            {
                result = ShowSDateToSDate(Ymd);
            }

            return result;
        }

        //----------------------------------------------------------------------------//
        // 機能      ： ＪＩＳコードチェック
        //              ※電子レセプトの文字規格（以下が使用可能）
        //　　　　　　　　JISX0201-1976：(JISローマ字,JISカナ（半角カナ）
        //                JISX0208-1983: (01区 ～ 08区 各種記号,英数字,かな）
        //                               (16区 ～ 47区 JIS第１水準漢字)
        //                               (48区 ～ 84区 JIS第２水準漢字)
        //                上記以外はエラー文字とする。
        //
        // 引数      ： sIn   : チェックする文字列
        //              sOut  : エラー文字以外の文字列
        // 戻り値　　： エラー文字列
        // 備考　　　： http://charset.7jp.net/jis0208.html
        //          ： http://d.hatena.ne.jp/tekk/20120623/1340462114
        //----------------------------------------------------------------------------//
        public static string Chk_JISKj(string sIn, out string sOut)
        {
            string strErr = "";
            sOut = "";

            // タブ、改行コードは除去しておく
            // Trim Tab & New-line character
            sIn = sIn.Replace("\t", "");
            sIn = sIn.Replace(Environment.NewLine, "");

            TextElementEnumerator textEnum = null;
            textEnum = StringInfo.GetTextElementEnumerator(sIn);

            while (textEnum.MoveNext())
            {
                // サロゲートペア文字の存在チェック
                if (textEnum.GetTextElement().Length > 1)
                {
                    strErr = strErr + CIUtil.Copy(sIn, textEnum.ElementIndex + 1, textEnum.GetTextElement().Length);

                }

                //文字列にUnicodeでしか使用されていない文字の存在チェック
                //.NET string is all UTF-16 string, therefore, no need to check
                // whether string contains both ASCII and UTF characters

                //end else if IsUnicodeOnly(sIn[1]) = true then begin
                //strErrStr:= strErrStr + Copy(sIn, 1, 1);
                //Delete(sIn, 1, 1);

                else
                {
                    //チェック対象文字をunicode　→ Ansiに変換
                    string strChkString = CIUtil.Copy(sIn, textEnum.ElementIndex + 1, textEnum.GetTextElement().Length);
                    byte[] asciiBytes = UTF16CharToBytes(strChkString[0]);

                    if (asciiBytes.Length == 1)
                    {
                        // 1バイトコード
                        // 半角: 記号，英数字
                        // 半角カタカナは全角として返しますので、ここでのチェックが不要
                        if ((asciiBytes[0] >= 0x20) && (asciiBytes[0] <= 0x7E))
                        {
                            if (asciiBytes[0] == 0x3F && strChkString != @"?")
                            {
                                // 0x3F = 63 = ? character
                                // Check string is not "?"
                                // Then can not convert to ISO 2022 JP = 第3水準，第4水準漢字等
                                strErr += strChkString;

                            }
                            else
                            {
                                sOut += strChkString;
                            }
                        }
                        {
                            //制御文字
                            //見えないので、追加は必要ない
                        }

                    }
                    else if (asciiBytes.Length == 8)
                    {
                        //Convert from asciiBytes to 区点 in JIS

                        int ku = asciiBytes[3] - 32;
                        int ten = asciiBytes[4] - 32;
                        //13区（環境依存文字）はエラーとする
                        if (ku == 13)
                        {
                            strErr += strChkString;
                        }
                        else
                        {
                            //JISX0208 - 1983: (01区 ～ 08区 各種記号, 英数字, かな）
                            //                           (16区 ～ 47区 JIS第１水準漢字)
                            //                           (48区 ～ 84区 JIS第２水準漢字)
                            if ((1 <= ku && ku <= 8) ||
                                (16 <= ku && ku <= 84))
                            {
                                //全角ひらがな、かたかな、第1水準、第2水準漢字
                                sOut += strChkString;
                            }
                            else
                            {
                                strErr += strChkString;
                            }
                        }
                    }
                    else
                    {
                        //外字??
                    }
                }

            }

            return strErr;
        }
        public static byte[] UTF16CharToBytes(char utf16Char)
        {
            byte[] asciiBytes = new byte[1];

            try
            {
                //ISO 2022 Japanese JIS X 0201-1989; Japanese (JIS-Allow 1 byte Kana - SO/SI)
                Encoding ascii = Encoding.GetEncoding("iso-2022-jp");
                asciiBytes = ascii.GetBytes(utf16Char.ToString());
            }
            catch
            {
                asciiBytes = new byte[1];
            }

            return asciiBytes;
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

        public static bool PtNumCheckDigits(long ptNum)
        {
            long ptNumber = ptNum / 10;
            return ptNum == PtIDChkDgtMakeM10W31(ptNumber);
        }

        public static long PtIDChkDgtMakeM10W31(long ptNum)
        {
            int digit = 0;
            string ptStr = ptNum.ToString("D10");
            int weight = 1;

            for (int i = 0; i <= ptStr.Length - 1; i++)
            {
                digit += (ptStr[i] - '0') * weight;
                weight = (weight == 1) ? 3 : 1;
            }

            digit %= 10;
            if (digit != 0)
                digit = 10 - digit;

            return ptNum * 10 + digit;
        }

        // yyyymmをyyyy/mmに変換
        public static String SMonthToShowSMonth(int tgtYm)
        {
            string rs = "";
            string sTgtYm = tgtYm.ToString();
            sTgtYm = sTgtYm.Replace("/", "");
            if (sTgtYm.Length != 6) return "";
            int iBuf = 0;
            bool isInt = int.TryParse(sTgtYm, out iBuf);
            if (isInt)
            {
                if (iBuf.ToString().Length != 6)
                    return "";
                string sBuf = SDateToShowSDate(iBuf * 100 + 1);
                if (sBuf == "")
                    return "";
                var dtBuf = DateTime.Parse(sBuf);
                return dtBuf.ToString("yyyy/MM");
            }
            return rs;
        }

        // yyyy/mmをyyyymmに変換
        public static int ShowSMonthToSMonth(string Ym)
        {
            int Result = 0;
            int DelimiterCount = 0;
            string sTemp;
            string WYm = Ym.Trim();
            DateTime CurrentDate = DateTime.Now;

            // Input month only
            int iYm = WYm.AsInteger();
            if (iYm <= 12 && iYm >= 1)
            {
                WYm = CurrentDate.Year + WYm.AsInteger().ToString("D2");
                Result = DateTime.ParseExact(WYm, "yyyyMM", CultureInfo.InvariantCulture)
                    .ToString("yyyyMM").AsInteger();

                return Result;
            }

            if (WYm.IndexOf('.') > 0 || WYm.IndexOf('/') > 0)
            {
                DelimiterCount = 1;
            }

            // Delimter character exists
            if (DelimiterCount > 0)
            {
                // 区切りが「.」のとき「/」に変換
                // Replace [.] character with [/]
                WYm = WYm.Replace(".", "/");

                if (WYm.IndexOf('/') > 0)
                {
                    // Character [/] exists
                    // Get year part
                    sTemp = WYm.Substring(0, WYm.IndexOf('/'));
                    // Year is equal to 0, then error
                    if (sTemp == "0" || sTemp == "00")
                    {
                        WYm = "";
                    }
                    else
                    {
                        int SYear = sTemp.AsInteger();
                        string SMonth = WYm.Substring(WYm.IndexOf('/') + 1);
                        //Zero padding
                        SMonth = SMonth.PadLeft(2, '0');
                        WYm = SYear + SMonth;
                    }
                }
            }
            else
            {
                if (WYm.Length == 4)
                {
                    int WYear = WYm.Substring(0, 2).AsInteger();
                    int SYear = WYearToSYear(WYear, ' ');
                    WYm = SYear.ToString() + WYm.Substring(2, 2);
                }
                // Delimiter character does not exists
                // Length != 6 is error
                if (WYm.Length != 6)
                {
                    WYm = "";
                }
            }

            try
            {
                Result = DateTime.ParseExact(WYm, "yyyyMM", CultureInfo.InvariantCulture)
                    .ToString("yyyyMM").AsInteger();
                return Result;
            }
            catch
            {
                Result = 0;
            }
            return Result;
        }

        //日付チェック(西暦yyyymmdd)
        public static bool CheckSDate(string input)
        {
            return DateTime.TryParseExact(input, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeResult);
        }

        public static bool CheckSTime(string input)
        {
            bool result = false;

            if (input.Length == 4)
            {
                result = (StrToIntDef(input.Substring(0, 2), 25) < 24) &&
                         (StrToIntDef(input.Substring(2, 2), 60) < 60);
            }
            else if (input.Length == 6)
            {
                result = (StrToIntDef(input.Substring(0, 2), 25) < 24) &&
                         (StrToIntDef(input.Substring(2, 2), 60) < 60) &&
                         (StrToIntDef(input.Substring(4, 2), 60) < 60);
            }

            return result;
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

        //西暦を表示用西暦+和暦（yyyy(gee)/mm）に変換
        // fmtReki[-1: 表示しない 0: 和暦を英字 1: 和暦を漢字]
        // fmtWeek[0: 曜日なし 1: 曜日あり]
        // fmtDate[0: / 1: 年月日]
        public static string SMonthToShowSWMonth(int Ym, int fmtReki = 0, int fmtWeek = 0, int fmtDate = 0)
        {
            string result = SDateToShowSWDate(Ym * 100 + 1, fmtReki, fmtWeek, fmtDate);
            if (result == string.Empty) return string.Empty;
            return result.Substring(0, result.Length - 3);
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
        /// Get Era from date
        ///    "明治","大正","昭和","平成","令和"
        /// </summary>
        /// <param name="Ymd"> Date</param>
        /// <returns></returns>
        public static string GetEraFromDate(int Ymd)
        {
            if (Ymd < 18680908 || Ymd == 99999999)
            {
                return string.Empty;
            }

            //明治
            if (Ymd < 19120730)
            {
                return "明治";
            }

            //大正
            if (Ymd < 19261225)
            {
                return "大正";
            }

            //昭和
            if (Ymd < 19890108)
            {
                return "昭和";
            }

            //平成
            if (Ymd < 20190501)
            {
                return "平成";
            }

            //令和
            return "令和";
        }

        /// <summary>
        /// Get Era Reki from data
        /// </summary>
        /// <param name="Ymd">Date</param>
        /// <param name="fmtReki"> Reki 1: 略K #1:略 </param>
        /// <returns></returns>
        public static string GetEraRekiFromDate(int Ymd, int fmtReki = 0)
        {
            string Result = "";

            if (Ymd < 18680908 || Ymd == 99999999)
            {
                return Result;
            }


            //明治
            if (Ymd < 19120730)
            {
                if (fmtReki == 1)
                {
                    Result = "明";
                }
                else
                {
                    Result = "M";
                }

                return Result;
            }

            //大正
            if (Ymd < 19261225)
            {
                if (fmtReki == 1)
                {
                    Result = "大";
                }
                else
                {
                    Result = "T";
                }
                return Result;
            }

            //昭和
            if (Ymd < 19890108)
            {
                if (fmtReki == 1)
                {
                    Result = "昭";
                }
                else
                {
                    Result = "S";
                }

                return Result;
            }

            //平成
            if (Ymd < 20190501)
            {
                if (fmtReki == 1)
                {
                    Result = "平";
                }
                else
                {
                    Result = "H";
                }

                return Result;
            }

            //令和
            if (fmtReki == 1)
            {
                Result = "令";
            }
            else
            {
                Result = "R";
            }

            return Result;
        }

        public static string DateTimeToTime(DateTime dateTime)
        {
            if (dateTime == null) return string.Empty;
            return dateTime.ToString("HHmmss");
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

        public static bool Is70Zenki_20per(int nBirthYmd, int nSinYmd)
        {
            //《2014年05月改正 前期高齢者の負担割合(1割->2割)》
            const int Con70KaiseiDay_20per = 20140501;   //改正日
            const int Con70Birth_20per = 19440402;   //誕生日が昭和19年4月2日以降は2割負担
            return (nBirthYmd >= Con70Birth_20per) && (nSinYmd >= Con70KaiseiDay_20per);
        }

        /// <summary>
        /// コメント項目を文字列展開する
        /// </summary>
        /// <param name="itemCd"></param>
        /// <param name="cmtCol">位置（1文字目を1とした位置）※C#の文字列は0始まりで処理するので注意！</param>
        /// <param name="cmtLen">長さ</param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <param name="comment">コメント文</param>
        /// <param name="maskEdit"></param>
        /// <returns>展開したコメントを返す</returns>
        //public static string GetCommentStr(string itemCd, List<int> cmtCol, List<int> cmtLen, string oldName, string newName, ref string comment, bool maskEdit = false)
        //{
        //    string ret = oldName;

        //    string _getReceName(string itemCd)
        //    {

        //    }

        //    // 半角→全角で、全角に統一する
        //    comment = ToWide(comment);

        //    switch (itemCd.Substring(0, 3))
        //    {
        //        case "810":
        //            ret = comment;
        //            break;
        //        case "820":
        //            ret = newName;
        //            break;
        //        case "830":
        //            ret = newName + ToWide(comment);
        //            break;
        //        case "831":
        //            ret = newName + _getReceName(comment);
        //            break;
        //        case "840":
        //            if (itemCd == ItemCdConst.GazoDensibaitaiHozon)
        //            {
        //                newName = "電子媒体保存撮影　　　回";
        //            }

        //            int maxLen = 0;

        //            for (int i = 0; i < cmtLen.Count; i++)
        //            {
        //                if (cmtLen[i] == 0) break;
        //                maxLen += cmtLen[i];
        //            }

        //            if (maxLen > 0)
        //            {
        //                if (maskEdit == true && maxLen > comment.Length)
        //                {
        //                    comment = comment + new string('＊', maxLen - comment.Length);
        //                }

        //                int num; // TryParse dummy
        //                if (comment.Length < maxLen && int.TryParse(ToNarrow(comment), out num))
        //                {
        //                    comment = new string('０', maxLen - comment.Length) + comment;
        //                }

        //                if (maxLen == comment.Length)
        //                {
        //                    ret = newName;
        //                    int start = 0;

        //                    for (int i = 0; i < cmtCol.Count; i++)
        //                    {
        //                        if (cmtCol[i] <= 0) break;

        //                        ret = ret.Remove(cmtCol[i] - 1, cmtLen[i]).Insert(cmtCol[i] - 1, comment.Substring(start, cmtLen[i]));
        //                        start += cmtLen[i];
        //                    }
        //                }
        //            }

        //            break;
        //    }

        //    return ret;

        //}

        /// <summary>
        /// 全角文字に変換
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dakuten">全角の濁点結合を行うかどうか</param>
        /// <returns></returns>
        public static string ToWide(string s, bool dakuten = true)
        {
            string ret = "";
            string dummy1 = "";
            string dummy2 = "";
            string val = "";

            Dictionary<string, string> replaceChar;

            if (dakuten == false)
            {
                replaceChar
                   = new Dictionary<string, string>()
                {
                    {"ｶﾞ", "ガ"}, {"ｷﾞ", "ギ"}, {"ｸﾞ", "グ"}, {"ｹﾞ", "ゲ"}, {"ｺﾞ", "ゴ"},
                    {"ｻﾞ", "ザ"}, {"ｼﾞ", "ジ"}, {"ｽﾞ", "ズ"}, {"ｾﾞ", "ゼ"}, {"ｿﾞ", "ゾ"},
                    {"ﾀﾞ", "ダ"}, {"ﾁﾞ", "ヂ"}, {"ﾂﾞ", "ヅ"}, {"ﾃﾞ", "デ"}, {"ﾄﾞ", "ド"},
                    {"ﾊﾞ", "バ"}, {"ﾋﾞ", "ビ"}, {"ﾌﾞ", "ブ"}, {"ﾍﾞ", "ベ"}, {"ﾎﾞ", "ボ"},
                    {"ﾊﾟ", "パ"}, {"ﾋﾟ", "ピ"}, {"ﾌﾟ", "プ"}, {"ﾍﾟ", "ペ"}, {"ﾎﾟ", "ポ"},
                    {"ｳﾞ", "ヴ"},

                };
            }
            else
            {
                replaceChar
                    = new Dictionary<string, string>()
                {
                    {"ｶﾞ", "ガ"}, {"ｷﾞ", "ギ"}, {"ｸﾞ", "グ"}, {"ｹﾞ", "ゲ"}, {"ｺﾞ", "ゴ"},
                    {"ｻﾞ", "ザ"}, {"ｼﾞ", "ジ"}, {"ｽﾞ", "ズ"}, {"ｾﾞ", "ゼ"}, {"ｿﾞ", "ゾ"},
                    {"ﾀﾞ", "ダ"}, {"ﾁﾞ", "ヂ"}, {"ﾂﾞ", "ヅ"}, {"ﾃﾞ", "デ"}, {"ﾄﾞ", "ド"},
                    {"ﾊﾞ", "バ"}, {"ﾋﾞ", "ビ"}, {"ﾌﾞ", "ブ"}, {"ﾍﾞ", "ベ"}, {"ﾎﾞ", "ボ"},
                    {"ﾊﾟ", "パ"}, {"ﾋﾟ", "ピ"}, {"ﾌﾟ", "プ"}, {"ﾍﾟ", "ペ"}, {"ﾎﾟ", "ポ"},
                    {"ｳﾞ", "ヴ"},
                    {"ｶ゛", "ガ"}, {"ｷ゛", "ギ"}, {"ｸ゛", "グ"}, {"ｹ゛", "ゲ"}, {"ｺ゛", "ゴ"},
                    {"ｻ゛", "ザ"}, {"ｼ゛", "ジ"}, {"ｽ゛", "ズ"}, {"ｾ゛", "ゼ"}, {"ｿ゛", "ゾ"},
                    {"ﾀ゛", "ダ"}, {"ﾁ゛", "ヂ"}, {"ﾂ゛", "ヅ"}, {"ﾃ゛", "デ"}, {"ﾄ゛", "ド"},
                    {"ﾊ゛", "バ"}, {"ﾋ゛", "ビ"}, {"ﾌ゛", "ブ"}, {"ﾍ゛", "ベ"}, {"ﾎ゛", "ボ"},
                    {"ﾊ゜", "パ"}, {"ﾋ゜", "ピ"}, {"ﾌ゜", "プ"}, {"ﾍ゜", "ペ"}, {"ﾎ゜", "ポ"},
                    {"ｳ゛", "ヴ"},
                    {"カ゛", "ガ"}, {"キ゛", "ギ"}, {"ク゛", "グ"}, {"ケ゛", "ゲ"}, {"コ゛", "ゴ"},
                    {"サ゛", "ザ"}, {"シ゛", "ジ"}, {"ス゛", "ズ"}, {"セ゛", "ゼ"}, {"ソ゛", "ゾ"},
                    {"タ゛", "ダ"}, {"チ゛", "ヂ"}, {"ツ゛", "ヅ"}, {"テ゛", "デ"}, {"ト゛", "ド"},
                    {"ハ゛", "バ"}, {"ヒ゛", "ビ"}, {"フ゛", "ブ"}, {"ヘ゛", "ベ"}, {"ホ゛", "ボ"},
                    {"ハ゜", "パ"}, {"ヒ゜", "ピ"}, {"フ゜", "プ"}, {"ヘ゜", "ペ"}, {"ホ゜", "ポ"},
                    {"ウ゛", "ヴ"},
                    {"カﾞ", "ガ"}, {"キﾞ", "ギ"}, {"クﾞ", "グ"}, {"ケﾞ", "ゲ"}, {"コﾞ", "ゴ"},
                    {"サﾞ", "ザ"}, {"シﾞ", "ジ"}, {"スﾞ", "ズ"}, {"セﾞ", "ゼ"}, {"ソﾞ", "ゾ"},
                    {"タﾞ", "ダ"}, {"チﾞ", "ヂ"}, {"ツﾞ", "ヅ"}, {"テﾞ", "デ"}, {"トﾞ", "ド"},
                    {"ハﾞ", "バ"}, {"ヒﾞ", "ビ"}, {"フﾞ", "ブ"}, {"ヘﾞ", "ベ"}, {"ホﾞ", "ボ"},
                    {"ハﾟ", "パ"}, {"ヒﾟ", "ピ"}, {"フﾟ", "プ"}, {"ヘﾟ", "ペ"}, {"ホﾟ", "ポ"},
                    {"ウﾞ", "ヴ"},
               };
            }

            CultureInfo jaJP = new CultureInfo("ja-JP");
            int i = 0;
            if (s != null)
            {
                while (i < s.Length)
                {
                    if (i < s.Length - 1 && replaceChar.TryGetValue(s.Substring(i, 2), out val))
                    {
                        ret += val;
                        i = i + 2;
                        continue;
                    }
                    else if (IsUntilJISKanjiLevel2InKana(s.Substring(i, 1), ref dummy1, ref dummy2))
                    {
                        ret += Microsoft.VisualBasic.Strings.StrConv(s.Substring(i, 1), Microsoft.VisualBasic.VbStrConv.Wide, jaJP.LCID);
                    }
                    else
                    {
                        ret += s.Substring(i, 1);
                    }
                    i++;
                }
            }
            return ret;
        }

        /// <summary>
        /// ひらがなの濁点を結合
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceHiraDakuten(string s)
        {
            string ret = "";
            string val = "";

            Dictionary<string, string> replaceChar
                   = new Dictionary<string, string>()
                {
                    {"か゛", "が"}, {"き゛", "ぎ"}, {"く゛", "ぐ"}, {"け゛", "げ"}, {"こ゛", "ご"},
                    {"さ゛", "ざ"}, {"し゛", "じ"}, {"す゛", "ず"}, {"せ゛", "ぜ"}, {"そ゛", "ぞ"},
                    {"た゛", "だ"}, {"ち゛", "ぢ"}, {"つ゛", "づ"}, {"て゛", "で"}, {"と゛", "ど"},
                    {"は゛", "ば"}, {"ひ゛", "び"}, {"ふ゛", "ぶ"}, {"へ゛", "べ"}, {"ほ゛", "ぼ"},
                    {"は゜", "ぱ"}, {"ひ゜", "ぴ"}, {"ふ゜", "ぷ"}, {"へ゜", "ぺ"}, {"ほ゜", "ぽ"},
                    {"う゛", "ヴ"},
                };

            int i = 0;
            if (s != null)
            {
                while (i < s.Length)
                {
                    if (i < s.Length - 1 && replaceChar.TryGetValue(s.Substring(i, 2), out val))
                    {
                        ret += val;
                        i = i + 2;
                        continue;
                    }
                    else
                    {
                        ret += s.Substring(i, 1);
                    }
                    i++;
                }
            }

            return ret;
        }
        /// <summary>
        /// 半角文字に変換
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToNarrow(string s)
        {
            return Microsoft.VisualBasic.Strings.StrConv(s, Microsoft.VisualBasic.VbStrConv.Narrow);
        }

        /// <summary>
        /// 半角カタカナの小文字を大文字に変換する
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string KanaUpper(string s)
        {
            string ret = s;

            ret = ret.Replace("ｧ", "ｱ");
            ret = ret.Replace("ｨ", "ｲ");
            ret = ret.Replace("ｩ", "ｳ");
            ret = ret.Replace("ｪ", "ｴ");
            ret = ret.Replace("ｫ", "ｵ");
            ret = ret.Replace("ｬ", "ﾔ");
            ret = ret.Replace("ｭ", "ﾕ");
            ret = ret.Replace("ｮ", "ﾖ");
            ret = ret.Replace("ｯ", "ﾂ");

            return ret;
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
                    //if (houbetuNo == "67" || houbetuNo == "27")
                    //{
                    //    houbetuNo = "100";
                    //    hokensyaNoSearch = CIUtil.Copy(hokensyaNo, 3, hokensyaNo.Length - 2);
                    //}
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

        public static string ToRecedenString(string value)
        {
            string ret = value;

            List<(string oldstr, string newstr)> replaceStrings = new List<(string, string)>()
            {
                {("¹", "１")}, {("²", "２")}, {("³", "３")}, {("⁴", "４")}, {("⁵", "５")}, {("⁶", "６")}, {("⁷", "７")}, {("⁸", "８")}, {("⁹", "９")}, {("⁰", "０")},
                {("₁", "１")}, {("₂", "２")}, {("₃", "３")}, {("₄", "４")}, {("₅", "５")}, {("₆", "６")}, {("₇", "７")}, {("₈", "８")}, {("₉", "９")}, {("₀", "０")},
                {("〜","～")}
            };

            foreach ((string oldstr, string newstr) in replaceStrings)
            {
                ret = ret.Replace(oldstr, newstr);
            }

            return ToWide(ret);
        }

        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        public static long DirSize(string dirPath)
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);
            return DirSize(di);
        }

        public static string GetTextFromGrpItemCd(int GrpItemCd)
        {
            switch (GrpItemCd)
            {
                case 0:
                    return "検査結果速報";
                case 1:
                    return "身体情報";
                case 2:
                    return "アレルギー";
                case 3:
                    return "病態";
                case 4:
                    return "相互作用";
                case 5:
                    return "生活歴";
                case 6:
                    return "患者コメント";
                case 7:
                    return "経過日数";
                case 8:
                    return "出産予定";
                case 9:
                    return "来院情報";
                default:
                    return string.Empty;
            }
        }

        ///<summary>
        ///指定の日数前の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">日数</param>
        ///<returns>基準日の指定日数前の日付</returns>
        public static int DaysBefore(int baseDate, int term)
        {
            DateTime? dt;
            DateTime dt1;
            int retDate = baseDate;

            dt = SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                dt1 = dt1.AddDays((term - 1) * -1);
                retDate = DateTimeToInt(dt1);
            }

            return retDate;
        }

        ///<summary>
        ///指定の週数前の日曜日の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">週数</param>
        ///<returns>基準日の指定週数前の週の日曜日の日付</returns>
        public static int WeeksBefore(int baseDate, int term)
        {
            DateTime? dt;
            DateTime dt1;
            int retDate = baseDate;

            dt = SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                dt1 = dt1.AddDays((int)dt1.DayOfWeek * -1 + (-7 * (term - 1)));
                retDate = DateTimeToInt(dt1);
            }

            return retDate;
        }

        /// <summary>
        /// 指定日が属する週の日曜日の日付を取得する
        /// </summary>
        /// <param name="baseDate">基準日</param>
        /// <returns>指定日が属する週の日曜日の日付</returns>
        public static int GetFirstDateOfWeek(int baseDate)
        {
            DateTime? dt;
            DateTime dt1;
            int retDate = baseDate;

            dt = SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                dt1 = dt1.AddDays((int)dt1.DayOfWeek * -1);
                retDate = DateTimeToInt(dt1);
            }

            return retDate;
        }

        /// <summary>
        /// 指定日が属する週の土曜日の日付を取得する
        /// </summary>
        /// <param name="baseDate">基準日</param>
        /// <returns>指定日が属する週の土曜日の日付</returns>
        public static int GetLastDateOfWeek(int baseDate)
        {
            DateTime? dt;
            DateTime dt1;
            int retDate = baseDate;

            dt = SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                dt1 = dt1.AddDays(6 - (int)dt1.DayOfWeek);
                retDate = DateTimeToInt(dt1);
            }

            return retDate;
        }

        /// <summary>
        /// 指定日の曜日を番号で取得する
        /// </summary>
        /// <param name="baseDate"></param>
        /// <returns>
        /// 0:日曜～6:土曜
        /// </returns>
        public static int GetWeek(int baseDate)
        {
            DateTime? dt;
            DateTime dt1;
            int ret = 0;

            dt = SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                ret = (int)dt1.DayOfWeek;
            }

            return ret;
        }
        /// <summary>
        /// 指定日の曜日を取得する
        /// </summary>
        /// <param name="baseDate"></param>
        /// <returns>
        /// 日～土
        /// </returns>
        public static string GetYobi(int baseDate)
        {
            string week = "";
            switch (GetWeek(baseDate))
            {
                case 0:
                    week = "日";
                    break;
                case 1:
                    week = "月";
                    break;
                case 2:
                    week = "火";
                    break;
                case 3:
                    week = "水";
                    break;
                case 4:
                    week = "木";
                    break;
                case 5:
                    week = "金";
                    break;
                case 6:
                    week = "土";
                    break;
            }
            return week;
        }

        /// <summary>
        /// 指定日が属する月の最終日を取得する
        /// </summary>
        /// <param name="baseDate">基準日</param>
        /// <returns></returns>
        public static int GetLastDateOfMonth(int baseDate)
        {
            DateTime? dt;
            DateTime dt1;
            int retDate = baseDate;

            dt = SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                retDate = baseDate / 100 * 100 + DateTime.DaysInMonth(dt1.Year, dt1.Month);
            }

            return retDate;
        }

        ///<summary>
        ///指定の月数前の初日の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">月数</param>
        ///<returns>基準日の指定月数前の月の初日の日付</returns>
        public static int MonthsBefore(int baseDate, int term)
        {
            DateTime? dt;
            DateTime dt1;
            int retDate = baseDate;

            dt = SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                dt1 = dt1.AddMonths(term * -1);
                retDate = DateTimeToInt(dt1);
                retDate = retDate / 100 * 100 + 1;
            }
            return retDate;
        }

        ///<summary>
        ///指定の月数後の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">月数</param>
        ///<returns>基準日の指定月数後の日付</returns>
        public static int MonthsAfter(int baseDate, int term)
        {
            DateTime? dt;
            DateTime dt1;
            int retDate = baseDate;

            dt = SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                dt1 = dt1.AddMonths(term);
                retDate = DateTimeToInt(dt1);
            }
            return retDate;
        }

        ///<summary>
        ///指定の年数前の月の初日の日付を取得する
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">年数</param>
        ///<returns>基準日の指定年数前の月の初日の日付</returns>
        public static int YearsBefore(int baseDate, int term)
        {
            DateTime? dt;
            DateTime dt1;
            int retDate = baseDate;

            dt = SDateToDateTime(baseDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                dt1 = dt1.AddYears(term * -1);
                retDate = DateTimeToInt(dt1);
                retDate = retDate / 100 * 100 + 1;
            }
            return retDate;
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
            //else if (digit > 0)
            //{
            //    int wrkRate = Math.Abs(digit) * 10;
            //    return Math.Round(value * wrkRate, MidpointRounding.AwayFromZero) / wrkRate;
            //}
            else
            {
                int wrkRate = Math.Abs(digit) * 10;
                return (int)Math.Round(value / wrkRate, MidpointRounding.AwayFromZero) * wrkRate;
            }
        }

        /// <summary>
        /// 0の場合は空文字、>0の場合は数値を文字列に変換した値を返す
        /// </summary>
        /// <param name="val">変換したい数値</param>
        /// <param name="format">
        ///     ToStringの引数に渡す書式
        ///     ※3桁カンマ区切りの場合、"#,0"を渡す
        /// </param>
        /// <returns>0, nullの場合は空文字、>0の場合は数値を文字列に変換した値を返す</returns>
        public static string ToStringIgnoreZero(int val, string format = "")
        {
            string ret = "";

            if (val > 0)
            {
                ret = val.ToString(format);
            }

            return ret;
        }
        public static string ToStringIgnoreZero(int? val)
        {
            string ret = "";

            if (val == null)
            {
                ret = "";
            }
            else if (val > 0)
            {
                ret = val.ToString();
            }

            return ret;
        }
        public static string ToStringIgnoreZero(long val, string format = "")
        {
            string ret = "";

            if (val > 0)
            {
                ret = val.ToString(format);
            }

            return ret;
        }
        public static string ToStringIgnoreZero(double val, string format = "")
        {
            string ret = "";

            if (val != 0)
            {
                ret = val.ToString(format);
            }

            return ret;
        }

        /// <summary>
        /// nullの場合は空文字、>0の場合は数値を文字列に変換した値を返す
        /// </summary>
        /// <param name="val">変換したい数値</param>
        /// <returns>nullの場合は空文字、>=0の場合は数値を文字列に変換した値を返す</returns>
        public static string ToStringIgnoreNull(int? val)
        {
            string ret = "";

            if (val != null)
            {
                ret = val.ToString();
            }

            return ret;
        }

        /// <summary>
        /// 文字列が半角英数字記号かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が半角英数字記号の場合はtrue、それ以外はfalse</returns>
        public static bool IsASCII(string target)
        {
            return new Regex("^[\x20-\x7E]+$").IsMatch(target);
        }
        /// <summary>
        /// 文字列が半角カタカナ（句読点～半濁点）かどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が半角カタカナ（句読点～半濁点）の場合はtrue、それ以外はfalse</returns>
        public static bool IsHalfKatakanaPunctuation(string target)
        {
            return new Regex("^[\uFF61-\uFF9F]+$").IsMatch(target);
        }
        /// <summary>
        /// 文字列が全角カタカナかどうかを判定します
        /// </summary>
        /// <param name="target">対象の文字列
        /// <returns>文字列が全角カタカナの場合はtrue、それ以外はfalse</returns>
        public static bool IsFullKatakana(string target)
        {
            return new Regex("^\\p{IsKatakana}+$").IsMatch(target);
        }
        /// <summary>
        /// 半角カタカナ以外を除去する
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetKatakana(string target)
        {
            string ret = "";

            for (int i = 0; i < target.Length; i++)
            {
                if ((IsHalfKatakanaPunctuation(target.Substring(i, 1))) ||
                    (IsFullKatakana(target.Substring(i, 1))) ||
                   (target.Substring(i, 1) == " ") ||
                   (target.Substring(i, 1) == "　") ||
                   (target.Substring(i, 1) == "゛"))
                {
                    if (target.Substring(i, 1) != "･" &&
                       target.Substring(i, 1) != "・")
                    {
                        ret += target.Substring(i, 1);
                    }
                }
            }

            return ret;
        }
        /// <summary>
        /// 文字列がJIS X 0208 漢字第二水準までで構成されているかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <param name="containsHalfKatakana">漢字第二水準までに半角カタカナを含む場合はtrue、それ以外はfalse</param>
        /// <returns>文字列がJIS X 0208 漢字第二水準までで構成されている場合はtrue、それ以外はfalse</returns>
        public static bool IsUntilJISKanjiLevel2(string target, bool containsHalfKatakana, ref string retText, ref string badText)
        {
            bool retHantei = true;

            retText = "";
            badText = "";

            // 文字エンコーディングに「iso-2022-jp」を指定
            Encoding encoding = Encoding.GetEncoding("iso-2022-jp");
            // 文字列長を取得
            int length = target.Length;
            for (int i = 0; i < length; i++)
            {
                bool hantei = true;

                // 対象の部分文字列を取得
                string targetSubString = target.Substring(i, 1);

                // 改行コードの場合
                if (targetSubString == "\r" || targetSubString == "\n")
                {

                }
                // 半角英数字記号の場合
                else if (IsASCII(targetSubString) == true)
                {
                    //continue;
                }
                // 漢字第二水準までに半角カタカナを含まずかつ対象の部分文字列が半角カタカナの場合
                else if (containsHalfKatakana == false &&
                    IsHalfKatakanaPunctuation(targetSubString) == true)
                {
                    hantei = false;
                }
                else
                {
                    // 対象部分文字列の文字コードバイト配列を取得
                    byte[] targetBytes = encoding.GetBytes(targetSubString);
                    // 要素数が「1」の場合は漢字第三水準以降の漢字が「?」に変換された
                    if (targetBytes.Length == 1)
                    {
                        hantei = false;
                    }
                    // 文字コードバイト配列がJIS X 0208 漢字第二水準外の場合
                    if (IsUntilJISKanjiLevel2(targetBytes) == false)
                    {
                        hantei = false;
                    }
                }

                if (hantei)
                {
                    retText += targetSubString;
                }
                else
                {
                    badText += targetSubString;
                    retHantei = false;
                }
            }
            return retHantei;
        }
        /// <summary>
        /// 文字列がJIS X 0208 漢字第二水準までで構成されているかを判定します
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列がJIS X 0208 漢字第二水準までで構成されている場合はtrue、それ以外はfalse</returns>
        /// <remarks>句読点～半濁点の半角カタカナはJIS X 0208 漢字第二水準外と判定します</remarks>
        public static bool IsUntilJISKanjiLevel2(string target, ref string retText, ref string badText)
        {
            return IsUntilJISKanjiLevel2(target, false, ref retText, ref badText);
        }
        public static bool IsUntilJISKanjiLevel2InKana(string target, ref string retText, ref string badText)
        {
            return IsUntilJISKanjiLevel2(target, true, ref retText, ref badText);
        }
        /// <summary>
        /// 文字コードバイト配列がJIS X 0208 漢字第二水準までであるかを判定します
        /// </summary>
        /// <param name="targetBytes">文字コードバイト配列</param>
        /// <returns>文字コードバイト配列がJIS X 0208 漢字第二水準までである場合はtrue、それ以外はfalse</returns>
        private static bool IsUntilJISKanjiLevel2(byte[] targetBytes)
        {
            // 文字コードバイト配列の要素数が8ではない場合
            if (targetBytes.Length != 8)
            {
                return false;
            }
            // 区を取得
            int row = targetBytes[3] - 0x20;
            // 点を取得
            int cell = targetBytes[4] - 0x20;
            switch (row)
            {
                case 1: // 1区の場合
                    if (1 <= cell && cell <= 94)
                    {
                        // 1点～94点の場合
                        return true;
                    }
                    break;
                case 2: // 2区の場合
                    if (1 <= cell && cell <= 14)
                    {
                        // 1点～14点の場合
                        return true;
                    }
                    else if (26 <= cell && cell <= 33)
                    {
                        // 26点～33点の場合
                        return true;
                    }
                    else if (42 <= cell && cell <= 48)
                    {
                        // 42点～48点の場合
                        return true;
                    }
                    else if (60 <= cell && cell <= 74)
                    {
                        // 60点～74点の場合
                        return true;
                    }
                    else if (82 <= cell && cell <= 89)
                    {
                        // 82点～89点の場合
                        return true;
                    }
                    else if (cell == 94)
                    {
                        // 94点の場合
                        return true;
                    }
                    break;
                case 3: // 3区の場合
                    if (16 <= cell && cell <= 25)
                    {
                        // 16点～25点の場合
                        return true;
                    }
                    else if (33 <= cell && cell <= 58)
                    {
                        // 33点～58点の場合
                        return true;
                    }
                    else if (65 <= cell && cell <= 90)
                    {
                        // 65点～90点の場合
                        return true;
                    }
                    break;
                case 4: // 4区の場合
                    if (1 <= cell && cell <= 83)
                    {
                        // 1点～83点の場合
                        return true;
                    }
                    break;
                case 5: // 5区の場合
                    if (1 <= cell && cell <= 86)
                    {
                        // 1点～86点の場合
                        return true;
                    }
                    break;
                case 6: // 6区の場合
                    if (1 <= cell && cell <= 24)
                    {
                        // 1点～24点の場合
                        return true;
                    }
                    else if (33 <= cell && cell <= 56)
                    {
                        // 33点～56点の場合
                        return true;
                    }
                    break;
                case 7: // 7区の場合
                    if (1 <= cell && cell <= 33)
                    {
                        // 1点～33点の場合
                        return true;
                    }
                    else if (49 <= cell && cell <= 81)
                    {
                        // 49点～81点の場合
                        return true;
                    }
                    break;
                case 8: // 8区の場合
                    if (1 <= cell && cell <= 32)
                    {
                        // 1点～32点の場合
                        return true;
                    }
                    break;
                default:
                    if (16 <= row && row <= 46) // 16区～46区の場合
                    {
                        if (1 <= cell && cell <= 94)
                        {
                            // 1点～94点の場合
                            return true;
                        }
                    }
                    else if (row == 47) // 47区の場合
                    {
                        if (1 <= cell && cell <= 51)
                        {
                            // 1点～51点の場合
                            return true;
                        }
                    }
                    else if (48 <= row && row <= 83) // 48区～83区の場合
                    {
                        if (1 <= cell && cell <= 94)
                        {
                            // 1点～94点の場合
                            return true;
                        }
                    }
                    else if (row == 84) // 84区の場合
                    {
                        if (1 <= cell && cell <= 6)
                        {
                            // 1点～6点の場合
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        public static string MinuteToShowHour(int minute)
        {
            string ret = "";

            if (minute >= 60)
            {
                ret += (minute / 60).ToString() + "時間";
            }

            if (minute % 60 > 0)
            {
                ret += (minute % 60).ToString() + "分";
            }

            return ret;
        }

        /// <summary>
        /// 文字列をint型に変換する
        /// 変換に失敗した場合、defaultValを返す
        /// </summary>
        /// <param name="str">変換する文字列</param>
        /// <param name="defaultVal">変換できなかった時に返す値</param>
        /// <returns>引数strをint型に変換した値</returns>
        public static int StrToIntDef(string str, int defaultVal)
        {
            int ret;

            if (int.TryParse(str, out ret) == false)
            {
                ret = defaultVal;
            }

            return ret;
        }

        public static double StrToDoubleDef(string str, double defaultVal)
        {
            double ret;

            if (double.TryParse(str, out ret) == false)
            {
                ret = defaultVal;
            }

            return ret;
        }

        public static string JapanDayOfWeek(DateTime dateTime)
        {
            string result = string.Empty;
            switch (dateTime.DayOfWeek)
            {
                case System.DayOfWeek.Sunday:
                    result = "日";
                    break;
                case System.DayOfWeek.Monday:
                    result = "月";
                    break;
                case System.DayOfWeek.Tuesday:
                    result = "火";
                    break;
                case System.DayOfWeek.Wednesday:
                    result = "水";
                    break;
                case System.DayOfWeek.Thursday:
                    result = "木";
                    break;
                case System.DayOfWeek.Friday:
                    result = "金";
                    break;
                case System.DayOfWeek.Saturday:
                    result = "土";
                    break;
            }
            return result;
        }

        public static string TryCIToTimeZone(int time, string format = @"hh\:mm")
        {
            TimeSpan timeSpan = new TimeSpan();
            if ((0 <= time) && (time <= 2400))
            {
                int iHour = time / 100;
                int iMinute = time % 100;

                if (((0 <= iHour) && (iHour <= 24)) &&
                    ((0 <= iMinute) && (iMinute <= 59)))
                {
                    return iHour.AsString().PadLeft(2, '0') + ":" + iMinute.AsString().PadLeft(2, '0');
                }
            }

            return timeSpan.ToString(format);
        }


        //==================================================
        //  Copy関数のスペースを0に変換			処理
        //--------------------------------------------------
        //【In】	S			：変換文字
        //　　　　Index	：開始文字列番号
        //　　　　Count	：文字数
        //==================================================
        public static String CDCopy(string S, int Index, int Count)
        {
            int i;
            int WLen;

            string Result = Copy(S, Index, Count);

            WLen = Result.Length;
            //長さが長くなった分
            if (WLen < Count)
            {
                for (i = WLen; i < Count; i++)
                {
                    Result += "0";
                }
            }

            //スペースの分
            for (i = 0; i < Result.Length; i++)
            {
                if (Result[i].ToString() == " ")
                {
                    Result = Result.Substring(0, i) + "0" + Result.Substring(i + 1);
                }
            }
            return Result;
        }

        //------------------------------------------------------------------------------
        //  処理名  ：CiCopyStrWidthDst
        //  引数    ：Src     （切り出し元の文字列）
        //            Index   （開始位置）
        //            Count   （切り出す文字数）
        //            FmtFg   （1：Countで指定した文字数に半角スペース埋め）
        //            Dst     （切り出し後の文字列）
        //  説明    ：Src の Index 文字目から Count 個の文字の入った部分文字列を返します。
        //
        //  ※文字数（半角文字が1文字、全角文字が2文字）で計算
        //------------------------------------------------------------------------------
        public static string CiCopyStrWidthDst(string Src, int Index, int Count, int FmtFg, ref string Dst)
        {
            string Result = "";
            string sSrcStr = Src;
            Dst = sSrcStr;

            if (Index == 1 && MecsStringWidth(sSrcStr) < Count)
            {
                //長さが指定文字数以下ならそのまま返す
                Result = sSrcStr;
                if (FmtFg == 1)
                {
                    Result = Result + StringOfChar(" ", Count - MecsStringWidth(Result));
                }
                Dst = "";
            }
            else
            {
                if (Index > MecsStringWidth(sSrcStr))
                {
                    Result = "";
                    return Result;
                }
                //開始位置を調べる
                int iIndex = 0;
                int iWidth = 0;
                if (Index > 1)
                {
                    for (int i = 1; i < sSrcStr.Length; i++)
                    {
                        //１文字ずつ取得
                        iIndex++;

                        if (MecsIsFullWidth(sSrcStr[i]))
                        {
                            //全角文字
                            iWidth += 2;
                            if (iWidth == Index)
                            {
                                iIndex++;
                                Count--;
                            }
                            if (iWidth >= Index)
                            {
                                break;
                            }
                        }
                        else
                        {
                            //半角文字
                            iWidth++;
                            if (iWidth >= Index)
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    iIndex++;
                }

                int iWord = 0;
                int iBuf = 0;
                //開始位置から文字列終了までループ
                for (int i = iIndex; i <= sSrcStr.Length; i++)
                {
                    //１文字ずつ取得
                    string sBuf = MecsCopy(sSrcStr, i, 1);
                    iWord = iWord + MecsStringWidth(sBuf);

                    if (iWord > Count)
                    {
                        //指定エレメント数を超えた場合
                        iBuf = Result.Length + iIndex;
                        //切り取った残りの文字列
                        Dst = MecsCopy(sSrcStr, iBuf, sSrcStr.Length - iBuf + 1);
                        break;
                    }

                    //切り取った文字列
                    Result = Result + sBuf;

                    //切り取った残りの文字列
                    iBuf = MecsLength(Result) + 1;
                    Dst = MecsCopy(sSrcStr, iBuf, MecsLength(sSrcStr) - iBuf + 1);
                }
            }
            return Result;
        }

        public static int MecsLength(string value)
        {
            return value.Length;
        }

        //------------------------------------------------------------------------------
        //  処理名  ：CiCopyStrWidth
        //  引数    ：Src の Index 文字目から Count 個の文字の入った部分文字列を返します。
        //
        //  ※文字数（半角文字が1文字、全角文字が2文字）で計算
        //------------------------------------------------------------------------------
        public static string CiCopyStrWidth(string Src, int Index, int Count, int FmtFg = 0)
        {
            string Result = "";
            string sSrcStr = Src;

            if (Index == 1 && MecsStringWidth(sSrcStr) < Count)
            {
                //長さが指定文字数以下ならそのまま返す
                Result = sSrcStr;
                if (FmtFg == 1)
                {
                    Result = Result + StringOfChar(" ", Count - MecsStringWidth(Result));
                }
            }
            else
            {
                if (Index > MecsStringWidth(sSrcStr))
                {
                    Result = "";
                    return Result;
                }
                //開始位置を調べる
                int iIndex = 0;
                int iWidth = 0;
                if (Index > 1)
                {
                    //for (int i = 1; i < sSrcStr.Length; i++)
                    for (int i = 0; i < sSrcStr.Length; i++)
                    {
                        //１文字ずつ取得
                        iIndex++;

                        if (MecsIsFullWidth(sSrcStr[i]))
                        {
                            //全角文字
                            iWidth += 2;
                            if (iWidth == Index)
                            {
                                iIndex++;
                                Count--;
                            }
                            if (iWidth >= Index)
                            {
                                break;
                            }
                        }
                        else
                        {
                            //半角文字
                            iWidth++;
                            if (iWidth >= Index)
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    iIndex++;
                }

                int iWord = 0;

                //開始位置から文字列終了までループ
                for (int i = iIndex; i <= sSrcStr.Length; i++)
                {
                    //１文字ずつ取得
                    string sBuf = MecsCopy(sSrcStr, i, 1);
                    iWord = iWord + MecsStringWidth(sBuf);

                    if (iWord > Count)
                    {
                        //指定エレメント数を超えた場合
                        break;
                    }

                    //切り取った文字列
                    Result = Result + sBuf;
                }
            }
            return Result;
        }

        public static string StringOfChar(string character, int count)
        {
            string result = String.Empty;
            for (int i = 0; i < count; i++)
            {
                result = result + character;
            }
            return result;
        }

        public static string MecsCopy(string value, int index, int count)
        {
            string result = string.Empty;
            try
            {
                result = Copy(value, index, count);
            }
            catch (Exception ex)
            {

            }
            return result;
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

        /// <summary>
        /// 指定の文字列の幅を返す（半角1、全角2）
        /// </summary>
        /// <param name="stTarget">幅を取得する元になる文字列。<param>
        /// <returns>バイト数</returns>
        public static int LenB(string stTarget)
        {
            int ret = 0;

            if (stTarget != null)
            {
                for (int i = 0; i < stTarget.Length; i++)
                {
                    //Console.Write(stTarget[i]);
                    ret += LenB(stTarget[i]);
                }
            }

            return ret;
        }

        public static int LenB(char stTarget)
        {
            int ret = 0;

            Char ch = stTarget;
            if (Char.IsHighSurrogate(ch))
            {
                ret++;
            }
            else if (Char.IsLowSurrogate(ch))
            {
                ret++;
            }
            else if ((!((0x0020 <= (ch)) && ((ch) <= 0x007f))) && (!((0xff61 <= (ch)) && ((ch) <= 0xff9f))))
            {
                ret += 2;
            }
            else
            {
                ret++;
            }

            return ret;
        }

        public static string PadLeftB(string str, int len, char paddingChar = ' ')
        {
            string ret = str.TrimStart();

            if (len <= LenB(ret)) return ret;

            return (new string(paddingChar, len - CIUtil.LenB(ret))) + ret;
        }

        public static string PadRightB(string str, int len, char paddingChar = ' ')
        {
            string ret = str.TrimEnd();

            if (len <= LenB(ret)) return ret;

            return ret + (new string(paddingChar, len - CIUtil.LenB(ret)));
        }

        public static string FormatHpCd(string hpCd, int prefNo)
        {
            string wrkCd = hpCd == null ? new string(' ', 7) : string.Format("{0:D7}", hpCd.AsInteger());

            if (new int[] { 2, 12, 13, 17, 18, 25, 32, 35, 41, 43, 44, 46 }.Contains(prefNo))
            {
                //xx,xxxx,xx タイプ
                return string.Format("{0},{1},{2}", wrkCd.Substring(0, 2), wrkCd.Substring(2, 4), wrkCd.Substring(6, 1));
            }
            else if (new int[] { 3 }.Contains(prefNo))
            {
                //フォーマットなし
                return wrkCd;
            }
            else if (new int[] { 34 }.Contains(prefNo))
            {
                //xx-x,xxx,x タイプ
                return string.Format("{0}-{1},{2},{3}", wrkCd.Substring(0, 2), wrkCd.Substring(2, 1), wrkCd.Substring(3, 3), wrkCd.Substring(6, 1));
            }
            else
            {
                //xxx,xxx,x タイプ
                return string.Format("{0},{1},{2}", wrkCd.Substring(0, 3), wrkCd.Substring(3, 3), wrkCd.Substring(6, 1));
            }
        }

        public static string CheckPathDelimiter(String s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return @"\";
            }
            if (!s.EndsWith(@"\"))
                s += @"\";
            return s;
        }

        public static int GetByteCountFromString(string str)
        {
            return Encoding.GetEncoding("shift_JIS").GetByteCount(str);
        }

        public static string GetDayOfTheWeekShortCaption(DateTime dateTime)
        {
            string result = "";
            if (dateTime != new DateTime(0))
            {
                switch (dateTime.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        result = "月";
                        break;
                    case System.DayOfWeek.Tuesday:
                        result = "火";
                        break;
                    case System.DayOfWeek.Wednesday:
                        result = "水";
                        break;
                    case System.DayOfWeek.Thursday:
                        result = "木";
                        break;
                    case System.DayOfWeek.Friday:
                        result = "金";
                        break;
                    case System.DayOfWeek.Saturday:
                        result = "土";
                        break;
                    case System.DayOfWeek.Sunday:
                        result = "日";
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// JANコード（標準13桁）のチェックデジット計算
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CalcChkDgtJAN13(string code)
        {
            int iEven;
            int iOdd;

            string ret = "0";

            if ((code.Length != 12) || (CIUtil.StrToIntDef(code, -1) == -1)) return "";

            iEven = 0;
            iOdd = 0;

            int i = 12;
            while (i >= 1)
            {

                if (i % 2 == 0)
                {
                    iEven = iEven + StrToIntDef(Copy(code, i, 1), 0);
                }
                else
                {
                    iOdd = iOdd + StrToIntDef(Copy(code, i, 1), 0);
                }

                i--;
            }

            i = (iEven * 3) + iOdd;
            i = StrToIntDef(Copy(i.ToString(), i.ToString().Length, 1), 0);

            if (i > 0)
            {
                ret = (10 - i).ToString();
            }

            return ret;
        }

        /// <summary>
        /// チェックデジット
        ///     モジュラス10 ウエイト2・1分割
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CalcChkDgtM10W2(string code)
        {
            int weight = 2;
            int sum = 0;

            for (int i = code.Length; i >= 1; i--)
            {
                int wrkVal = code.Substring(i - 1, 1).AsInteger() * weight;
                //十の位と一の位を分けて足し合わせる（分割）
                sum += wrkVal % 10 + wrkVal / 10;

                //下の桁から2・1・2・1・...の順番に係数（ウエイト）をかける
                weight = weight == 2 ? 1 : 2;
            }

            //合計を10で割り、余りを求める（モジュラス）
            int modulus = sum % 10;

            //余りを10から引いたものをチェックデジットとする（但し、余りが0の場合はチェックデジットも0）
            return (modulus == 0 ? 0 : 10 - modulus).ToString();
        }

        /// <summary>
        /// 分割調剤の分割数量取得
        /// </summary>
        public static string GetBunkatuStr(string Str, int KouiCd)
        {
            string ret = "";
            string sTgt = Str;
            string sTani;

            if (KouiCd == 21)
            {
                //内服
                sTani = "日分";
            }
            else
            {
                sTani = "回分";
            }

            string[] bunkatuKaisus = sTgt.Split('+');

            foreach (string bunkatuKaisu in bunkatuKaisus)
            {
                if (ret != "")
                {
                    ret += "、";
                }

                ret = bunkatuKaisu + sTani;
            }

            if (ret != "")
            {
                ret = $"({ret})";
            }

            return ret;
        }

        /// <summary>
        /// Add days to date
        /// </summary>
        /// <param name="ymd"></param>
        /// <param name="addDay"></param>
        /// <returns></returns>
        public static int SDateInc(int ymd, int addDay)
        {
            DateTime workDate;
            if (DateTime.TryParseExact(ymd.AsString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out workDate))
            {
                workDate = workDate.AddDays(addDay);
                return workDate.ToString("yyyyMMdd").AsInteger();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// DateTime型を数値化する
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long DateTimeToLong(DateTime dt)
        {
            DateTime dateFrom = new System.DateTime(1899, 12, 30);
            return (long)(dt - dateFrom).TotalDays;
        }

        public static string StringReplace(string Source, string OldPattern, string NewPattern, TypeReplace typeReplace)
        {
            string result = "";
            if (string.IsNullOrEmpty(Source))
            {
                return result;
            }
            switch (typeReplace)
            {
                case TypeReplace.rfReplaceAll:
                    result = Source.Replace(OldPattern, NewPattern);
                    break;
                case TypeReplace.rfReplaceIgnoreCase:
                    result = Regex.Replace(Source, OldPattern, NewPattern, RegexOptions.IgnoreCase);
                    break;
                default:
                    break;
            }
            return result;
        }

        public static string FormatIntToString(int? num, string pattern = ",", bool isReturnEmpty = false)
        {
            if (num == null)
            {
                return isReturnEmpty == false ? "0" : "";
            }

            return FormatIntToString(num.Value, pattern, isReturnEmpty);
        }

        public static string FormatIntToString(int num, string pattern = ",", bool isReturnEmpty = false)
        {
            if (num == 0 && isReturnEmpty == false)
            {
                return "0";
            }

            return num.ToString($"#{pattern}###");
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
                        targetDateInt = DateTimeToInt(targetDate);
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

        public static string FormatStringToHex(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            var sb = new StringBuilder();
            var bytes = Encoding.Unicode.GetBytes(text);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }

        // convert hhmmss to hh:mm:ss
        public static string FormatHours(string time)
        {
            if (string.IsNullOrEmpty(time))
            {
                return string.Empty;
            }

            time = time.PadLeft(6, '0');

            int hours = time.Substring(0, 2).AsInteger();
            int minute = time.Substring(2, 2).AsInteger();
            int sec = time.Substring(4).AsInteger();

            if (hours > 23 || minute > 60 || sec > 60)
            {
                return string.Empty;
            }

            return time.Substring(0, 2) + ":" + time.Substring(2, 2) + ":" + time.Substring(4);
        }

        public static bool IsFileImage(string pathFile)
        {
            if (string.IsNullOrEmpty(pathFile))
            {
                return false;
            }

            List<string> listExtensionImage = new List<string>() { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".ico", ".tif", ".tiff", ".wmf" };
            var extension = Path.GetExtension(pathFile).ToLower();
            return listExtensionImage.Contains(extension);
        }

        public static bool IsNetworkPath(string pathFile)
        {
            if (string.IsNullOrEmpty(pathFile))
            {
                return false;
            }

            string patternNetWork = @"\\";
            if (pathFile.StartsWith(patternNetWork))
            {
                return true;
            }

            return false;
        }

        public static bool IsNullOrEmptyOrWhiteSpace(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 郵便番号をハイフン付きに変換
        /// </summary>
        /// <param name="postcd"></param>
        /// <returns></returns>
        public static string GetDspPostCd(string postcd)
        {
            string ret = postcd ?? "";

            if (ret.Length > 5 && ret.Contains("-") == false)
            {
                ret = $"{ret.Substring(0, 3)}-{ret.Substring(3, ret.Length - 3)}";
            }
            return ret;
        }

        public static string GetDisplayGender(int sex)
        {
            return sex == 1 ? "男" : sex == 2 ? "女" : "未設定";
        }

        public static string CombinePath(string path1, string path2, string path3 = "", string path4 = "")
        {
            try
            {
                string FormatPathToCombine(string path)
                {
                    if (string.IsNullOrEmpty(path)) return string.Empty;
                    return path.TrimStart(Path.DirectorySeparatorChar).TrimStart(Path.AltDirectorySeparatorChar);
                }

                if (string.IsNullOrEmpty(path1))
                {
                    return string.Empty;
                }
                if (!path1.StartsWith(@"\\"))
                {
                    path1 = path1.Insert(0, @"\\");
                }
                if (string.IsNullOrEmpty(path2))
                {
                    return path1;
                }
                else if (string.IsNullOrEmpty(path3))
                {
                    return Path.Combine(path1, FormatPathToCombine(path2));
                }
                else if (string.IsNullOrEmpty(path4))
                {
                    return Path.Combine(path1, FormatPathToCombine(path2), FormatPathToCombine(path3));
                }
                else
                {
                    return Path.Combine(path1, FormatPathToCombine(path2), FormatPathToCombine(path3), FormatPathToCombine(path4));
                }
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }

    public enum TypeReplace
    {
        rfReplaceAll,
        rfReplaceIgnoreCase
    }

    public enum WarekiFormat
    {
        Short,
        Full,
        Mix
    }
}
