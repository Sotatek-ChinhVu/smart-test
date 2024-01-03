using Helper.Common;
using Helper.Extension;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3050.Models;

public class CoSta3050PrintData
{
    public CoSta3050PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 合計行のキャプション
    /// </summary>
    public string TotalCaption { get; set; }

    /// <summary>
    /// 診療日
    /// </summary>
    public int SinDate { get; set; }

    /// <summary>
    /// 診療日 (yyyy/MM/dd)
    /// </summary>
    public string SinDateS { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum { get; set; }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string PtKanaName { get; set; }

    /// <summary>
    /// 氏名
    /// </summary>
    public string PtName { get; set; }

    /// <summary>
    /// 性別コード
    /// </summary>
    public int SexCd { get; set; }

    /// <summary>
    /// 性別
    /// </summary>
    public string Sex
    {
        get
        {
            switch (SexCd)
            {
                case 1: return "男";
                case 2: return "女";
            }
            return "";
        }
    }

    /// <summary>
    /// 生年月日
    /// </summary>
    public int BirthDay { get; set; }

    /// <summary>
    /// 年齢
    /// </summary>
    public string Age
    {
        get =>
            BirthDay == 0 ? "" :
            AgeYear.AsInteger() <= 9 ? string.Format($"{AgeYear}y{AgeMonth}m") :
            AgeYear;
    }

    /// <summary>
    /// 年齢(年数)
    /// </summary>
    public string AgeYear
    {
        get =>
            BirthDay == 0 ? "" :
            CIUtil.SDateToAge(BirthDay, SinDate).ToString();
    }

    /// <summary>
    /// 年齢(月数)
    /// </summary>
    public string AgeMonth
    {
        get
        {
            if (BirthDay == 0) return "";

            int ageYear = 0;
            int ageMonth = 0;
            int ageDay = 0;

            CIUtil.SDateToDecodeAge(BirthDay, SinDate, ref ageYear, ref ageMonth, ref ageDay);

            return ageMonth.ToString();
        }

    }

    /// <summary>
    /// 診療科ID
    /// </summary>
    public int KaId { get; set; }

    /// <summary>
    /// 診療科略称
    /// </summary>
    public string KaSname { get; set; }

    /// <summary>
    /// 担当医ID
    /// </summary>
    public int TantoId { get; set; }

    /// <summary>
    /// 担当医略称
    /// </summary>
    public string TantoSname { get; set; }

    /// <summary>
    /// 診療識別
    /// </summary>
    public string SinId { get; set; }

    /// <summary>
    /// 診療行為区分
    /// </summary>
    public string SinKouiKbn { get; set; }

    /// <summary>
    /// 診療行為コード
    /// </summary>
    public string ItemCd { get; set; }

    /// <summary>
    /// 診療行為名称
    /// </summary>
    public string ItemName { get; set; }

    /// <summary>
    /// 単価
    /// </summary>
    public string Ten { get; set; }

    /// <summary>
    /// 単価(単位)
    /// </summary>
    public string TenUnit { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public string Suryo { get; set; }

    /// <summary>
    /// 単位名称
    /// </summary>
    public string UnitName { get; set; }

    /// <summary>
    /// 回数
    /// </summary>
    public string Count { get; set; }

    /// <summary>
    /// 合計数量
    /// </summary>
    public string TotalSuryo { get; set; }

    /// <summary>
    /// 金額
    /// </summary>
    public string Money { get; set; }

    /// <summary>
    /// 初再診コード
    /// </summary>
    public int? SyosaisinCd { get; set; }

    /// <summary>
    /// 初再診
    /// </summary>
    public string Syosaisin
    {
        get
        {
            switch (SyosaisinCd)
            {
                case 0: return "-";
                case 1: return "初診";
                case 3: return "再診";
                case 4: return "電再";
                case 5: return "自費";
                case 6: return "同初";
                case 7: return "再２";
                case 8: return "電２";
            }
            return "";
        }
    }

    /// <summary>
    /// 保険種別
    /// </summary>
    public string HokenSbt { get; set; }

    /// <summary>
    /// 院内院外区分
    /// </summary>
    public int InoutKbn { get; set; }

    /// <summary>
    /// 麻毒区分
    /// </summary>
    public int MadokuKbn { get; set; }

    /// <summary>
    /// 麻毒区分略称
    /// </summary>
    public string MadokuKbnSname
    {
        get
        {
            switch (MadokuKbn)
            {
                case 1: return "麻";
                case 2: return "毒";
                case 3: return "覚";
                case 5: return "向";
            }
            return "";
        }
    }

    /// <summary>
    /// 麻毒区分名称
    /// </summary>
    public string MadokuKbnName
    {
        get
        {
            switch (MadokuKbn)
            {
                case 1: return "麻薬";
                case 2: return "毒薬";
                case 3: return "覚せい剤原料";
                case 5: return "向精神薬";
            }
            return "";
        }
    }

    /// <summary>
    /// 向精神薬区分
    /// </summary>
    public int KouseisinKbn { get; set; }

    /// <summary>
    /// 向精神薬区分略称
    /// </summary>
    public string KouseisinKbnSname
    {
        get
        {
            switch (KouseisinKbn)
            {
                case 1: return "不";
                case 2: return "睡";
                case 3: return "う";
                case 4: return "精";
            }
            return "";
        }
    }

    /// <summary>
    /// 向精神薬区分名称
    /// </summary>
    public string KouseisinKbnName
    {
        get
        {
            switch (KouseisinKbn)
            {
                case 1: return "抗不安薬";
                case 2: return "睡眠薬";
                case 3: return "抗うつ薬";
                case 4: return "抗精神病薬";
            }
            return "";
        }
    }

    /// <summary>
    /// 後発医薬品区分
    /// </summary>
    public int KohatuKbn { get; set; }

    /// <summary>
    /// 後発医薬品区分名称
    /// </summary>
    public string KohatuKbnName
    {
        get
        {
            if (new string[] { "21", "23", "2x", "30" }.Contains(SinKouiKbn))
            {
                switch (KohatuKbn)
                {
                    case 0: return "先発品(後発なし)";
                    case 1: return "後発品";
                    case 2: return "先発品(後発あり)";
                }
            }
            return "";
        }
    }

    /// <summary>
    /// 採用区分
    /// </summary>
    public int IsAdopted { get; set; }

    /// <summary>
    /// 保険組合せID
    /// </summary>
    public int HokenPid { get; set; }

    /// <summary>
    /// 公１法別
    /// </summary>
    public string Kohi1Houbetu { get; set; }

    /// <summary>
    /// 公２法別
    /// </summary>
    public string Kohi2Houbetu { get; set; }

    /// <summary>
    /// 公３法別
    /// </summary>
    public string Kohi3Houbetu { get; set; }

    /// <summary>
    /// 公４法別
    /// </summary>
    public string Kohi4Houbetu { get; set; }
}
