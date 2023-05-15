namespace Reporting.Statistics.Sta3050.Models;

public class CoSinKouiModel
{
    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum { get; set; }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string PtKanaName { get; set; } = string.Empty;

    /// <summary>
    /// 氏名
    /// </summary>
    public string PtName { get; set; } = string.Empty;

    /// <summary>
    /// 性別
    /// </summary>
    public int Sex { get; set; }

    /// <summary>
    /// 生年月日
    /// </summary>
    public int BirthDay { get; set; }

    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo { get; set; }

    /// <summary>
    /// 診療年月
    /// </summary>
    public int SinYm { get; set; }

    /// <summary>
    /// 診療日
    /// </summary>
    public int SinDate { get; set; }

    /// <summary>
    /// 診療識別
    /// </summary>
    public string SinId { get; set; } = string.Empty;

    /// <summary>
    /// 診療識別名称
    /// </summary>
    public string SinIdName
    {
        get
        {
            switch (SinId)
            {
                case "1x": return "初再診";
                case "13": return "医学管理";
                case "14": return "在宅";
                case "21": return "投薬(内服)";
                case "22": return "投薬(頓服)";
                case "23": return "投薬(外用)";
                case "2x": return "投薬(その他)";
                case "31": return "注射(皮下筋)";
                case "32": return "注射(静脈内)";
                case "33": return "注射(その他)";
                case "40": return "処置";
                case "50": return "手術";
                case "54": return "麻酔";
                case "60": return "検査";
                case "70": return "画像診断";
                case "80": return "その他";
                case "96": return "自費";
            }
            return "";
        }
    }

    /// <summary>
    /// 単価
    /// </summary>
    public double Ten { get; set; }

    /// <summary>
    /// 円点区分
    ///     0:点数 1:金額
    /// </summary>
    public int EntenKbn { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public double Suryo { get; set; }

    /// <summary>
    /// 単位名称
    /// </summary>
    public string UnitName { get; set; } = string.Empty;

    /// <summary>
    /// 回数
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 合計数量（数量×回数）
    /// </summary>
    public double TotalSuryo { get; set; }

    /// <summary>
    /// 金額
    /// </summary>
    public int Money { get; set; }

    /// <summary>
    /// 診療行為区分
    /// </summary>
    public string SinKouiKbn { get; set; } = string.Empty;

    /// <summary>
    /// 診療行為区分名称
    /// </summary>
    public string SinKouiKbnName
    {
        get
        {
            switch (SinKouiKbn)
            {
                case "11": return "初診";
                case "12": return "再診";
                case "13": return "医学管理";
                case "14": return "在宅";
                case "20": return "投薬";
                case "21": return "内用薬";
                case "23": return "外用薬";
                case "2x": return "他薬";
                case "25": return "処方料";
                case "26": return "麻毒加算";
                case "27": return "調基";
                case "30": return "注射薬";
                case "31": return "皮下筋";
                case "32": return "静脈内";
                case "33": return "点滴";
                case "34": return "他注";
                case "40": return "処置";
                case "50": return "手術";
                case "54": return "麻酔";
                case "60": return "検査";
                case "61": return "検体検査";
                case "62": return "生体検査";
                case "64": return "病理診断";
                case "70": return "画像診断";
                case "77": return "フィルム";
                case "80": return "その他";
                case "81": return "リハビリ";
                case "82": return "精神";
                case "83": return "処方箋料";
                case "84": return "放射線";
                case "96": return "保険外";
                case "99": return "コメント";
                case "T": return "特材";
            }
            return "";
        }
    }

    /// <summary>
    /// 診療行為コード
    /// </summary>
    public string ItemCd { get; set; } = string.Empty;

    /// <summary>
    /// 診療行為コード（フリーコメントの場合は名称を入れる）
    /// </summary>
    public string ItemCdCmt { get; set; } = string.Empty;

    /// <summary>
    /// 診療行為名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 診療行為カナ名称
    /// </summary>
    public string ItemKanaName1 { get; set; } = string.Empty;
    public string ItemKanaName2 { get; set; } = string.Empty;
    public string ItemKanaName3 { get; set; } = string.Empty;
    public string ItemKanaName4 { get; set; } = string.Empty;
    public string ItemKanaName5 { get; set; } = string.Empty;
    public string ItemKanaName6 { get; set; } = string.Empty;
    public string ItemKanaName7 { get; set; } = string.Empty;

    /// <summary>
    /// 麻毒区分
    /// </summary>
    public int MadokuKbn { get; set; }

    /// <summary>
    /// 向精神薬区分
    /// </summary>
    public int KouseisinKbn { get; set; }

    /// <summary>
    /// 課税区分
    /// </summary>
    public int KazeiKbn { get; set; }

    /// <summary>
    /// 診療科ID
    /// </summary>
    public int KaId { get; set; }

    /// <summary>
    /// 診療科略称
    /// </summary>
    public string KaSname { get; set; } = string.Empty;

    /// <summary>
    /// 担当医ID
    /// </summary>
    public int TantoId { get; set; }

    /// <summary>
    /// 担当医略称
    /// </summary>
    public string TantoSname { get; set; } = string.Empty;

    /// <summary>
    /// 初再診区分
    /// </summary>
    public int SyosaisinKbn { get; set; }

    /// <summary>
    /// 保険組合せID
    /// </summary>
    public int HokenPid { get; set; }

    /// <summary>
    /// 保険区分
    /// </summary>
    public int HokenKbn { get; set; }

    /// <summary>
    /// 法別番号
    /// </summary>
    public string Houbetu { get; set; } = string.Empty;

    /// <summary>
    /// 保険種別コード
    /// </summary>
    public int HokenSbtCd { get; set; }

    /// <summary>
    /// 保険種別
    /// </summary>
    public string HokenSbt
    {
        get
        {
            switch (HokenKbn)
            {
                case 0: return Houbetu == "109" ? "自レ" : "自費";
                case 1: return "社保";
                case 2: return HokenSbtCd / 100 == 3 ? "後期" : "国保";
                case 11:
                case 12:
                case 13: return "労災";
                case 14: return "自賠";
            }
            return "";
        }
    }

    /// <summary>
    /// 公１法別
    /// </summary>
    public string Kohi1Houbetu { get; set; } = string.Empty;

    /// <summary>
    /// 公２法別
    /// </summary>
    public string Kohi2Houbetu { get; set; } = string.Empty;

    /// <summary>
    /// 公３法別
    /// </summary>
    public string Kohi3Houbetu { get; set; } = string.Empty;

    /// <summary>
    /// 公４法別
    /// </summary>
    public string Kohi4Houbetu { get; set; } = string.Empty;


    /// <summary>
    /// 院外処方区分
    /// </summary>
    public int InoutKbn { get; set; }

    /// <summary>
    /// 後発医薬品区分
    /// </summary>
    public int KohatuKbn { get; set; }

    /// <summary>
    /// 採用区分
    /// </summary>
    public int IsAdopted { get; set; }
}