using Entity.Tenant;
using Helper.Common;

namespace Reporting.Sokatu.WelfareSeikyu.Models
{
    public class CoP40WelfareReceInfModel
    {
        public ReceInf ReceInf { get; private set; }
        public PtKohi PtKohi1 { get; private set; }
        public PtKohi PtKohi2 { get; private set; }
        public PtKohi PtKohi3 { get; private set; }
        public PtKohi PtKohi4 { get; private set; }

        private List<string> kohiHoubetus;

        public CoP40WelfareReceInfModel(ReceInf receInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4, List<string> kohiHoubetus)
        {
            ReceInf = receInf;
            PtKohi1 = ptKohi1;
            PtKohi2 = ptKohi2;
            PtKohi3 = ptKohi3;
            PtKohi4 = ptKohi4;
            this.kohiHoubetus = kohiHoubetus;
        }

        /// <summary>
        /// 公費負担者番号(3..7桁目)
        /// </summary>
        /// <param name="kohiHoubetus">法別番号</param>
        /// <returns></returns>
        public string FutansyaNo()
        {
            return
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? CIUtil.Copy(PtKohi1.FutansyaNo ?? string.Empty, 3, 5) :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? CIUtil.Copy(PtKohi2.FutansyaNo ?? string.Empty, 3, 5) :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? CIUtil.Copy(PtKohi3.FutansyaNo ?? string.Empty, 3, 5) :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? CIUtil.Copy(PtKohi4.FutansyaNo ?? string.Empty, 3, 5) :
                "";
        }

        /// <summary>
        /// 保険負担率
        /// </summary>
        public int HokenRate
        {
            get => ReceInf.HokenRate;
        }


        /// <summary>
        /// 公費実日数
        /// </summary>
        public int KohiNissu(string kohiHoubetu)
        {
            return
                ReceInf.Kohi1Houbetu == kohiHoubetu ? ReceInf.Kohi1Nissu ?? 0 :
                ReceInf.Kohi2Houbetu == kohiHoubetu ? ReceInf.Kohi2Nissu ?? 0 :
                ReceInf.Kohi3Houbetu == kohiHoubetu ? ReceInf.Kohi3Nissu ?? 0 :
                ReceInf.Kohi4Houbetu == kohiHoubetu ? ReceInf.Kohi4Nissu ?? 0 :
                0;
        }

        /// <summary>
        /// 公費分点数
        /// </summary>
        public int KohiTensu(string kohiHoubetu)
        {
            return
                ReceInf.Kohi1Houbetu == kohiHoubetu ? ReceInf.Kohi1Tensu :
                ReceInf.Kohi2Houbetu == kohiHoubetu ? ReceInf.Kohi2Tensu :
                ReceInf.Kohi3Houbetu == kohiHoubetu ? ReceInf.Kohi3Tensu :
                ReceInf.Kohi4Houbetu == kohiHoubetu ? ReceInf.Kohi4Tensu :
                0;
        }

        /// <summary>
        /// 公費レセ記載
        /// </summary>
        private int kohiReceKisai(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1: return ReceInf.Kohi1ReceKisai;
                case 2: return ReceInf.Kohi2ReceKisai;
                case 3: return ReceInf.Kohi3ReceKisai;
                case 4: return ReceInf.Kohi4ReceKisai;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費分点数
        /// </summary>
        private int kohiTensu(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1: return ReceInf.Kohi1Tensu;
                case 2: return ReceInf.Kohi2Tensu;
                case 3: return ReceInf.Kohi3Tensu;
                case 4: return ReceInf.Kohi4Tensu;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費分一部負担
        /// </summary>
        private int kohiIchibuFutan(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1: return ReceInf.Kohi1IchibuSotogaku;
                case 2: return ReceInf.Kohi2IchibuSotogaku;
                case 3: return ReceInf.Kohi3IchibuSotogaku;
                case 4: return ReceInf.Kohi4IchibuSotogaku;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 一部負担金欄
        /// </summary>
        public int KohiIchibuFutan(string kohiHoubetu)
        {
            int kohiNo =
                ReceInf.Kohi1Houbetu == kohiHoubetu ? 1 :
                ReceInf.Kohi2Houbetu == kohiHoubetu ? 2 :
                ReceInf.Kohi3Houbetu == kohiHoubetu ? 3 :
                ReceInf.Kohi4Houbetu == kohiHoubetu ? 4 : 0;

            if (kohiNo <= 1) return 0;

            int highKohiNo = 0;
            for (int i = kohiNo - 1; i >= 1; i--)
            {
                if (kohiReceKisai(i) == 1)
                {
                    highKohiNo = i;
                    break;
                }
            }

            //上位公費がある場合のみ記載する
            if (highKohiNo == 0) return 0;

            return kohiIchibuFutan(highKohiNo);
        }

        /// <summary>
        /// 子・障・親 医療費給付外の額 欄
        /// </summary>
        public int KohiKyufugai(string kohiHoubetu)
        {
            int kohiNo =
                ReceInf.Kohi1Houbetu == kohiHoubetu ? 1 :
                ReceInf.Kohi2Houbetu == kohiHoubetu ? 2 :
                ReceInf.Kohi3Houbetu == kohiHoubetu ? 3 :
                ReceInf.Kohi4Houbetu == kohiHoubetu ? 4 : 0;

            if (kohiNo == 0) return 0;

            int retFutan = kohiIchibuFutan(kohiNo);

            return retFutan >= 0 ? retFutan : 0;
        }

        /// <summary>
        /// 公費の有無
        /// </summary>
        public bool IsKohiHoubetu(string houbetu)
        {
            return
                (ReceInf.Kohi1Houbetu == houbetu && ReceInf.Kohi1ReceKisai == 0) ||
                (ReceInf.Kohi2Houbetu == houbetu && ReceInf.Kohi2ReceKisai == 0) ||
                (ReceInf.Kohi3Houbetu == houbetu && ReceInf.Kohi3ReceKisai == 0) ||
                (ReceInf.Kohi4Houbetu == houbetu && ReceInf.Kohi4ReceKisai == 0);
        }
    }
}
