using Helper.Common;
using Helper.Constants;

namespace Domain.Models.ReceSeikyu
{
    public class RegisterRequestModel
    {
        public RegisterRequestModel(long ptId, string ptName, int sinYm, int seikyuYm, int seikyuKbn, int newSeikyuKbn, int hokenId, string hokensyaNo, int hokenKbn, string houbetu, int honkeKbn, int hokenStartDate, int hokenEndDate, bool isModified)
        {
            PtId = ptId;
            PtName = ptName;
            SinYm = sinYm;
            SeikyuYm = seikyuYm;
            SeikyuKbn = seikyuKbn;
            NewSeikyuKbn = newSeikyuKbn;
            HokenId = hokenId;
            HokensyaNo = hokensyaNo;
            HokenKbn = hokenKbn;
            Houbetu = houbetu;
            HonkeKbn = honkeKbn;
            HokenStartDate = hokenStartDate;
            HokenEndDate = hokenEndDate;
            IsModified = isModified;
        }

        public long PtId { get; private set; }


        public string PtName { get; private set; }

        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinYm { get; private set; }

        public string SinYmDisplay
        {
            get
            {
                return CIUtil.SMonthToShowSMonth(SinYm);
            }
        }

        /// <summary>
        /// 請求年月
        /// </summary>
        public int SeikyuYm { get; private set; }

        public string SeikyuYmDisplay
        {
            get
            {
                return CIUtil.SMonthToShowSMonth(SeikyuYm);
            }
        }

        /// <summary>
        /// 請求区分
        /// 1:月遅れ 2:返戻 3:オンライン返戻
        /// </summary>
        public string SeikyuKbnDisplay
        {
            get
            {
                switch (SeikyuYm)
                {
                    case 1:
                        return "月遅れ";
                    case 2:
                        return "返戻";
                    case 3:
                        return "オンライン返戻";
                    default:
                        return string.Empty;
                }
            }
        }

        public int SeikyuKbn { get; private set; }

        public int NewSeikyuKbn { get; private set; }

        public string NewSeikyuKbnDisplay
        {
            get
            {
                switch (NewSeikyuKbn)
                {
                    case 1:
                        return "月遅れ";
                    case 2:
                        return "返戻";
                    default:
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// 保険ID
        /// </summary>
        public int HokenId { get; private set; }

        public string HokensyaNo { get; private set; }

        public int HokenKbn { get; private set; }

        public string Houbetu { get; private set; }

        public int HonkeKbn { get; private set; }

        public string HokenSentaku
        {
            get
            {
                string result = string.Empty;
                if (HokenId == 0)
                {
                    return string.Empty;
                }
                result += HokenId.ToString().PadLeft(2, '0');
                result += " ";
                if (string.IsNullOrEmpty(HokensyaNo))
                {
                    return string.Empty;
                }

                switch (HokenKbn)
                {
                    case 0:
                        if (Houbetu == HokenConstant.HOUBETU_JIHI_108)
                        {
                            result += "自費";
                        }
                        else if (Houbetu == HokenConstant.HOUBETU_JIHI_109)
                        {
                            result += "自費レセ";
                        }
                        else
                        {
                            result += "自費";
                        }
                        break;
                    case 1:
                        if (Houbetu == HokenConstant.HOUBETU_NASHI)
                        {
                            result += "公費";
                        }
                        else
                        {
                            result += "社保";
                        }
                        break;
                    case 2:
                        if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("39"))
                        {
                            result += "後期";
                        }
                        else if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("67"))
                        {
                            result += "退職";
                        }
                        else
                        {
                            result += "国保";
                        }
                        break;
                    case 11:
                        result += "労災（短期給付）";
                        break;
                    case 12:
                        result += "労災（傷病年金）";
                        break;
                    case 13:
                        result += "労災（アフターケア）";
                        break;
                    case 14:
                        result += "自賠責";
                        break;
                }

                if (HonkeKbn != 0)
                {
                    result += "(";
                    if (HonkeKbn == 1)
                    {
                        result += "本人";
                    }
                    else
                    {
                        result += "家族";
                    }
                    result += ")";
                }

                string sBuff;
                if (HokenStartDate > 0)
                {
                    sBuff = string.Format("{0, -11}", CIUtil.SDateToShowWDate(HokenStartDate));
                }
                else
                {
                    sBuff = string.Format("{0, -11}", " ");
                }

                sBuff += " ～ ";

                if (HokenEndDate > 0 && HokenEndDate < 99999999)
                {
                    sBuff += string.Format("{0, -11}", CIUtil.SDateToShowWDate(HokenEndDate));
                }
                else
                {
                    sBuff += string.Format("{0, -11}", " ");
                }

                return result + " " + sBuff;
            }
        }

        public int HokenStartDate { get; private set; }

        public int HokenEndDate { get; private set; }

        public bool IsModified { get; private set; }

        public bool CheckDefaultValue() => PtId == 0;
    }
}
