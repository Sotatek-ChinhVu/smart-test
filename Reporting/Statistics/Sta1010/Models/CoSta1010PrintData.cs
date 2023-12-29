using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta1010.Models;

public class CoSta1010PrintData
{
    public CoSta1010PrintData(RowType rowType = RowType.Data)
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
    /// 合計行の件数
    /// </summary>
    public string TotalCount { get; set; }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum { get; set; }

    /// <summary>
    /// 患者番号Key
    /// </summary>
    public long PtNumKey { get; set; }

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
    public string Sex { get; set; }

    /// <summary>
    /// 生年月日
    /// </summary>
    public int BirthDay { get; set; }

    /// <summary>
    /// 生年月日 (yyyy/MM/dd)
    /// </summary>
    public string BirthDayFmt
    {
        get => CIUtil.SDateToShowSDate(BirthDay);
    }

    /// <summary>
    /// 年齢
    /// </summary>
    public string Age { get; set; }

    /// <summary>
    /// 郵便番号
    /// </summary>
    public string HomePost { get; set; }

    /// <summary>
    /// 住所
    /// </summary>
    public string HomeAddress { get; set; }

    /// <summary>
    /// 電話番号１
    /// </summary>
    public string Tel1 { get; set; }

    /// <summary>
    /// 電話番号２
    /// </summary>
    public string Tel2 { get; set; }

    /// <summary>
    /// 緊急連絡先電話番号
    /// </summary>
    public string RenrakuTel { get; set; }

    /// <summary>
    /// 診察日
    /// </summary>
    public int SinDate { get; set; }

    /// <summary>
    /// 診察日 (yyyy/MM/dd)
    /// </summary>
    public string SinDateFmt
    {
        get => CIUtil.SDateToShowSDate(SinDate);
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
    /// 来院番号
    /// </summary>
    public long RaiinNo { get; set; }

    /// <summary>
    /// 保険種別
    /// </summary>
    public string HokenSbt { get; set; }

    /// <summary>
    /// 初再診
    /// </summary>
    public string Syosaisin { get; set; }

    /// <summary>
    /// 請求額
    /// </summary>
    public string SeikyuGaku { get; set; }

    /// <summary>
    /// 請求額(旧)
    /// </summary>
    public string OldSeikyuGaku { get; set; }

    /// <summary>
    /// 請求額(新)
    /// </summary>
    public string NewSeikyuGaku { get; set; }

    /// <summary>
    /// 調整額
    /// </summary>
    public string AdjustFutan { get; set; }

    /// <summary>
    /// 調整額(旧)
    /// </summary>
    public string OldAdjustFutan { get; set; }

    /// <summary>
    /// 調整額(新)
    /// </summary>
    public string NewAdjustFutan { get; set; }

    /// <summary>
    /// 合計請求額
    /// </summary>
    public string TotalSeikyuGaku { get; set; }

    /// <summary>
    /// 入金額
    /// </summary>
    public string NyukinGaku { get; set; }

    /// <summary>
    /// 未収額
    /// </summary>
    public string MisyuGaku { get; set; }

    /// <summary>
    /// コメント
    /// </summary>
    public string NyukinCmt { get; set; }

    /// <summary>
    /// 最終来院日
    /// </summary>
    public int LastVisitDate { get; set; }

    /// <summary>
    /// 最終来院日 (yyyy/MM/dd)
    /// </summary>
    public string LastVisitDateFmt
    {
        get => CIUtil.SDateToShowSDate(LastVisitDate);
    }

    /// <summary>
    /// 未収区分
    /// </summary>
    public string MisyuKbn { get; set; }
}
