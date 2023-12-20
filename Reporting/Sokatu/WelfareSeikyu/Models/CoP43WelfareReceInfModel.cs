using Entity.Tenant;

namespace Reporting.Sokatu.WelfareSeikyu.Models
{
    public class CoP43WelfareReceInfModel
    {
        public ReceInf ReceInf { get; private set; }
        public PtInf PtInf { get; private set; }
        public PtKohi PtKohi1 { get; private set; }
        public PtKohi PtKohi2 { get; private set; }
        public PtKohi PtKohi3 { get; private set; }
        public PtKohi PtKohi4 { get; private set; }

        private List<string> kohiHoubetus = new();

        private List<int> kohiHokenNos = new();

        private readonly List<CityKohi> NotKikuchi = new List<CityKohi>
        {
            new CityKohi() { CityName = "天草市", HokenNo = 141, HokenEdaNo = 5},
            new CityKohi() { CityName = "苓北町", HokenNo = 141, HokenEdaNo = 6}
        };

        private struct CityKohi
        {
            public int HokenNo;
            public int HokenEdaNo;
            public string CityName;
        }

        public CoP43WelfareReceInfModel(ReceInf receInf, PtInf ptInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4, List<string> kohiHoubetus)
        {
            ReceInf = receInf;
            PtInf = ptInf;
            PtKohi1 = ptKohi1;
            PtKohi2 = ptKohi2;
            PtKohi3 = ptKohi3;
            PtKohi4 = ptKohi4;
            this.kohiHoubetus = kohiHoubetus;
        }

        public CoP43WelfareReceInfModel(ReceInf receInf, PtInf ptInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4, List<int> kohiHokenNos)
        {
            ReceInf = receInf;
            PtInf = ptInf;
            PtKohi1 = ptKohi1;
            PtKohi2 = ptKohi2;
            PtKohi3 = ptKohi3;
            PtKohi4 = ptKohi4;
            this.kohiHokenNos = kohiHokenNos;
        }

        /// <summary>
        /// 菊池市こども医療費の対象
        /// </summary>
        public bool IsKikuchi41
        {
            get
            {
                return
                  kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? (string.IsNullOrEmpty(PtKohi1.TokusyuNo) && !NotKikuchi.Exists(x => x.HokenNo == PtKohi1.HokenNo && x.HokenEdaNo == PtKohi1.HokenEdaNo)) :
                  kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? (string.IsNullOrEmpty(PtKohi2.TokusyuNo) && !NotKikuchi.Exists(x => x.HokenNo == PtKohi2.HokenNo && x.HokenEdaNo == PtKohi2.HokenEdaNo)) :
                  kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? (string.IsNullOrEmpty(PtKohi3.TokusyuNo) && !NotKikuchi.Exists(x => x.HokenNo == PtKohi3.HokenNo && x.HokenEdaNo == PtKohi3.HokenEdaNo)) :
                  kohiHoubetus.Contains(ReceInf.Kohi4Houbetu?? string.Empty) ? (string.IsNullOrEmpty(PtKohi4.TokusyuNo) && !NotKikuchi.Exists(x => x.HokenNo == PtKohi4.HokenNo && x.HokenEdaNo == PtKohi4.HokenEdaNo)) :
                  false;
            }
        }

        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinYm
        {
            get => ReceInf.SinYm;
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get => ReceInf.PtId;
        }

        /// <summary>
        /// 患者番号
        /// </summary>
        public long PtNum
        {
            get => PtInf.PtNum;
        }

        /// <summary>
        /// 主保険保険ID
        /// </summary>
        public long HokenId
        {
            get => ReceInf.HokenId;
        }

        /// <summary>
        /// 受給者番号
        /// </summary>
        /// <param name="kohiHoubetus">法別番号</param>
        /// <returns></returns>
        public string? JyukyusyaNo(List<string> kohiHoubetus)
        {
            return
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? PtKohi1.JyukyusyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? PtKohi2.JyukyusyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? PtKohi3.JyukyusyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? PtKohi4.JyukyusyaNo :
                "";
        }

        /// <summary>
        /// 受給者番号
        /// </summary>
        /// <param name="kohiHokenNos">保険番号</param>
        /// <returns></returns>
        public string? JyukyusyaNo(List<int> kohiHokenNos)
        {
            return
                kohiHokenNos.Contains(PtKohi1.HokenNo) ? PtKohi1.JyukyusyaNo :
                kohiHokenNos.Contains(PtKohi2.HokenNo) ? PtKohi2.JyukyusyaNo :
                kohiHokenNos.Contains(PtKohi3.HokenNo) ? PtKohi3.JyukyusyaNo :
                kohiHokenNos.Contains(PtKohi4.HokenNo) ? PtKohi4.JyukyusyaNo :
                "";
        }

