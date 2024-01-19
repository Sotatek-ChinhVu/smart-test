using Entity.Tenant;
using Helper.Common;
using Helper.Extension;

namespace Reporting.Sokatu.WelfareSeikyu.Models
{
    public class CoP43WelfareReceInfModel2
    {
        public ReceInf ReceInf { get; private set; }
        public PtInf PtInf { get; private set; }
        public PtKohi PtKohi1 { get; private set; }
        public PtKohi PtKohi2 { get; private set; }
        public PtKohi PtKohi3 { get; private set; }
        public PtKohi PtKohi4 { get; private set; }

        private List<CityKohi> kohiHokens;

        public struct CityKohi
        {
            public int HokenNo;
            public int HokenEdaNo;
        }

        public CoP43WelfareReceInfModel2(ReceInf receInf, PtInf ptInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4, List<CityKohi> kohiHokens)
        {
            ReceInf = receInf;
            PtInf = ptInf;
            PtKohi1 = ptKohi1;
            PtKohi2 = ptKohi2;
            PtKohi3 = ptKohi3;
            PtKohi4 = ptKohi4;
            this.kohiHokens = kohiHokens;
        }

        /// <summary>
        /// 患者番号
        /// </summary>
        public long PtNum
        {
            get => PtInf.PtNum.AsLong();
        }

        /// <summary>
        /// 患者氏名
        /// </summary>
        public string PtName
        {
            get => PtInf.Name;
        }

        public string PtNameJis
        {
            get
            {
                string wrkName = string.Empty;
                string errWord = CIUtil.Chk_JISKj(PtInf.Name, out wrkName);
                if (string.IsNullOrEmpty(errWord))
                {
                    return PtInf.Name;
                }
                else
                {
                    return PtInf.KanaName;
                }
            }
        }

        /// <summary>
        /// 生年月日
        /// </summary>
        public string BirthDayW
        {
            get
            {
                var wrkDate = CIUtil.SDateToShowSWDate(PtInf.Birthday);
                int pos1 = wrkDate.IndexOf("(") + 1;
                int pos2 = wrkDate.IndexOf(")");

                if (pos1 >= 0 && pos2 >= 0)
                {
                    wrkDate = wrkDate.Substring(pos1, pos2 - pos1) + wrkDate.Substring(pos2 + 1);
                    wrkDate = wrkDate.Replace("/", ".");
                }

                return wrkDate;
            }
        }

        /// <summary>
        /// 福祉公費有無
        /// </summary>
        public bool IsWelfare
        {
            get =>
                PtKohi1 != null && kohiHokens.Exists(x => x.HokenNo == PtKohi1.HokenNo && x.HokenEdaNo == PtKohi1.HokenEdaNo) ||
                PtKohi2 != null && kohiHokens.Exists(x => x.HokenNo == PtKohi2.HokenNo && x.HokenEdaNo == PtKohi2.HokenEdaNo) ||
                PtKohi3 != null && kohiHokens.Exists(x => x.HokenNo == PtKohi3.HokenNo && x.HokenEdaNo == PtKohi3.HokenEdaNo) ||
                PtKohi4 != null && kohiHokens.Exists(x => x.HokenNo == PtKohi4.HokenNo && x.HokenEdaNo == PtKohi4.HokenEdaNo);
        }

        /// <summary>
        /// 受給者証番号
        /// </summary>
        public string JyukyusyaNo
        {
            get =>
                kohiHokens.Exists(x => x.HokenNo == PtKohi1.HokenNo && x.HokenEdaNo == PtKohi1.HokenEdaNo) ? PtKohi1.JyukyusyaNo :
                kohiHokens.Exists(x => x.HokenNo == PtKohi2.HokenNo && x.HokenEdaNo == PtKohi2.HokenEdaNo) ? PtKohi2.JyukyusyaNo :
                kohiHokens.Exists(x => x.HokenNo == PtKohi3.HokenNo && x.HokenEdaNo == PtKohi3.HokenEdaNo) ? PtKohi3.JyukyusyaNo :
                kohiHokens.Exists(x => x.HokenNo == PtKohi4.HokenNo && x.HokenEdaNo == PtKohi4.HokenEdaNo) ? PtKohi4.JyukyusyaNo :
                string.Empty;
        }

