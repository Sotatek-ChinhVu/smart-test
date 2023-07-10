﻿using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.CommonMasters.Constants;

namespace Reporting.Sokatu.WelfareSeikyu.Models;

public class CoP23WelfareReceInfModel
{
    public ReceInf ReceInf { get; private set; }
    public PtInf PtInf { get; private set; }
    public PtKohi PtKohi1 { get; private set; }
    public PtKohi PtKohi2 { get; private set; }
    public PtKohi PtKohi3 { get; private set; }
    public PtKohi PtKohi4 { get; private set; }

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
            if (WelfareJyukyusyaNo.Length < 3) return "";

            if ((WelfareHoubetu == "81" && WelfareJyukyusyaNo.Substring(0, 3).AsInteger() >= 0 && WelfareJyukyusyaNo.Substring(0, 3).AsInteger() <= 199) ||
                (WelfareHoubetu == "82" && WelfareJyukyusyaNo.Substring(0, 3).AsInteger() >= 700 && WelfareJyukyusyaNo.Substring(0, 3).AsInteger() <= 799) ||
                (WelfareHoubetu == "83" && WelfareJyukyusyaNo.Substring(0, 3).AsInteger() >= 600 && WelfareJyukyusyaNo.Substring(0, 3).AsInteger() <= 699))
            {
                return "名古屋市";
            }
            if (WelfareJyukyusyaNo.Substring(1, 2) == "02" || WelfareJyukyusyaNo.Substring(1, 2) == "00")
            {
                return "豊橋市";
            }
            if (WelfareJyukyusyaNo.Substring(1, 1) == "9")
            {
                return "岡崎市";
            }

            switch (WelfareJyukyusyaNo.Substring(1, 2))
            {
                case "04": return "一宮市"; break;
                case "05": return "瀬戸市"; break;
                case "06": return "半田市"; break;
                case "07": return "春日井市"; break;
                case "08": return "豊川市"; break;
                case "09": return "津島市"; break;
                case "10": return "碧南市"; break;
                case "11": return "刈谷市"; break;
                case "12": return "豊田市"; break;
                case "13": return "安城市"; break;
                case "14": return "西尾市"; break;
                case "15": return "蒲郡市"; break;
                case "16": return "犬山市"; break;
                case "17": return "常滑市"; break;
                case "18": return "江南市"; break;
                case "20": return "小牧市"; break;
                case "21": return "稲沢市"; break;
                case "22": return "新城市"; break;
                case "23": return "東海市"; break;
                case "24": return "大府市"; break;
                case "25": return "知多市"; break;
                case "26": return "知立市"; break;
                case "27": return "尾張旭市"; break;
                case "28": return "高浜市"; break;
                case "29": return "岩倉市"; break;
                case "30": return "豊明市"; break;
                case "54": return "愛西市"; break;
                case "40": return "清須市"; break;
                case "37": return "北名古屋市"; break;
                case "32": return "日進市"; break;
                case "46": return "あま市"; break;
                case "53": return "弥富市"; break;
                case "68": return "みよし市"; break;
                case "86": return "田原市"; break;
                case "31": return "東郷町"; break;
                case "33": return "長久手市"; break;
                case "35": return "豊山町"; break;
                case "41": return "大口町"; break;
                case "42": return "扶桑町"; break;
                case "49": return "大治町"; break;
                case "50": return "蟹江町"; break;
                case "52": return "飛島村"; break;
                case "58": return "阿久比町"; break;
                case "59": return "東浦町"; break;
                case "60": return "南知多町"; break;
                case "61": return "美浜町"; break;
                case "62": return "武豊町"; break;
                case "66": return "幸田町"; break;
                case "74": return "設楽町"; break;
                case "75": return "東栄町"; break;
                case "76": return "豊根村"; break;
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
        get => ReceInf.HokensyaNo;
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
}
