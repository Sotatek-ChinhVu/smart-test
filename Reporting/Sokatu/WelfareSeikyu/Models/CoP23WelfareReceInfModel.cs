using Entity.Tenant;
using Helper.Common;
using Helper.Extension;

namespace Reporting.Sokatu.WelfareSeikyu.Models;

public class CoP23WelfareReceInfModel
{
    public ReceInf ReceInf { get; private set; }
    public PtInf PtInf { get; private set; }
    public PtKohi PtKohi1 { get; private set; } = new PtKohi();
    public PtKohi PtKohi2 { get; private set; } = new PtKohi();
    public PtKohi PtKohi3 { get; private set; } = new PtKohi();
    public PtKohi PtKohi4 { get; private set; } = new PtKohi();

    private List<string> kohiHoubetus;

    public CoP23WelfareReceInfModel(ReceInf receInf, PtInf ptInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4, List<string> kohiHoubetus)
    {
        ReceInf = receInf;
        PtInf = ptInf;
        PtKohi1 = ptKohi1;
        PtKohi2 = ptKohi2;
        PtKohi3 = ptKohi3;
        PtKohi4 = ptKohi4;
        this.kohiHoubetus = kohiHoubetus;
    }

    public CoP23WelfareReceInfModel()
    {
        ReceInf = new();
        PtInf = new();
        PtKohi1 = new();
        PtKohi2 = new();
        PtKohi3 = new();
        PtKohi4 = new();
        this.kohiHoubetus = new();
    }

    /// <summary>
    /// 診療年月
    /// </summary>
    public int SinYm
    {
        get => ReceInf.SinYm;
    }

    public string CityName
    {
        get
        {
            if (WelfareJyukyusyaNo?.Length < 3) return "";

            if ((WelfareHoubetu == "81" && WelfareJyukyusyaNo?.Substring(0, 3).AsInteger() >= 0 && WelfareJyukyusyaNo.Substring(0, 3).AsInteger() <= 199) ||
                (WelfareHoubetu == "82" && WelfareJyukyusyaNo?.Substring(0, 3).AsInteger() >= 700 && WelfareJyukyusyaNo.Substring(0, 3).AsInteger() <= 799) ||
                (WelfareHoubetu == "83" && WelfareJyukyusyaNo?.Substring(0, 3).AsInteger() >= 600 && WelfareJyukyusyaNo.Substring(0, 3).AsInteger() <= 699))
            {
                return "名古屋市";
            }
            if (WelfareJyukyusyaNo?.Substring(1, 2) == "02" || WelfareJyukyusyaNo?.Substring(1, 2) == "00")
            {
                return "豊橋市";
            }
            if (WelfareJyukyusyaNo?.Substring(1, 1) == "9")
            {
                return "岡崎市";
            }

            switch (WelfareJyukyusyaNo?.Substring(1, 2))
            {
                case "04": return "一宮市"; 
                case "05": return "瀬戸市";
                case "06": return "半田市";
                case "07": return "春日井市";
                case "08": return "豊川市";
                case "09": return "津島市";
                case "10": return "碧南市";
                case "11": return "刈谷市";
                case "12": return "豊田市";
                case "13": return "安城市";
                case "14": return "西尾市";
                case "15": return "蒲郡市";
                case "16": return "犬山市";
                case "17": return "常滑市";
                case "18": return "江南市";
                case "20": return "小牧市";
                case "21": return "稲沢市";
                case "22": return "新城市";
                case "23": return "東海市";
                case "24": return "大府市";
                case "25": return "知多市";
                case "26": return "知立市";
                case "27": return "尾張旭市";
                case "28": return "高浜市";
                case "29": return "岩倉市";
                case "30": return "豊明市";
                case "54": return "愛西市";
                case "40": return "清須市";
                case "37": return "北名古屋市";
                case "32": return "日進市";
                case "46": return "あま市";
                case "53": return "弥富市";
                case "68": return "みよし市";
                case "86": return "田原市";
                case "31": return "東郷町";
                case "33": return "長久手市";
                case "35": return "豊山町";
                case "41": return "大口町";
                case "42": return "扶桑町";
                case "49": return "大治町";
                case "50": return "蟹江町";
                case "52": return "飛島村";
                case "58": return "阿久比町";
                case "59": return "東浦町";
                case "60": return "南知多町";
                case "61": return "美浜町";
                case "62": return "武豊町";
                case "66": return "幸田町";
                case "74": return "設楽町";
                case "75": return "東栄町";
                case "76": return "豊根村";
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
            kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? ReceInf.Kohi1Futan :
            kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? ReceInf.Kohi2Futan :
            kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? ReceInf.Kohi3Futan :
            kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? ReceInf.Kohi4Futan :
            0;
    }

    public string WelfareHoubetu
    {
        get =>
            kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? ReceInf.Kohi1Houbetu ?? string.Empty :
            kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? ReceInf.Kohi2Houbetu ?? string.Empty :
            kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? ReceInf.Kohi3Houbetu ?? string.Empty :
            kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? ReceInf.Kohi4Houbetu ?? string.Empty :
            "";
    }

    public string? WelfareJyukyusyaNo
    {
        get =>
            kohiHoubetus.Contains(ReceInf.Kohi1Houbetu ?? string.Empty) ? PtKohi1.TokusyuNo :
            kohiHoubetus.Contains(ReceInf.Kohi2Houbetu ?? string.Empty) ? PtKohi2.TokusyuNo :
            kohiHoubetus.Contains(ReceInf.Kohi3Houbetu ?? string.Empty) ? PtKohi3.TokusyuNo :
            kohiHoubetus.Contains(ReceInf.Kohi4Houbetu ?? string.Empty) ? PtKohi4.TokusyuNo :
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
        get => ReceInf.Houbetu ?? string.Empty;
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
            case 1: return ReceInf.Kohi1Houbetu ?? string.Empty;
            case 2: return ReceInf.Kohi2Houbetu ?? string.Empty;
            case 3: return ReceInf.Kohi3Houbetu ?? string.Empty;
            case 4: return ReceInf.Kohi4Houbetu ?? string.Empty;
        }
        return "";
    }

    /// <summary>
    /// 国保組合
    /// </summary>
    public bool IsKokuhoKumiai
    {
        get => ReceInf.HokensyaNo?.Length < 6 ? false :
            ReceInf.HokenKbn == Helper.Constants.HokenKbn.Kokho &&
            ReceInf.ReceSbt?.Substring(1, 1) != "3" &&   //後期以外
            ReceInf.HokensyaNo?.Substring(ReceInf.HokensyaNo.Length - 6, 6).Substring(2, 1) == "3";
    }

    /// <summary>
    /// レセプト種別
    /// </summary>
    public string ReceSbt
    {
        get => ReceInf.ReceSbt ?? string.Empty;
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
        get => ReceInf.ReceSbt?.Substring(3, 1) == "0" || ReceInf.ReceSbt?.Substring(3, 1) == "8";
    }

    public bool TokkiContains(string tokkiCd)
    {
        return
            CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 1, 2) == tokkiCd ||
            CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 3, 2) == tokkiCd ||
            CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 5, 2) == tokkiCd ||
            CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 7, 2) == tokkiCd ||
            CIUtil.Copy(ReceInf.Tokki ?? string.Empty, 9, 2) == tokkiCd;
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
        get => PtInf.Name ?? string.Empty;
    }
}
