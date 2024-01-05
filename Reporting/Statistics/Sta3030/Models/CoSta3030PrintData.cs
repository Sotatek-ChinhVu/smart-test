using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3030.Models;

public class CoSta3030PrintData
{
    public CoSta3030PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public string PtNum { get; set; } = string.Empty;

    /// <summary>
    /// 氏名
    /// </summary>
    public string PtName { get; set; } = string.Empty;

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string PtKanaName { get; set; } = string.Empty;

    /// <summary>
    /// 生年月日
    /// </summary>
    public int BirthdayI { get; set; }

    /// <summary>
    /// 生年月日（西暦）
    /// </summary>
    public string Birthday
    {
        get => CIUtil.SDateToShowSDate(BirthdayI);
    }

    /// <summary>
    /// 生年月日（和暦）
    /// </summary>
    public string BirthdayW
    {
        get => CIUtil.SDateToShowWDate(BirthdayI);
    }

    /// <summary>
    /// 生年月日（和暦西暦）
    /// </summary>
    public string BirthdayWS
    {
        get => CIUtil.SDateToShowSWDate(BirthdayI);
    }

    /// <summary>
    /// 年齢
    /// </summary>
    public int Age { get; set; }


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
    /// 最終来院日
    /// </summary>
    public int LastVisitDateI { get; set; }

    /// <summary>
    /// 最終来院日（西暦）
    /// </summary>
    public string LastVisitDate
    {
        get => CIUtil.SDateToShowSDate(LastVisitDateI);
    }

    /// <summary>
    /// 最終来院日（和暦）
    /// </summary>
    public string LastVisitDateW
    {
        get => CIUtil.SDateToShowWDate(LastVisitDateI);
    }

    /// <summary>
    /// 最終来院日（和暦西暦）
    /// </summary>
    public string LastVisitDateWS
    {
        get => CIUtil.SDateToShowSWDate(LastVisitDateI);
    }

    /// <summary>
    /// 病名
    /// </summary>
    public string Byomei { get; set; }

    /// <summary>
    /// 病名コード
    /// </summary>
    public string ByomeiCd { get; set; }
    /// <summary>
    /// 主病区分
    /// </summary>
    public string SyubyoKbn { get; set; }

    /// <summary>
    /// 開始日
    /// </summary>
    public int StartDateI { get; set; }

    /// <summary>
    /// 開始日（西暦）
    /// </summary>
    public string StartDate
    {
        get => CIUtil.SDateToShowSDate(StartDateI);
    }

    /// <summary>
    /// 開始日（和暦）
    /// </summary>
    public string StartDateW
    {
        get => CIUtil.SDateToShowWDate(StartDateI);
    }

    /// <summary>
    /// 開始日（和暦西暦）
    /// </summary>
    public string StartDateWS
    {
        get => CIUtil.SDateToShowSWDate(StartDateI);
    }

    /// <summary>
    /// 転帰区分
    /// </summary>
    public int TenkiKbn { get; set; }

    /// <summary>
    /// 転帰
    /// </summary>
    public string Tenki
    {
        get
        {
            switch (TenkiKbn)
            {
                case 1: return "治ゆ";
                case 2: return "中止";
                case 3: return "死亡";
                case 9: return "その他";
                default: return "";
            }
        }
    }

    /// <summary>
    /// 転帰日
    /// </summary>
    public int TenkiDateI { get; set; }

    /// <summary>
    /// 転帰日（西暦）
    /// </summary>
    public string TenkiDate
    {
        get => CIUtil.SDateToShowSDate(TenkiDateI);
    }

    /// <summary>
    /// 転帰日（和暦）
    /// </summary>
    public string TenkiDateW
    {
        get => CIUtil.SDateToShowWDate(TenkiDateI);
    }

    /// <summary>
    /// 転帰日（和暦西暦）
    /// </summary>
    public string TenkiDateWS
    {
        get => CIUtil.SDateToShowSWDate(TenkiDateI);
    }

    /// <summary>
    /// 当月病名
    /// </summary>
    public int TogetuByomei { get; set; }

    /// <summary>
    /// 疾患区分コード
    /// </summary>
    public int SikkanKbnCd { get; set; }

    /// <summary>
    /// 疾患区分
    /// </summary>
    public string SikkanKbn
    {
        get
        {
            switch (SikkanKbnCd)
            {
                case 3: return "皮１";
                case 4: return "皮２";
                case 5: return "特疾";
                case 7: return "て";
                case 8: return "特て";
                default: return "";
            }
        }
    }

    /// <summary>
    /// 難病外来コード
    /// </summary>
    public int NanbyoCd { get; set; }

    /// <summary>
    /// 難病外来
    /// </summary>
    public string Nanbyo
    {
        get => NanbyoCd == 9 ? "難病" : "";
    }

    /// <summary>
    /// 補足コメント
    /// </summary>
    public string HosokuCmt { get; set; }

    /// <summary>
    /// 病名（合成）
    /// </summary>
    public string ByomeiComp
    {
        get => HosokuCmt != string.Empty ? string.Format("{0}（{1})", Byomei, HosokuCmt) : Byomei;
    }

    /// <summary>
    /// 保険
    /// </summary>
    public string Hoken { get; set; }

    /// <summary>
    /// 保険ＩＤ
    /// </summary>
    public string HokenPid { get; set; }

    /// <summary>
    /// 接頭語コード
    /// </summary>
    public string SettogoCd { get; set; }

    /// <summary>
    /// 接尾語コード
    /// </summary>
    public string SetubigoCd { get; set; }
}
