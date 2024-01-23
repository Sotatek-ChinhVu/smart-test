using Entity.Tenant;
using Helper.Common;
using Helper.Extension;

namespace Reporting.Sokatu.WelfareSeikyu.Models;

public class CoP22WelfareReceInfModel
{
    public ReceInf ReceInf { get; private set; }
    public PtInf PtInf { get; private set; }
    public PtKohi PtKohi1 { get; private set; }
    public PtKohi PtKohi2 { get; private set; }
    public PtKohi PtKohi3 { get; private set; }
    public PtKohi PtKohi4 { get; private set; }

    private List<string> kohiHoubetus;

    public CoP22WelfareReceInfModel(ReceInf receInf, PtInf ptInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4, List<string> kohiHoubetus)
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
                ret = CIUtil.Copy(WelfareFutansyaNo, 5, 3);
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
                case "001": return "静岡市";
                case "002": return "浜松市";
                case "003": return "沼津市";
                case "005": return "熱海市";
                case "006": return "三島市";
                case "007": return "富士宮市";
                case "008": return "伊東市";
                case "009": return "島田市";
                case "010": return "富士市";
                case "011": return "磐田市";
                case "012": return "焼津市";
                case "013": return "掛川市";
                case "014": return "藤枝市";
                case "015": return "御殿場市";
                case "016": return "袋井市";
                case "019": return "下田市";
                case "020": return "裾野市";
                case "021": return "湖西市";
                case "022": return "東伊豆町";
                case "023": return "河津町";
                case "024": return "南伊豆町";
                case "025": return "松崎町";
                case "026": return "西伊豆町";
                case "032": return "函南町";
                case "037": return "清水町";
                case "038": return "長泉町";
                case "039": return "小山町";
                case "040": return "芝川町";
                case "049": return "吉田町";
                case "052": return "川根本町";
                case "060": return "森町";
                case "072": return "新居町";
                case "081": return "伊豆市";
                case "082": return "御前崎市";
                case "083": return "菊川市";
                case "084": return "伊豆の国市";
                case "085": return "牧之原市";
                default: return CityCode;
            }
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
    public int PtFutan
    {
        get => ReceInf.PtFutan;
    }

    public string WelfareHoubetu
    {
        get =>
            kohiHoubetus.Contains(ReceInf.Kohi1Houbetu) ? ReceInf.Kohi1Houbetu :
            kohiHoubetus.Contains(ReceInf.Kohi2Houbetu) ? ReceInf.Kohi2Houbetu :
            kohiHoubetus.Contains(ReceInf.Kohi3Houbetu) ? ReceInf.Kohi3Houbetu :
            kohiHoubetus.Contains(ReceInf.Kohi4Houbetu) ? ReceInf.Kohi4Houbetu :
            "";
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
    public string WelfareTokusyuNo
    {
        get =>
            kohiHoubetus.Contains(ReceInf.Kohi1Houbetu) ? PtKohi1.TokusyuNo :
            kohiHoubetus.Contains(ReceInf.Kohi2Houbetu) ? PtKohi2.TokusyuNo :
            kohiHoubetus.Contains(ReceInf.Kohi3Houbetu) ? PtKohi3.TokusyuNo :
            kohiHoubetus.Contains(ReceInf.Kohi4Houbetu) ? PtKohi4.TokusyuNo :
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
    /// 患者負担率
    /// </summary>
    public int PtRate
    {
        get => ReceInf.PtRate;
    }

    /// <summary>
    /// 点数
    /// </summary>
    public int Tensu
    {
        get => ReceInf.Tensu;
    }

    /// <summary>
    /// 公費分点数
    /// </summary>
    public int KohiReceTensu(string kohiHoubetu)
    {
        return
            kohiHoubetu == ReceInf.Kohi1Houbetu ? ReceInf.Kohi1ReceTensu ?? 0 :
            kohiHoubetu == ReceInf.Kohi2Houbetu ? ReceInf.Kohi2ReceTensu ?? 0 :
            kohiHoubetu == ReceInf.Kohi3Houbetu ? ReceInf.Kohi3ReceTensu ?? 0 :
            kohiHoubetu == ReceInf.Kohi4Houbetu ? ReceInf.Kohi4ReceTensu ?? 0 :
            0;
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
    /// 法別番号
    /// </summary>
    public string Houbetu
    {
        get => ReceInf.Houbetu;
    }

    /// <summary>
    /// 公費法別
    /// </summary>
    /// <param name="kohiIndex"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 国保組合
    /// </summary>
    public bool IsKokuhoKumiai
    {
        get => ReceInf.HokensyaNo.Length < 6 ? false :
            ReceInf.HokenKbn == Helper.Constants.HokenKbn.Kokho &&
            ReceInf.ReceSbt.Substring(1, 1) != "3" &&   //後期以外
            ReceInf.HokensyaNo.Substring(ReceInf.HokensyaNo.Length - 6, 6).Substring(2, 1) == "3";
    }

    /// <summary>
    /// レセプト種別
    /// </summary>
    public string ReceSbt
    {
        get => ReceInf.ReceSbt;
    }

    /// <summary>
    /// 公費レセ記載
    /// </summary>
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
    /// 高齢者
    /// 
    /// </summary>
    public bool IsElder
    {
        get => ReceInf.ReceSbt.Substring(3, 1) == "0" || ReceInf.ReceSbt.Substring(3, 1) == "8";
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
}
