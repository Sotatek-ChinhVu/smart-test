﻿using Helper.Extension;
using System.Globalization;

namespace Helper.Common
{
    public static class OdrUtil
    {
        /// <summary>
        /// 行為区分名取得
        /// </summary>
        /// <param name = "kouiCode" > 行為コード </ param >
        /// < returns ></ returns >
        public static string GetOdrGroupName(int kouiCode)
        {
            string Result = string.Empty;
            // kouiCode = 100, kouiCode = 101
            if (kouiCode.AsString().Length == 3)
            {
                Result = "処方";
            }
            else
            {
                int wkNo = CIUtil.Copy(kouiCode.ToString("D2"), 1, 1).AsInteger();
                if (wkNo == 1)
                {
                    if (kouiCode == 14)
                    {
                        Result = "在宅";
                    }
                    else if (kouiCode == 10)
                    {
                        Result = "初再診";
                    }
                    else
                    {
                        Result = "医学管理";
                    }
                }
                else if (wkNo == 2)
                {
                    Result = "処方";
                }
                else if (wkNo == 3)
                {
                    Result = "注射";
                }
                else if (wkNo == 4)
                {
                    Result = "処置";
                }
                else if (wkNo == 5)
                {
                    Result = "手術";
                }
                else if (wkNo == 6)
                {
                    if ((kouiCode >= 68 && kouiCode < 70))
                        Result = "画像";
                    else
                        Result = "検査";
                }
                else if (wkNo == 7)
                {
                    Result = "画像";
                }
                else if (wkNo == 8)
                {
                    Result = "その他";
                }
                else if (wkNo == 9)
                {
                    if ((kouiCode >= 90 && kouiCode < 95))
                        Result = "入院";
                    else if ((kouiCode >= 95 && kouiCode < 97))
                        Result = "自費";
                    else if ((kouiCode >= 97 && kouiCode < 99))
                        Result = "食事";
                    else if (kouiCode >= 99)
                        Result = "コメント";
                }
                else
                {
                    Result = "コメント";
                }
            }

            return Result;
        }

