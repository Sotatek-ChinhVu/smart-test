using Entity.Tenant;
using Helper.Common;

namespace Reporting.Sokatu.WelfareSeikyu.Models
{
    public class CoP44WelfareReceInfModel
    {
        public ReceInf ReceInf { get; private set; }
        public PtInf PtInf { get; private set; }
        public PtKohi PtKohi1 { get; private set; }
        public PtKohi PtKohi2 { get; private set; }
        public PtKohi PtKohi3 { get; private set; }
        public PtKohi PtKohi4 { get; private set; }

        private List<string> kohiHoubetus;

        public CoP44WelfareReceInfModel(ReceInf receInf, PtInf ptInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4, List<string> kohiHoubetus)
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

        /// <summary>
        /// 受給者番号
        /// </summary>
        /// <param name="kohiHoubetus">法別番号</param>
        /// <returns></returns>
        public string JyukyusyaNo(List<string> kohiHoubetus)
        {
            return
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu) ? PtKohi1.JyukyusyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu) ? PtKohi2.JyukyusyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu) ? PtKohi3.JyukyusyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu) ? PtKohi4.JyukyusyaNo :
                "";
        }

        /// <summary>
        /// 公費負担者番号
        /// </summary>
        /// <param name="kohiHoubetus">法別番号</param>
        /// <returns></returns>
        public string FutansyaNo(List<string> kohiHoubetus)
        {
            return
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu) ? PtKohi1.FutansyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu) ? PtKohi2.FutansyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu) ? PtKohi3.FutansyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu) ? PtKohi4.FutansyaNo :
                "";
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get => ReceInf.PtId;
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
        /// 保険種別
        ///     1:国保          
        ///     2:社保
        ///     3:後期
        /// </summary>
        public int HokenSbt(int hokenKbn)
        {
            short sbtNo = 0;
            if (hokenKbn == 1)
            {
                sbtNo = 2;
            }
            else if (hokenKbn == 2)
            {
                switch (ReceSbt.Substring(1, 1))
                {
                    case "3": sbtNo = 3; break;  //後期
                    default: sbtNo = 1; break;   //国保
                }
            }
            return sbtNo;
        }

        /// <summary>
        /// 保険者番号
        /// </summary>
        public string HokensyaNo
        {
            get => ReceInf.HokensyaNo;
        }

        /// <summary>
        /// 保険負担率
        /// </summary>
        public int HokenRate
        {
            get => ReceInf.HokenRate;
        }

        /// <summary>
        /// 患者負担率
        /// </summary>
        public int PtFutanRate
        {
            get => ReceInf.PtRate;
        }

        /// <summary>
        /// 保険実日数
        /// </summary>
        public int HokenNissu
        {
            get => ReceInf.HokenNissu ?? 0;
        }

        /// <summary>
        /// レセプト種別
        /// </summary>
        public string ReceSbt
        {
            get => ReceInf.ReceSbt;
        }

        /// <summary>
        /// 点数
        /// </summary>
        public int Tensu
        {
            get => ReceInf.Tensu;
        }

        public int PtFutan
        {
            get => ReceInf.PtFutan;
        }

        /// <summary>
        /// 保険レセ負担額
        /// </summary>
        public int? HokenReceFutan
        {
            get => ReceInf.HokenReceFutan;
        }

        public string Tokki(int tokkiNo)
        {
            switch (tokkiNo)
            {
                case 1: return CIUtil.Copy(ReceInf.Tokki1, 1, 2);
                case 2: return CIUtil.Copy(ReceInf.Tokki2, 1, 2);
                case 3: return CIUtil.Copy(ReceInf.Tokki3, 1, 2);
                case 4: return CIUtil.Copy(ReceInf.Tokki4, 1, 2);
                case 5: return CIUtil.Copy(ReceInf.Tokki5, 1, 2);
            }
            return "";

        }

        /// <summary>
        /// 公費法別
        /// </summary>
        public string KohiHoubetu(int kohiIndex)
        {
            int kisaiCnt = 0;
            for (int i = 1; i <= 4; i++)
            {
                if (KohiReceKisai(i)) kisaiCnt++;

                if (kisaiCnt == kohiIndex)
                {
                    switch (i)
                    {
                        case 1: return ReceInf.Kohi1Houbetu;
                        case 2: return ReceInf.Kohi2Houbetu;
                        case 3: return ReceInf.Kohi3Houbetu;
                        case 4: return ReceInf.Kohi4Houbetu;
                    }
                }
            }
            return "";
        }

        public bool IsSeiho
        {
            get =>
                ReceInf.Kohi1ReceKisai == 1 && ReceInf.Kohi1Houbetu == "12" ||
                ReceInf.Kohi2ReceKisai == 1 && ReceInf.Kohi2Houbetu == "12" ||
                ReceInf.Kohi3ReceKisai == 1 && ReceInf.Kohi3Houbetu == "12" ||
                ReceInf.Kohi4ReceKisai == 1 && ReceInf.Kohi4Houbetu == "12";
        }

        /// <summary>
        /// 公費レセ記載
        /// </summary>
        public bool KohiReceKisai(int kohiIndex)
        {
            switch (kohiIndex)
            {
                case 1: return ReceInf.Kohi1ReceKisai == 1;
                case 2: return ReceInf.Kohi2ReceKisai == 1;
                case 3: return ReceInf.Kohi3ReceKisai == 1;
                case 4: return ReceInf.Kohi4ReceKisai == 1;
            }
            return false;
        }

        /// <summary>
        /// 患者氏名
        /// </summary>
        public string PtName
        {
            get => PtInf.Name;
        }

        /// <summary>
        /// 患者カナ氏名
        /// </summary>
        public string PtKanaName
        {
            get => PtInf.KanaName;
        }

        /// <summary>
        /// 性別
        /// </summary>
        public int Sex
        {
            get => PtInf.Sex;
        }

        /// <summary>
        /// 生年月日
        /// </summary>
        public int BirthDay
        {
            get => PtInf.Birthday;
        }

        /// <summary>
        /// 本人家族区分
        /// </summary>
        public string Honka
        {
            get => CIUtil.Copy(ReceInf.ReceSbt, 4, 1);
        }
    }
}
