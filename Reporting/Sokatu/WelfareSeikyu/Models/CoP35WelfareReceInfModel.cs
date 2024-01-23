using Entity.Tenant;
using Helper.Common;
using Helper.Extension;

namespace Reporting.Sokatu.WelfareSeikyu.Models
{
    public class CoP35WelfareReceInfModel
    {
        public ReceInf ReceInf { get; private set; }
        public PtInf PtInf { get; private set; }
        public PtKohi PtKohi1 { get; private set; }
        public PtKohi PtKohi2 { get; private set; }
        public PtKohi PtKohi3 { get; private set; }
        public PtKohi PtKohi4 { get; private set; }

        private List<string> kohiHoubetus;

        public CoP35WelfareReceInfModel(ReceInf receInf, PtInf ptInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4, List<string> kohiHoubetus)
        {
            ReceInf = receInf;
            PtInf = ptInf;
            PtKohi1 = ptKohi1;
            PtKohi2 = ptKohi2;
            PtKohi3 = ptKohi3;
            PtKohi4 = ptKohi4;
            this.kohiHoubetus = kohiHoubetus;
        }

        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinYm
        {
            get => ReceInf.SinYm;
        }

        public string CityCode
        {
            get
            {
                string ret = "";

                if (string.IsNullOrEmpty(WelfareFutansyaNo) == false && WelfareFutansyaNo.Length == 8)
                {
                    ret = CIUtil.Copy(WelfareFutansyaNo, 6, 3);
                }

                return ret;
            }
        }

        public string CityName
        {
            get
            {
                switch (CityCode)
                {
                    case "019": return "下関市";
                    case "027": return "宇部市";
                    case "035": return "山口市";
                    case "068": return "防府市";
                    case "076": return "下松市";
                    case "084": return "岩国市";
                    case "092": return "山陽小野田市";
                    case "100": return "光市";
                    case "126": return "柳井市";
                    case "134": return "美祢市";
                    case "159": return "周防大島町";
                    case "191": return "和木町";
                    case "282": return "上関町";
                    case "308": return "田布施町";
                    case "316": return "平生町";
                    case "522": return "阿武町";
                    case "597": return "周南町";
                    case "605": return "萩市";
                    case "613": return "長門市";
                }

                return "";
            }
        }

        /// <summary>
        /// 公費負担額
        /// </summary>
        public int KohiFutan
        {
            get =>
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu) ? ReceInf.Kohi1Futan :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu) ? ReceInf.Kohi2Futan :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu) ? ReceInf.Kohi3Futan :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu) ? ReceInf.Kohi4Futan :
                0;
        }

        public string WelfareJyukyusyaNo
        {
            get =>
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu) ? PtKohi1.JyukyusyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu) ? PtKohi2.JyukyusyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu) ? PtKohi3.JyukyusyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu) ? PtKohi4.JyukyusyaNo :
                "";
        }

        public string WelfareFutansyaNo
        {
            get =>
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu) ? PtKohi1.FutansyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu) ? PtKohi2.FutansyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu) ? PtKohi3.FutansyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu) ? PtKohi4.FutansyaNo :
                "";
        }

        public int KohiReceTensu(List<string> kohiHoubetus)
        {
            return
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu) ? ReceInf.Kohi1ReceTensu ?? 0 :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu) ? ReceInf.Kohi2ReceTensu ?? 0 :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu) ? ReceInf.Kohi3ReceTensu ?? 0 :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu) ? ReceInf.Kohi4ReceTensu ?? 0 :
                0;
        }

        public int KohiReceFutan(List<string> kohiHoubetus)
        {
            return
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu) ? ReceInf.Kohi1ReceFutan ?? 0 :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu) ? ReceInf.Kohi2ReceFutan ?? 0 :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu) ? ReceInf.Kohi3ReceFutan ?? 0 :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu) ? ReceInf.Kohi4ReceFutan ?? 0 :
                0;
        }

        /// <summary>
        /// 保険者番号
        /// </summary>
        public string HokensyaNo
        {
            get => ReceInf.HokensyaNo;
        }

        /// <summary>
        /// レセプト種別
        /// </summary>
        public string ReceSbt
        {
            get => ReceInf.ReceSbt;
        }

        /// <summary>
        /// 入外
        /// </summary>
        public int GetNyugai()
        {
            switch (ReceSbt.Substring(3, 1))
            {
                case "8": return 8;  //高齢受給者（２割負担）外来
                case "0": return 0;  //高齢受給者（３割負担）外来
                case "4": return 4;  //未就学者 外来
                default: return 2;   //本人・家族 外来
            }
        }

        public bool TokkiContains(string tokkiCd)
        {
            return
                CIUtil.Copy(ReceInf.Tokki, 1, 2) == tokkiCd ||
                CIUtil.Copy(ReceInf.Tokki, 3, 2) == tokkiCd ||
                CIUtil.Copy(ReceInf.Tokki, 5, 2) == tokkiCd ||
                CIUtil.Copy(ReceInf.Tokki, 7, 2) == tokkiCd ||
                CIUtil.Copy(ReceInf.Tokki, 9, 2) == tokkiCd;
        }

        /// <summary>
        /// 患者氏名
        /// </summary>
        public string PtName
        {
            get => PtInf.Name;
        }

        /// <summary>
        /// 生年月日
        /// </summary>
        public int Birthday
        {
            get => PtInf.Birthday;
        }

        /// <summary>
        /// 保険実日数
        /// </summary>
        public int HokenNissu
        {
            get => ReceInf.HokenNissu ?? 0;
        }

        /// <summary>
        /// 患者番号
        /// </summary>
        public long PtNum
        {
            get => PtInf.PtNum.AsLong();
        }

        /// <summary>
        /// 性別
        /// </summary>
        public int Sex
        {
            get => PtInf.Sex;
        }
    }
}