        public string JyukyusyaNo(int kohiIndex)
        {
            switch (kohiIndex)
            {
                case 1: return PtKohi1?.JyukyusyaNo ?? "";
                case 2: return PtKohi2?.JyukyusyaNo ?? "";
                case 3: return PtKohi3?.JyukyusyaNo ?? "";
                case 4: return PtKohi4?.JyukyusyaNo ?? "";
            }
            return "";
        }

        /// <summary>
        /// 特殊番号
        /// </summary>
        /// <param name="kohiHokenNos">保険番号</param>
        /// <returns></returns>
        public string? TokusyuNo(List<int> kohiHokenNos)
        {
            return
                kohiHokenNos.Contains(PtKohi1.HokenNo) ? PtKohi1.TokusyuNo :
                kohiHokenNos.Contains(PtKohi2.HokenNo) ? PtKohi2.TokusyuNo :
                kohiHokenNos.Contains(PtKohi3.HokenNo) ? PtKohi3.TokusyuNo :
                kohiHokenNos.Contains(PtKohi4.HokenNo) ? PtKohi4.TokusyuNo :
                "";
        }

        public string? WelfareTokusyuNo
        {
            get =>
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? PtKohi1.TokusyuNo :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? PtKohi2.TokusyuNo :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? PtKohi3.TokusyuNo :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? PtKohi4.TokusyuNo :
                "";
        }
        public string? WelfareJyukyusyaNo
        {
            get =>
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? PtKohi1.JyukyusyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? PtKohi2.JyukyusyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? PtKohi3.JyukyusyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? PtKohi4.JyukyusyaNo :
                "";
        }

        /// <summary>
        /// 保険者番号
        /// </summary>
        public string HokensyaNo
        {
            get => ReceInf.HokensyaNo ?? string.Empty;
        }

        /// <summary>
        /// 公費保険番号枝番
        /// </summary>
        /// <param name="kohiHokenNos">保険番号</param>
        /// <returns></returns>
        public int KohiHokenEdaNo(List<int> kohiHokenNos)
        {
            return
                kohiHokenNos.Contains(PtKohi1.HokenNo) ? PtKohi1.HokenEdaNo :
                kohiHokenNos.Contains(PtKohi2.HokenNo) ? PtKohi2.HokenEdaNo :
                kohiHokenNos.Contains(PtKohi3.HokenNo) ? PtKohi3.HokenEdaNo :
                kohiHokenNos.Contains(PtKohi4.HokenNo) ? PtKohi4.HokenEdaNo :
                0;
        }

        /// <summary>
        /// 点数
        /// </summary>
        public int Tensu
        {
            get => ReceInf.Tensu;
        }

        /// <summary>
        /// 保険区分
        ///     1:社保          
        ///     2:国保          
        /// </summary>
        public int HokenKbn
        {
            get => ReceInf.HokenKbn;
        }

        /// <summary>
        /// 保険負担率
        /// </summary>
        public int HokenRate
        {
            get => ReceInf.HokenRate;
        }

        /// <summary>
        /// 保険分一部負担額
        /// </summary>
        public int HokenIchibuFutan
        {
            get => ReceInf.HokenIchibuFutan;
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

        public int KohiReceTensu(List<string> kohiHoubetus)
        {
            return
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? ReceInf.Kohi1ReceTensu ?? 0 :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? ReceInf.Kohi2ReceTensu ?? 0 :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? ReceInf.Kohi3ReceTensu ?? 0 :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? ReceInf.Kohi4ReceTensu ?? 0 :
                0;
        }

        /// <summary>
        /// 患者氏名
        /// </summary>
        public string PtName
        {
            get => PtInf.Name ?? string.Empty;
        }

        /// <summary>
        /// 生年月日
        /// </summary>
        public int BirthDay
        {
            get => PtInf.Birthday;
        }

        /// <summary>
        /// 患者住所１
        /// </summary>
        public string HomeAddress1
        {
            get => PtInf.HomeAddress1 ?? string.Empty;
        }

        /// <summary>
        /// 患者住所２
        /// </summary>
        public string HomeAddress2
        {
            get => PtInf.HomeAddress2 ?? string.Empty;
        }
    }
}
