using Domain.Constant;
using Helper.Common;

namespace Domain.Models.ReceSeikyu
{
    public class RecedenHenJiyuuModel
    {
        public RecedenHenJiyuuModel(int hpId, long ptId, int hokenId, int sinYm, int seqNo, string henreiJiyuuCd, string henreiJiyuu, string hosoku, int isDeleted, int hokenKbn, string houbetu, int hokenStartDate, int hokenEndDate, string hokensyaNo)
        {
            HpId = hpId;
            PtId = ptId;
            HokenId = hokenId;
            SinYm = sinYm;
            SeqNo = seqNo;
            HenreiJiyuuCd = henreiJiyuuCd;
            HenreiJiyuu = henreiJiyuu;
            Hosoku = hosoku;
            IsDeleted = isDeleted;
            HokenKbn = hokenKbn;
            Houbetu = houbetu;
            HokenStartDate = hokenStartDate;
            HokenEndDate = hokenEndDate;
            HokensyaNo = hokensyaNo;
        }


        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; private set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId { get; private set; }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        public int HokenId { get; private set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm { get; private set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo { get; private set; }

        /// <summary>
        /// 返戻事由コード
        /// 0
        /// </summary>
        public string HenreiJiyuuCd { get; private set; }

        /// <summary>
        /// 返戻事由
        /// 
        /// </summary>
        public string HenreiJiyuu { get; private set; }

        /// <summary>
        /// 補足情報
        /// 
        /// </summary>
        public string Hosoku { get; private set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted { get; private set; }

        public int HokenKbn { get; private set; }

        /// <summary>
        /// in PtHokenInf
        /// </summary>
        public string Houbetu { get; private set; }

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
                if (Houbetu == null)
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

                if (this.HokenKbn != 0)
                {
                    result += "(";
                    if (this.HokenKbn == 1)
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

        public string HokensyaNo { get; private set; }
    }
}