        /// <summary>
        /// Using to check comment master only
        /// </summary>
        /// <param name="text"></param>
        /// <param name="CmtName"></param>
        /// <returns></returns>
        public static bool IsFree830Prefix(string text, string CmtName)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(CmtName)) return false;
            return text.StartsWith(CmtName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kouiCode">行為コード</param>
        /// <param name="inoutKbn">院内院外区分</param>
        /// <returns></returns>
        public static string GetInOutName(int kouiCode, int inoutKbn)
        {
            string Result = string.Empty;
            //To do hardcode setting 検査依頼有無
            int FKensaUmu = 1;

            // kouiCode = 100, kouiCode = 101
            if (kouiCode.AsString().Length == 3)
            {
                //処方箋区分(0:院内 1:院外)
                if (inoutKbn == 0)
                    Result = "院内";
                else
                    Result = "院外";
            }
            else
            {
                int wkNo = CIUtil.Copy(kouiCode.ToString("D2"), 1, 1).AsInteger();
                if (wkNo == 2)
                {
                    //処方箋区分(0:院内 1:院外)
                    if (inoutKbn == 0)
                        Result = "院内";
                    else
                        Result = "院外";
                }
                else if (wkNo == 6)
                {
                    if (!(new List<int>() { 1, 3, 4 }).Contains(FKensaUmu))
                    {
                        return Result;
                    }
                    if (inoutKbn == 0)
                        Result = "院内";
                    else
                        Result = "院外";

                    return Result;
                }
            }

            return Result;
        }

        /// <summary>
        /// 0: 日数判断
        /// 1: 臨時
        /// 2: 常態
        /// </summary>
        /// <param name="SyohoSbt"></param>
        /// <returns></returns>
        public static string GetSikyuName(int SyohoSbt)
        {
            if (SyohoSbt == RinjiKubun.NissuHandan)
            {
                return "日数";
            }
            if (SyohoSbt == RinjiKubun.Rinji)
            {
                return "臨時";
            }
            if (SyohoSbt == RinjiKubun.Jotai)
            {
                return "常態";
            }
            else
            {
                return "";
            }
        }

        public static string GetSikyuKensa(int sikyuKbn, int tosekiKbn)
        {
            if (tosekiKbn == 0)
            {
                if (sikyuKbn == 0)
                {
                    return "通常";
                }
                else if (sikyuKbn == 1)
                {
                    return "至急";
                }
                else
                {
                    return "";
                }
            }
            else if (tosekiKbn == 1)
            {
                if (sikyuKbn == 0)
                {
                    return "透前";
                }
                else if (sikyuKbn == 1)
                {
                    return "透前急";
                }
                else
                {
                    return "";
                }
            }
            else if (tosekiKbn == 2)
            {
                if (sikyuKbn == 0)
                {
                    return "透後";
                }
                else if (sikyuKbn == 1)
                {
                    return "透後急";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public static int GetGroupKoui(int odrKouiKbn)
        {
            var result = odrKouiKbn / 10 * 10;

            if (11 <= odrKouiKbn && odrKouiKbn <= 13)
            {
                // NuiTran recommend handle this case
                result = 11;
            }
            else if (odrKouiKbn == 14)
            {
                // 在宅
                result = odrKouiKbn;
            }
            else if (odrKouiKbn >= 68 && odrKouiKbn < 70)
            {
                // 画像
                result = odrKouiKbn;
            }
            else if (odrKouiKbn >= 95 && odrKouiKbn < 99)
            {
                // コメント以外
                result = odrKouiKbn;
            }
            else if (odrKouiKbn == 100 || odrKouiKbn == 101)
            {
                // コメント（処方箋）
                // コメント（処方箋備考）
                result = 20;
            }

            return result;
        }

        public static string GetCmtOpt850(string input, string itemName)
        {
            string cmtOpt = string.Empty;
            if (string.IsNullOrWhiteSpace(input))
            {
                return cmtOpt;
            }

            if (string.IsNullOrWhiteSpace(itemName))
            {
                return cmtOpt;
            }

            string halfSizeValue = HenkanJ.Instance.ToHalfsize(input);
            if (itemName.Contains("日"))
            {
                if (halfSizeValue.AsInteger() != 0 && halfSizeValue.Length == 7)
                {
                    string formatDate = string.Format("{0}.{1}.{2}.{3}",
                        CIUtil.Copy(halfSizeValue, 1, 1),
                        CIUtil.Copy(halfSizeValue, 2, 2),
                        CIUtil.Copy(halfSizeValue, 4, 2),
                        CIUtil.Copy(halfSizeValue, 6, 2));
                    int tempConvertDate = CIUtil.ShowWDateToSDate(formatDate);
                    if (tempConvertDate == 0)
                    {
                        return cmtOpt;
                    }
                    return HenkanJ.Instance.ToFullsize(halfSizeValue);
                }
                else
                {
                    int intDateValue = CIUtil.ShowWDateToSDate(halfSizeValue);
                    if (intDateValue == 0 || intDateValue > 99999999)
                    {
                        return cmtOpt;
                    }
                    cmtOpt = HenkanJ.Instance.ToFullsize(CIUtil.SDateToWDate(intDateValue).AsString());
                    return cmtOpt;
                }
            }
            else
            {
                if (halfSizeValue.AsInteger() != 0 && halfSizeValue.Length == 5)
                {
                    string formatDate = string.Format("{0}.{1}.{2}.{3}",
                        CIUtil.Copy(halfSizeValue, 1, 1),
                        CIUtil.Copy(halfSizeValue, 2, 2),
                        CIUtil.Copy(halfSizeValue, 4, 2),
                        "01");
                    int tempConvertDate = CIUtil.ShowWDateToSDate(formatDate);
                    if (tempConvertDate == 0)
                    {
                        return cmtOpt;
                    }
                    return halfSizeValue;
                }
                else
                {
                    string daySuffix = "01";
                    if (halfSizeValue.Contains("."))
                    {
                        daySuffix = "." + daySuffix;
                    }
                    int intDateValue = CIUtil.ShowWDateToSDate(halfSizeValue + daySuffix);
                    if (intDateValue == 0 || intDateValue > 99999999)
                    {
                        return cmtOpt;
                    }
                    string strDateValue = CIUtil.SDateToWDate(intDateValue).AsString();
                    cmtOpt = CIUtil.Copy(strDateValue, 1, strDateValue.Length - 2);
                    return cmtOpt;
                }
            }
        }

        public static string GetCmtOpt851(string input)
        {
            string cmtOpt = string.Empty;
            if (string.IsNullOrWhiteSpace(input))
            {
                return cmtOpt;
            }

            string halfSizeValue = HenkanJ.Instance.ToHalfsize(input);
            int intConvertValue;
            if (!int.TryParse(halfSizeValue, out intConvertValue))
            {
                return cmtOpt;
            }

            if (intConvertValue >= 2400)
            {
                return cmtOpt;
            }

            halfSizeValue = intConvertValue.AsString();

            string convertTimeFormat = CIUtil.FormatTimeHHmmss(halfSizeValue);
            if (string.IsNullOrEmpty(convertTimeFormat))
            {
                return cmtOpt;
            }

            return HenkanJ.Instance.ToFullsize(halfSizeValue.PadLeft(4, '0'));
        }

        public static string GetCmtOpt852(string input)
        {
            string cmtOpt = string.Empty;
            if (string.IsNullOrWhiteSpace(input))
            {
                return cmtOpt;
            }

            string halfSizeValue = HenkanJ.Instance.ToHalfsize(input);
            int intConvertValue;
            if (!int.TryParse(halfSizeValue, out intConvertValue))
            {
                return cmtOpt;
            }

            if (intConvertValue > 99999)
            {
                return cmtOpt;
            }

            return HenkanJ.Instance.ToFullsize(intConvertValue.AsString().PadLeft(5, '0'));
        }

        public static string GetCmtOpt853(string input, int sinDate = 0)
        {
            string cmtOpt = string.Empty;
            if (string.IsNullOrWhiteSpace(input))
            {
                return cmtOpt;
            }

            input = CIUtil.Copy(input.PadLeft(6, '0'), 1, 6);
            string halfSizeValue = HenkanJ.Instance.ToHalfsize(input);
            string day = CIUtil.Copy(halfSizeValue, 1, 2);
            string time = CIUtil.Copy(halfSizeValue, 3, 4);
            int intConvertValue;
            int intConvertDay;

            if (!int.TryParse(day, out intConvertDay))
            {
                return cmtOpt;
            }

            if (!int.TryParse(time, out intConvertValue))
            {
                return cmtOpt;
            }
            int countDay = 31;

            if (sinDate > 0)
            {
                DateTime.TryParseExact(sinDate.AsString(), "yyyyMMdd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

                countDay = DateTime.DaysInMonth(date.Year, date.Month);
            }

            if (intConvertValue >= 2400 || intConvertDay < 1 || intConvertDay > countDay)
            {
                return cmtOpt;
            }

            return HenkanJ.Instance.ToFullsize(halfSizeValue.PadLeft(6, '0'));
        }

        //Get kouiKbnName by setting for orderSheet
        public static string GetKouiKbnNmBySetting(int val)
        {
            switch (val)
            {
                case 1:
                    return "医学管理";
                case 2:
                    return "在宅";
                case 3:
                    return "処方";
                case 4:
                    return "内服";//処方
                case 5:
                    return "頓服";//処方
                case 6:
                    return "外用";//処方
                case 7:
                    return "その他";//処方
                case 8:
                    return "注射";
                case 9:
                    return "皮下筋注";//注射
                case 10:
                    return "静注";//注射
                case 11:
                    return "点滴";//注射
                case 12:
                    return "他注";//注射
                case 13:
                    return "処置";
                case 14:
                    return "手術";
                case 15:
                    return "検査";
                case 16:
                    return "検体";//検査
                case 17:
                    return "生体";//検査
                case 18:
                    return "その他";//検査
                case 19:
                    return "画像";
                case 20:
                    return "その他";
                case 21:
                    return "自費";
                default:
                    return string.Empty;
            }
        }

        public static string GetChildOdrGrpKouiName(int kouiCode)
        {
            //item comment 処方箋コメント、処方箋備考コメント
            if (kouiCode == 100 || kouiCode == 101)
            {
                return "その他";
            }
            int grpKouiKbn = kouiCode / 10;
            switch (grpKouiKbn)
            {
                case 2:
                    switch (kouiCode)
                    {
                        case 21:
                            return "内服";
                        case 22:
                            return "頓服";
                        case 23:
                            return "外用";
                        default:
                            return "その他";
                    }
                case 3:
                    switch (kouiCode)
                    {
                        case 31:
                            return "皮下筋注";
                        case 32:
                            return "静注";
                        case 33:
                            return "点滴";
                        default:
                            //kouiKbn = 30 || kouiKbn = 34
                            return "他注";
                    }
                case 6:
                    switch (kouiCode)
                    {
                        case 61:
                            return "検体";
                        case 62:
                            return "生体";
                        default:
                            return "その他";
                    }
                default:
                    return string.Empty;
            }
        }
    }

    public static class RinjiKubun
    {
        internal static readonly int NissuHandan = 0;
        internal static readonly int Rinji = 1;
        internal static readonly int Jotai = 2;
    }
}
