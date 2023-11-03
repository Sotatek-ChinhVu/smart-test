using Helper.Constants;
using Helper.Extension;
using System.Globalization;
using System.Text;

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

        public static string GetCmtOpt840(string input)
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

            if (intConvertValue < 0)
            {
                return cmtOpt;
            }

            return input;
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

        public static string GetItemNameComment(string cmtName, string cmtOpt,
                int pos1, int length1, int pos2, int length2, int pos3, int length3, int pos4, int length4)
        {
            string markComment = "＊";

            string itemName = cmtName;
            if (pos1 > 0 && length1 > 0)
            {
                StringBuilder val1 = new();

                if (string.IsNullOrEmpty(cmtOpt))
                {
                    for (int i = 0; i < length1; i++)
                    {
                        val1.Append(val1 + markComment);
                    }
                }
                else
                {
                    val1.Append(CIUtil.Copy(cmtOpt, 1, length1));
                }

                string partRight = CIUtil.Copy(itemName, pos1 + length1, itemName.Length - (pos1 + length1 - 1));
                itemName = CIUtil.Copy(itemName, 1, pos1 - 1) + HenkanJ.Instance.ToFullsize(val1.ToString()) + partRight;
            }

            if (pos2 > 0 && length2 > 0)
            {
                StringBuilder val2 = new();

                if (string.IsNullOrEmpty(cmtOpt))
                {
                    for (int i = 0; i < length2; i++)
                    {
                        val2.Append(val2 + markComment);
                    }
                }
                else
                {
                    val2.Append(CIUtil.Copy(cmtOpt, length1 + 1, length2));
                }

                string partRight = CIUtil.Copy(itemName, pos2 + length2, itemName.Length - (pos2 + length2 - 1));
                itemName = CIUtil.Copy(itemName, 1, pos2 - 1) + HenkanJ.Instance.ToFullsize(val2.ToString()) + partRight;
            }

            if (pos3 > 0 && length3 > 0)
            {
                StringBuilder val3 = new();

                if (string.IsNullOrEmpty(cmtOpt))
                {
                    for (int i = 0; i < length3; i++)
                    {
                        val3.Append(val3 + markComment);
                    }
                }
                else
                {
                    val3.Append(CIUtil.Copy(cmtOpt, length1 + length2 + 1, length3));
                }

                string partRight = CIUtil.Copy(itemName, pos3 + length3, itemName.Length - (pos3 + length3 - 1));
                itemName = CIUtil.Copy(itemName, 1, pos3 - 1) + HenkanJ.Instance.ToFullsize(val3.ToString()) + partRight;
            }

            if (pos4 > 0 && length4 > 0)
            {
                StringBuilder val4 = new();

                if (string.IsNullOrEmpty(cmtOpt))
                {
                    for (int i = 0; i < length4; i++)
                    {
                        val4.Append(val4 + markComment);
                    }
                }
                else
                {
                    val4.Append(CIUtil.Copy(cmtOpt, length1 + length2 + length3 + 1, length4));
                }

                string partRight = CIUtil.Copy(itemName, pos4 + length4, itemName.Length - (pos4 + length4 - 1));
                itemName = CIUtil.Copy(itemName, 1, pos4 - 1) + HenkanJ.Instance.ToFullsize(val4.ToString()) + partRight;
            }

            return itemName;
        }

        /// <summary>
        /// 先発品
        /// </summary>
        /// <param name="syohoKbn">処方せん記載区分</param>
        /// <param name="syohoLimitKbn">処方せん記載制限区分</param>
        /// <returns></returns>
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

        public static string GetCmtOptDisplay850(string input, string itemName)
        {
            string cmtOpt = GetCmtOpt850(input, itemName);
            if (string.IsNullOrEmpty(cmtOpt))
            {
                return string.Empty;
            }

            if (cmtOpt.Length == 7)
            {
                string tempCmtOpt = HenkanJ.Instance.ToHalfsize(cmtOpt);
                string formatDate = string.Format("{0}.{1}.{2}.{3}",
                    CIUtil.Copy(tempCmtOpt, 1, 1),
                    CIUtil.Copy(tempCmtOpt, 2, 2),
                    CIUtil.Copy(tempCmtOpt, 4, 2),
                    CIUtil.Copy(tempCmtOpt, 6, 2));
                var tempConvertDate = CIUtil.ShowWDateToSDate(formatDate);
                if (tempConvertDate == 0)
                {
                    return string.Empty;
                }

                var warekiYmd = CIUtil.SDateToShowWDate3(tempConvertDate);
                return HenkanJ.Instance.ToFullsize(warekiYmd.Ymd.Replace(" ", ""));
            }
            else
            {
                string tempCmtOpt = HenkanJ.Instance.ToHalfsize(cmtOpt);
                string formatDate = string.Format("{0}.{1}.{2}.{3}",
                    CIUtil.Copy(tempCmtOpt, 1, 1),
                    CIUtil.Copy(tempCmtOpt, 2, 2),
                    CIUtil.Copy(tempCmtOpt, 4, 2),
                    "01");
                var tempConvertDate = CIUtil.ShowWDateToSDate(formatDate);
                if (tempConvertDate == 0)
                {
                    return string.Empty;
                }

                var warekiYmd = CIUtil.SDateToShowWDate3(tempConvertDate);
                string strWarekiYmn = warekiYmd.Ymd.Replace(" ", "");
                return HenkanJ.Instance.ToFullsize(CIUtil.Copy(strWarekiYmn, 1, strWarekiYmn.Length - 3));
            }
        }

        public static string GetCmtOptDisplay851(string cmtOpt)
        {
            if (string.IsNullOrEmpty(cmtOpt))
            {
                return string.Empty;
            }
            string hours = CIUtil.Copy(cmtOpt, 1, 2);
            string min = CIUtil.Copy(cmtOpt, 3, 2);
            //Set _inputName with mark comment and raise this changed
            return string.Format("{0}時{1}分", hours, min);
        }

        public static string GetCmtOptDisplay852(string cmtOpt)
        {
            if (string.IsNullOrEmpty(cmtOpt))
            {
                return string.Empty;
            }

            var value = HenkanJ.Instance.ToHalfsize(cmtOpt).AsInteger().AsString();

            return string.Format("{0}分", HenkanJ.Instance.ToFullsize(value));
        }

        public static string GetCmtOptDisplay853(string cmtOpt)
        {
            if (string.IsNullOrEmpty(cmtOpt))
            {
                return string.Empty;
            }

            string day = CIUtil.Copy(cmtOpt, 1, 2);
            string hours = CIUtil.Copy(cmtOpt, 3, 2);
            string min = CIUtil.Copy(cmtOpt, 5, 2);

            return $"{day}日　{hours}時{min}分";
        }

        public static string GetCmtOptDisplay880(string cmtOpt)
        {
            if (string.IsNullOrWhiteSpace(cmtOpt))
            {
                return string.Empty;
            }

            cmtOpt = HenkanJ.Instance.ToHalfsize(cmtOpt).PadLeft(15, '0');

            string cmtOptDate = CIUtil.Copy(cmtOpt, 1, 7);
            string cmtOptValue = CIUtil.Copy(cmtOpt, 8, 8);

            string formatDate = string.Format("{0}.{1}.{2}.{3}",
                    CIUtil.Copy(cmtOptDate, 1, 1),
                    CIUtil.Copy(cmtOptDate, 2, 2),
                    CIUtil.Copy(cmtOptDate, 4, 2),
                    CIUtil.Copy(cmtOptDate, 6, 2));
            var tempConvertDate = CIUtil.ShowWDateToSDate(formatDate);
            if (tempConvertDate == 0)
            {
                return string.Empty;
            }

            var warekiYmd = CIUtil.SDateToShowWDate3(tempConvertDate);
            var date = HenkanJ.Instance.ToFullsize(warekiYmd.Ymd.Replace(" ", ""));

            string value = "0";
            if (cmtOptValue != "00000000")
            {
                value = cmtOptValue.TrimStart('0');
                if (value.Length > 0 && value[0] == '.')
                {
                    value = "0" + value;
                }
            }

            return $"{date}　検査値：{HenkanJ.Instance.ToFullsize(value)}";
        }
    }


    public static class RinjiKubun
    {
        internal static readonly int NissuHandan = 0;
        internal static readonly int Rinji = 1;
        internal static readonly int Jotai = 2;
    }
}
