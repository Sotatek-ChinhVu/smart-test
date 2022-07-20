using Helper.Extendsions;

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
    }

    public static class RinjiKubun
    {
        internal static readonly int NissuHandan = 0;
        internal static readonly int Rinji = 1;
        internal static readonly int Jotai = 2;
    }
}
