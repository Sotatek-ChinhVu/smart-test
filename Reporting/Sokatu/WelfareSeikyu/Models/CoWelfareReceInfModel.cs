using Entity.Tenant;
using Helper.Common;

namespace Reporting.Sokatu.WelfareSeikyu.Models
{
    public class CoWelfareReceInfModel
    {
        public ReceInf ReceInf { get; private set; }
        public PtInf PtInf { get; private set; }
        public PtHokenInf PtHokenInf { get; private set; }
        public PtKohi PtKohi1 { get; private set; }
        public PtKohi PtKohi2 { get; private set; }
        public PtKohi PtKohi3 { get; private set; }
        public PtKohi PtKohi4 { get; private set; }

        public CoWelfareReceInfModel(ReceInf receInf, PtInf ptInf, PtHokenInf ptHokenInf,
            PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4)
        {
            ReceInf = receInf;
            PtInf = ptInf;
            PtHokenInf = ptHokenInf;
            PtKohi1 = ptKohi1;
            PtKohi2 = ptKohi2;
            PtKohi3 = ptKohi3;
            PtKohi4 = ptKohi4;
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get => ReceInf.PtId;
        }

        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinYm
        {
            get => ReceInf.SinYm;
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
        /// 保険者番号
        /// </summary>
        public string HokensyaNo
        {
            get => ReceInf.HokensyaNo ?? string.Empty;
        }

        /// <summary>
        /// 記号
        /// </summary>
        public string Kigo
        {
            get => PtHokenInf.Kigo ?? string.Empty;
        }

        /// <summary>
        /// 番号
        /// </summary>
        public string Bango
        {
            get => PtHokenInf.Bango ?? string.Empty;
        }

        /// <summary>
        /// 公費負担者番号
        /// </summary>
        /// <param name="kohiHoubetus">法別番号</param>
        /// <returns></returns>
        public string? FutansyaNo(List<string> kohiHoubetus)
        {
            return
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? PtKohi1.FutansyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? PtKohi2.FutansyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? PtKohi3.FutansyaNo :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? PtKohi4.FutansyaNo :
                "";
        }

        public string FutansyaNo(int kohiIndex)
        {
            switch (kohiIndex)
            {
                case 1: return PtKohi1?.FutansyaNo ?? "";
                case 2: return PtKohi2?.FutansyaNo ?? "";
                case 3: return PtKohi3?.FutansyaNo ?? "";
                case 4: return PtKohi4?.FutansyaNo ?? "";
            }
            return "";
        }

        /// <summary>
        /// 受給者番号
        /// </summary>
        /// <param name="kohiHoubetus">法別番号</param>
        /// <returns></returns>
        public string? JyukyusyaNo(List<string> kohiHoubetus)
        {
            return
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? PtKohi1?.JyukyusyaNo ?? string.Empty :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? PtKohi2?.JyukyusyaNo ?? string.Empty :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? PtKohi3?.JyukyusyaNo ?? string.Empty :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? PtKohi4?.JyukyusyaNo ?? string.Empty :
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
        public string TokusyuNo(List<int> kohiHokenNos)
        {
            return
                kohiHokenNos.Contains(PtKohi1.HokenNo) ? PtKohi1.TokusyuNo ?? "" :
                kohiHokenNos.Contains(PtKohi2.HokenNo) ? PtKohi2.TokusyuNo ?? "" :
                kohiHokenNos.Contains(PtKohi3.HokenNo) ? PtKohi3.TokusyuNo ?? "" :
                kohiHokenNos.Contains(PtKohi4.HokenNo) ? PtKohi4.TokusyuNo ?? "" :
                "";
        }

        /// <summary>
        /// 特殊番号
        /// </summary>
        /// <param name="kohiHoubetus">法別番号</param>
        /// <returns></returns>
        public string TokusyuNo(List<string> kohiHoubetus)
        {
            return
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu) ? PtKohi1?.TokusyuNo ?? "" :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu) ? PtKohi2?.TokusyuNo ?? "" :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu) ? PtKohi3?.TokusyuNo ?? "" :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu) ? PtKohi4?.TokusyuNo ?? "" :
                "";
        }

        /// <summary>
        /// 公費の保険番号
        /// </summary>
        /// <param name="kohiHokenNos">保険番号</param>
        /// <returns></returns>
        public int KohiHokenNo(List<int> kohiHokenNos)
        {
            return
                kohiHokenNos.Contains(PtKohi1.HokenNo) ? PtKohi1.HokenNo :
                kohiHokenNos.Contains(PtKohi2.HokenNo) ? PtKohi2.HokenNo :
                kohiHokenNos.Contains(PtKohi3.HokenNo) ? PtKohi3.HokenNo :
                kohiHokenNos.Contains(PtKohi4.HokenNo) ? PtKohi4.HokenNo :
                0;
        }

        /// <summary>
        /// 公費の都道府県番号
        /// </summary>
        public int PrefNo(int kohiIndex)
        {
            switch (kohiIndex)
            {
                case 1: return PtKohi1?.PrefNo ?? 0;
                case 2: return PtKohi2?.PrefNo ?? 0;
                case 3: return PtKohi3?.PrefNo ?? 0;
                case 4: return PtKohi4?.PrefNo ?? 0;
            }
            return 0;
        }

        /// <summary>
        /// 保険負担率
        /// </summary>
        public int HokenRate
        {
            get => ReceInf.HokenRate;
        }

        /// <summary>
        /// 点数
        /// </summary>
        public int Tensu
        {
            get => ReceInf.Tensu;
        }

        /// <summary>
        /// 保険実日数
        /// </summary>
        public int HokenNissu
        {
            get => ReceInf.HokenNissu ?? 0;
        }

        /// <summary>
        /// 保険レセ負担額
        /// </summary>
        public int HokenReceFutan
        {
            get => ReceInf.HokenReceFutan ?? 0;
        }

        public int KohiReceTensu(int kohiIndex)
        {
            switch (kohiIndex)
            {
                case 1: return ReceInf.Kohi1ReceTensu ?? 0;
                case 2: return ReceInf.Kohi2ReceTensu ?? 0;
                case 3: return ReceInf.Kohi3ReceTensu ?? 0;
                case 4: return ReceInf.Kohi4ReceTensu ?? 0;
            }
            return 0;
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

        /// <summary>
        /// 一部負担額
        /// </summary>
        /// <param name="kohiHokenNos"></param>
        /// <returns></returns>
        public int KohiReceFutan(List<int> kohiHokenNos)
        {
            return
                kohiHokenNos.Contains(PtKohi1.HokenNo) ? ReceInf.Kohi1ReceFutan ?? 0 :
                kohiHokenNos.Contains(PtKohi2.HokenNo) ? ReceInf.Kohi2ReceFutan ?? 0 :
                kohiHokenNos.Contains(PtKohi3.HokenNo) ? ReceInf.Kohi3ReceFutan ?? 0 :
                kohiHokenNos.Contains(PtKohi4.HokenNo) ? ReceInf.Kohi4ReceFutan ?? 0 :
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

        public int KohiReceFutan(int kohiIndex)
        {
            switch (kohiIndex)
            {
                case 1: return ReceInf.Kohi1ReceFutan ?? 0;
                case 2: return ReceInf.Kohi2ReceFutan ?? 0;
                case 3: return ReceInf.Kohi3ReceFutan ?? 0;
                case 4: return ReceInf.Kohi4ReceFutan ?? 0;
            }
            return 0;
        }

        /// <summary>
        /// 公費負担額
        /// </summary>
        /// <param name="kohiHokenNos"></param>
        /// <returns></returns>
        public int KohiFutan(List<int> kohiHokenNos)
        {
            return
                kohiHokenNos.Contains(PtKohi1.HokenNo) ? ReceInf.Kohi1Futan :
                kohiHokenNos.Contains(PtKohi2.HokenNo) ? ReceInf.Kohi2Futan :
                kohiHokenNos.Contains(PtKohi3.HokenNo) ? ReceInf.Kohi3Futan :
                kohiHokenNos.Contains(PtKohi4.HokenNo) ? ReceInf.Kohi4Futan :
                0;
        }

        public int KohiFutan(List<string> kohiHoubetus)
        {
            return
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu) ? ReceInf.Kohi1Futan :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu) ? ReceInf.Kohi2Futan :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu) ? ReceInf.Kohi3Futan :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu) ? ReceInf.Kohi4Futan :
                0;
        }

        public int KohiIchibuSotogaku(int kohiIndex)
        {
            switch (kohiIndex)
            {
                case 1: return ReceInf.Kohi1IchibuSotogaku;
                case 2: return ReceInf.Kohi2IchibuSotogaku;
                case 3: return ReceInf.Kohi3IchibuSotogaku;
                case 4: return ReceInf.Kohi4IchibuSotogaku;
            }
            return 0;
        }

        public int KohiIchibuSotogaku(List<string> kohiHoubetus)
        {
            return
                kohiHoubetus.Contains(ReceInf.Kohi1Houbetu) ? ReceInf.Kohi1IchibuSotogaku :
                kohiHoubetus.Contains(ReceInf.Kohi2Houbetu) ? ReceInf.Kohi2IchibuSotogaku :
                kohiHoubetus.Contains(ReceInf.Kohi3Houbetu) ? ReceInf.Kohi3IchibuSotogaku :
                kohiHoubetus.Contains(ReceInf.Kohi4Houbetu) ? ReceInf.Kohi4IchibuSotogaku :
                0;
        }

        public int KohiIchibuFutan(int kohiIndex)
        {
            switch (kohiIndex)
            {
                case 1: return ReceInf.Kohi1IchibuFutan;
                case 2: return ReceInf.Kohi2IchibuFutan;
                case 3: return ReceInf.Kohi3IchibuFutan;
                case 4: return ReceInf.Kohi4IchibuFutan;
            }
            return 0;
        }

        public int Kohi1Futan
        {
            get => ReceInf.Kohi1Futan;
        }

        public int Kohi2Futan
        {
            get => ReceInf.Kohi2Futan;
        }

        public int Kohi3Futan
        {
            get => ReceInf.Kohi3Futan;
        }

        public int Kohi4Futan
        {
            get => ReceInf.Kohi4Futan;
        }

        public int KohiFutan(int kohiIndex)
        {
            switch (kohiIndex)
            {
                case 1: return ReceInf.Kohi1Futan;
                case 2: return ReceInf.Kohi2Futan;
                case 3: return ReceInf.Kohi3Futan;
                case 4: return ReceInf.Kohi4Futan;
            }
            return 0;
        }

        /// <summary>
        /// 保険分一部負担額
        /// </summary>
        public int HokenIchibuFutan
        {
            get => ReceInf.HokenIchibuFutan;
        }

        public int IchibuFutan
        {
            get => ReceInf.IchibuFutan;
        }

        /// <summary>
        /// 患者負担額
        /// </summary>
        public int PtFutan
        {
            get => ReceInf.PtFutan;
        }

        /// <summary>
        /// マル長
        /// </summary>
        public bool IsChoki
        {
            get => TokkiContains("02") || TokkiContains("16");

        }

        /// <summary>
        /// マル長（レセ記載なし・保険あり）
        /// </summary>
        public bool IsChokiHoken
        {
            get =>
                ReceInf.Kohi1Houbetu == "102" ||
                ReceInf.Kohi2Houbetu == "102" ||
                ReceInf.Kohi3Houbetu == "102" ||
                ReceInf.Kohi4Houbetu == "102" ||
                ReceInf.ChokiKbn >= 1;
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

        public string TokkiContainsName(string tokkiCd)
        {
            return
                CIUtil.Copy(ReceInf.Tokki1, 1, 2) == tokkiCd ? ReceInf.Tokki1 :
                CIUtil.Copy(ReceInf.Tokki2, 1, 2) == tokkiCd ? ReceInf.Tokki2 :
                CIUtil.Copy(ReceInf.Tokki3, 1, 2) == tokkiCd ? ReceInf.Tokki3 :
                CIUtil.Copy(ReceInf.Tokki4, 1, 2) == tokkiCd ? ReceInf.Tokki4 :
                CIUtil.Copy(ReceInf.Tokki5, 1, 2) == tokkiCd ? ReceInf.Tokki5 :
                string.Empty;
        }

        /// <summary>
        /// 請求区分
        ///     1:月遅れ 2:返戻 3:オンライン返戻
        /// </summary>
        public int SeikyuKbn
        {
            get => ReceInf.SeikyuKbn;
        }

        /// <summary>
        /// 公１レセ記載
        ///     0:記載しない 1:記載する
        /// </summary>
        public int Kohi1ReceKisai
        {
            get => ReceInf.Kohi1ReceKisai;
        }

        /// <summary>
        /// 公２レセ記載
        ///     0:記載しない 1:記載する
        /// </summary>
        public int Kohi2ReceKisai
        {
            get => ReceInf.Kohi2ReceKisai;
        }

        /// <summary>
        /// 公３レセ記載
        ///     0:記載しない 1:記載する
        /// </summary>
        public int Kohi3ReceKisai
        {
            get => ReceInf.Kohi3ReceKisai;
        }

        /// <summary>
        /// 公４レセ記載
        ///     0:記載しない 1:記載する
        /// </summary>
        public int Kohi4ReceKisai
        {
            get => ReceInf.Kohi4ReceKisai;
        }

        public int KohiReceKisai(int kohiIndex)
        {
            switch (kohiIndex)
            {
                case 1: return ReceInf.Kohi1ReceKisai;
                case 2: return ReceInf.Kohi2ReceKisai;
                case 3: return ReceInf.Kohi3ReceKisai;
                case 4: return ReceInf.Kohi4ReceKisai;
            }
            return 0;
        }

        /// <summary>
        /// 高額療養費区分
        /// </summary>
        public int KogakuKbn
        {
            get => ReceInf.KogakuKbn;
        }

        /// <summary>
        /// 高額療養費超過区分
        /// </summary>
        public int KogakuOverKbn
        {
            get => ReceInf.KogakuOverKbn;
        }

        /// <summary>
        /// 限度額特例フラグ
        /// </summary>
        public bool IsTokurei
        {
            get => ReceInf.IsTokurei == 1;
        }

        /// <summary>
        /// レセプト種別
        /// </summary>
        public string ReceSbt
        {
            get => ReceInf.ReceSbt ?? string.Empty;
        }

        /// <summary>
        /// 前期一般（指定公費あり）
        /// 
        /// </summary>
        public bool IsSiteiKohi
        {
            get =>
                ReceInf.HokenRate == 10 &&
                ReceInf.ReceSbt.Substring(1, 1) != "3" && ReceInf.ReceSbt.Substring(3, 1) == "8";
        }

        /// <summary>
        /// 前期高齢
        /// </summary>
        public bool IsElderZenki
        {
            get => ReceInf.ReceSbt.Substring(1, 1) != "3" &&
                (ReceInf.ReceSbt.Substring(3, 1) == "0" || ReceInf.ReceSbt.Substring(3, 1) == "8");
        }

        /// <summary>
        /// 高齢者
        /// 
        /// </summary>
        public bool IsElder
        {
            get => ReceInf.ReceSbt.Substring(3, 1) == "0" || ReceInf.ReceSbt.Substring(3, 1) == "8";
        }

        /// <summary>
        /// 高齢受給者７割
        /// </summary>
        public bool IsElderJyoi
        {
            get => ReceInf.ReceSbt.Substring(3, 1) == "0";
        }

        /// <summary>
        /// 法別番号
        /// </summary>
        public string Houbetu
        {
            get => ReceInf.Houbetu;
        }

        /// <summary>
        /// 公１法別
        /// </summary>
        public string Kohi1Houbetu
        {
            get => ReceInf.Kohi1Houbetu;
        }

        /// <summary>
        /// 公２法別
        /// </summary>
        public string Kohi2Houbetu
        {
            get => ReceInf.Kohi2Houbetu;
        }

        /// <summary>
        /// 公３法別
        /// </summary>
        public string Kohi3Houbetu
        {
            get => ReceInf.Kohi3Houbetu;
        }

        /// <summary>
        /// 公４法別
        /// </summary>
        public string Kohi4Houbetu
        {
            get => ReceInf.Kohi4Houbetu;
        }

        public string KohiHoubetu(int kohiIndex)
        {
            switch (kohiIndex)
            {
                case 1: return ReceInf.Kohi1Houbetu;
                case 2: return ReceInf.Kohi2Houbetu;
                case 3: return ReceInf.Kohi3Houbetu;
                case 4: return ReceInf.Kohi4Houbetu;
            }
            return "";
        }

        public bool IsKohiHoubetu(string houbetu)
        {
            return
                (ReceInf.Kohi1Houbetu == houbetu && ReceInf.Kohi1ReceKisai == 1) ||
                (ReceInf.Kohi2Houbetu == houbetu && ReceInf.Kohi2ReceKisai == 1) ||
                (ReceInf.Kohi3Houbetu == houbetu && ReceInf.Kohi3ReceKisai == 1) ||
                (ReceInf.Kohi4Houbetu == houbetu && ReceInf.Kohi4ReceKisai == 1);
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
        public string KanaName
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
    }
}