        /// <summary>
        /// 公費実日数
        /// </summary>
        public int KohiNissu
        {
            get =>
                kohiHokens.Exists(x => x.HokenNo == PtKohi1.HokenNo && x.HokenEdaNo == PtKohi1.HokenEdaNo) ? ReceInf.Kohi1Nissu ?? 0 :
                kohiHokens.Exists(x => x.HokenNo == PtKohi2.HokenNo && x.HokenEdaNo == PtKohi2.HokenEdaNo) ? ReceInf.Kohi2Nissu ?? 0 :
                kohiHokens.Exists(x => x.HokenNo == PtKohi3.HokenNo && x.HokenEdaNo == PtKohi3.HokenEdaNo) ? ReceInf.Kohi3Nissu ?? 0 :
                kohiHokens.Exists(x => x.HokenNo == PtKohi4.HokenNo && x.HokenEdaNo == PtKohi4.HokenEdaNo) ? ReceInf.Kohi4Nissu ?? 0 :
                0;
        }

        /// <summary>
        /// 公費点数
        /// </summary>
        public int KohiTensu
        {
            get =>
                kohiHokens.Exists(x => x.HokenNo == PtKohi1.HokenNo && x.HokenEdaNo == PtKohi1.HokenEdaNo) ? ReceInf.Kohi1Tensu :
                kohiHokens.Exists(x => x.HokenNo == PtKohi2.HokenNo && x.HokenEdaNo == PtKohi2.HokenEdaNo) ? ReceInf.Kohi2Tensu :
                kohiHokens.Exists(x => x.HokenNo == PtKohi3.HokenNo && x.HokenEdaNo == PtKohi3.HokenEdaNo) ? ReceInf.Kohi3Tensu :
                kohiHokens.Exists(x => x.HokenNo == PtKohi4.HokenNo && x.HokenEdaNo == PtKohi4.HokenEdaNo) ? ReceInf.Kohi4Tensu :
                0;
        }

        /// <summary>
        /// 一部負担額（10円単位に四捨五入）
        /// </summary>
        public int IchibuFutan
        {
            get
            {
                var wrkFutan =
                    kohiHokens.Exists(x => x.HokenNo == PtKohi1.HokenNo && x.HokenEdaNo == PtKohi1.HokenEdaNo) ? ReceInf.Kohi1Futan + ReceInf.Kohi1IchibuSotogaku :
                    kohiHokens.Exists(x => x.HokenNo == PtKohi2.HokenNo && x.HokenEdaNo == PtKohi2.HokenEdaNo) ? ReceInf.Kohi2Futan + ReceInf.Kohi2IchibuSotogaku :
                    kohiHokens.Exists(x => x.HokenNo == PtKohi3.HokenNo && x.HokenEdaNo == PtKohi3.HokenEdaNo) ? ReceInf.Kohi3Futan + ReceInf.Kohi3IchibuSotogaku :
                    kohiHokens.Exists(x => x.HokenNo == PtKohi4.HokenNo && x.HokenEdaNo == PtKohi4.HokenEdaNo) ? ReceInf.Kohi4Futan + ReceInf.Kohi4IchibuSotogaku :
                    0;

                return CIUtil.RoundInt(wrkFutan, 1);
            }
        }

        /// <summary>
        /// 自己負担額
        /// </summary>
        public int KohiPtFutan
        {
            get
            {
                var wrkFutan =
                    kohiHokens.Exists(x => x.HokenNo == PtKohi1.HokenNo && x.HokenEdaNo == PtKohi1.HokenEdaNo) ? ReceInf.Kohi1IchibuFutan :
                    kohiHokens.Exists(x => x.HokenNo == PtKohi2.HokenNo && x.HokenEdaNo == PtKohi2.HokenEdaNo) ? ReceInf.Kohi2IchibuFutan :
                    kohiHokens.Exists(x => x.HokenNo == PtKohi3.HokenNo && x.HokenEdaNo == PtKohi3.HokenEdaNo) ? ReceInf.Kohi3IchibuFutan :
                    kohiHokens.Exists(x => x.HokenNo == PtKohi4.HokenNo && x.HokenEdaNo == PtKohi4.HokenEdaNo) ? ReceInf.Kohi4IchibuFutan :
                    0;

                return CIUtil.RoundInt(wrkFutan, 1);
            }
        }

        /// <summary>
        /// 保険負担率（割合）
        /// </summary>
        public string HokenRate
        {
            get => ReceInf.HokenRate / 10 + "割";
        }

        /// <summary>
        /// 保険区分
        ///     0:自費
        ///     1:社保          
        ///     2:国保          
        ///     11:労災(短期給付)          
        ///     12:労災(傷病年金)          
        ///     13:アフターケア          
        ///     14:自賠責          
        /// </summary>
        public int HokenKbn
        {
            get => ReceInf.HokenKbn;
        }
    }
}
