using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace Reporting.Receipt.Models
{
    public class RecePreviewModel
    {
        public ReceInf ReceInf { get; }

        public RecePreviewModel(ReceInf receInf)
        {
            ReceInf = receInf;
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId => ReceInf.HpId;

        /// <summary>
        /// 請求年月
        /// </summary>
        public int SeikyuYm => ReceInf.SeikyuYm;

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId => ReceInf.PtId;

        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinYm => ReceInf.SinYm;

        /// <summary>
        /// 主保険保険ID
        /// </summary>
        public int HokenId => ReceInf.HokenId;
        public int HOkenId2 => ReceInf.HokenId2;
        /// <summary>
        /// 診療科ID
        /// </summary>
        public int KaId => ReceInf.KaId;

        /// <summary>
        /// 担当医ID
        /// </summary>
        public int TantoId => ReceInf.TantoId;

        /// <summary>
        /// レセプト種別
        /// </summary>
        public string ReceSbt => ReceInf.ReceSbt;

        /// <summary>
        /// 保険区分
        /// </summary>
        public int HokenKbn => ReceInf.HokenKbn;

        public string SeikyuYmDisplay => CIUtil.SMonthToShowSMonth(ReceInf.SeikyuYm);

        public string SinYmDisplay => CIUtil.SMonthToShowSMonth(ReceInf.SinYm);

        #region Hoken pattern info
        public string HokenPatternName => GetHokenName();

        public int HokenSbtCd => ReceInf.HokenSbtCd;

        public string Houbetu => ReceInf.Houbetu;

        public int Kohi1Id => ReceInf.Kohi1Id;

        public int Kohi2Id => ReceInf.Kohi2Id;

        public int Kohi3Id => ReceInf.Kohi3Id;

        public int Kohi4Id => ReceInf.Kohi4Id;

        public string Kohi1Houbetu => ReceInf.Kohi1Houbetu;

        public string Kohi2Houbetu => ReceInf.Kohi2Houbetu;

        public string Kohi3Houbetu => ReceInf.Kohi3Houbetu;

        public string Kohi4Houbetu => ReceInf.Kohi4Houbetu;

        public int HonkeKbn => ReceInf.HonkeKbn;

        public bool HaveKohi
        {
            get => ReceInf.Kohi1Id > 0 ||
                   ReceInf.Kohi2Id > 0 ||
                   ReceInf.Kohi3Id > 0 ||
                   ReceInf.Kohi4Id > 0;
        }

        private string GetHokenName()
        {
            if (PtId == 0 && SinYm == 0 && HokenId == 0) return string.Empty;

            string hokenName = string.Empty;

            string prefix = string.Empty;
            string postfix = string.Empty;
            if (CheckDefaultValue()) return hokenName;
            if (HokenSbtCd == 0)
            {
                switch (HokenKbn)
                {
                    case 0:
                        if (HokenId > 0)
                        {
                            if (Houbetu == HokenConstant.HOUBETU_JIHI_108)
                            {
                                hokenName += "自費";
                            }
                            else if (Houbetu == HokenConstant.HOUBETU_JIHI_109)
                            {
                                hokenName += "自費レセ";
                            }
                            else
                            {
                                hokenName += "自費";
                            }
                        }
                        else
                        {
                            hokenName += "自費";
                        }
                        if (HaveKohi)
                        {
                            int nomarlKohiCount = GetKohiCount();
                            prefix = GetKohiCountName(nomarlKohiCount);
                            postfix = GetKohiName();
                        }
                        break;
                    case 11:
                    case 12:
                    case 13:
                        hokenName += "労災";
                        break;
                    case 14:
                        hokenName += "自賠責";
                        break;
                }
            }
            else
            {
                if (HokenSbtCd < 0)
                {
                    return hokenName;
                }
                string hokenSbtCd = HokenSbtCd.AsString().PadRight(3, '0');
                int firstNum = hokenSbtCd[0].AsInteger();
                int secondNum = hokenSbtCd[1].AsInteger();
                int thirNum = hokenSbtCd[2].AsInteger();
                switch (firstNum)
                {
                    case 1:
                        hokenName += "社保";
                        break;
                    case 2:
                        hokenName += "国保";
                        break;
                    case 3:
                        hokenName += "後期";
                        break;
                    case 4:
                        hokenName += "退職";
                        break;
                    case 5:
                        hokenName += "公費";
                        break;
                }

                if (secondNum > 0)
                {

                    prefix = GetKohiCountName(thirNum);


                    switch (HonkeKbn)
                    {
                        case 1:
                            prefix += "(本)";
                            break;
                        case 2:
                            prefix += "(家)";
                            break;
                        default:
                            break;
                    }
                    postfix = GetKohiName();

                }
            }

            if (!string.IsNullOrEmpty(postfix))
            {
                return hokenName + prefix + " " + postfix;
            }
            return hokenName + prefix;
        }

        private int GetKohiCount()
        {
            int result = 0;
            if (Kohi1Id > 0 && Kohi1Houbetu != HokenConstant.HOUBETU_MARUCHO)
            {
                result++;
            }
            if (Kohi2Id > 0 && Kohi1Houbetu != HokenConstant.HOUBETU_MARUCHO)
            {
                result++;
            }
            if (Kohi3Id > 0 && Kohi1Houbetu != HokenConstant.HOUBETU_MARUCHO)
            {
                result++;
            }
            if (Kohi4Id > 0 && Kohi1Houbetu != HokenConstant.HOUBETU_MARUCHO)
            {
                result++;
            }
            if (result > 0)
            {
                return result + 1;
            }
            return result;
        }

        private string GetKohiCountName(int kohicount)
        {
            if (kohicount <= 0)
            {
                return string.Empty;
            }
            if (kohicount == 1)
            {
                return "単独";
            }
            else
            {
                return kohicount + "併";
            }
        }

        private string GetKohiName()
        {
            string postfix = string.Empty;
            if (Kohi1Id > 0)
            {
                if (!string.IsNullOrEmpty(postfix))
                {
                    postfix += "-";
                }
                if (Kohi1Houbetu == HokenConstant.HOUBETU_MARUCHO)
                {
                    postfix += "マル長";
                }
                else
                {
                    postfix += Kohi1Houbetu;
                }
            }
            if (Kohi2Id > 0)
            {
                if (!string.IsNullOrEmpty(postfix))
                {
                    postfix += "-";
                }
                if (Kohi2Houbetu == HokenConstant.HOUBETU_MARUCHO)
                {
                    postfix += "マル長";
                }
                else
                {
                    postfix += Kohi2Houbetu;
                }
            }
            if (Kohi3Id > 0)
            {
                if (!string.IsNullOrEmpty(postfix))
                {
                    postfix += "-";
                }
                if (Kohi3Houbetu == HokenConstant.HOUBETU_MARUCHO)
                {
                    postfix += "マル長";
                }
                else
                {
                    postfix += Kohi3Houbetu;
                }
            }
            if (Kohi4Id > 0)
            {
                if (!string.IsNullOrEmpty(postfix))
                {
                    postfix += "+";
                }
                if (Kohi4Houbetu == HokenConstant.HOUBETU_MARUCHO)
                {
                    postfix += "マル長";
                }
                else
                {
                    postfix += Kohi4Houbetu;
                }
            }

            return postfix;
        }

        public bool NotDefaultValue { get; set; }

        public bool CheckDefaultValue()
        {
            return PtId == 0 && SinYm == 0 && HokenId == 0 && NotDefaultValue == false;
        }

        #endregion
    }
}
