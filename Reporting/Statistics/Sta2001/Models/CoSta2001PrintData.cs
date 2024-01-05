using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta2001.Models;

public class CoSta2001PrintData
{
    public CoSta2001PrintData(RowType rowType = RowType.Data)
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
    /// 入金日
    /// </summary>
    public int NyukinDate { get; set; }

    /// <summary>
    /// 入金日 (yyyy/MM/dd)
    /// </summary>
    public string NyukinDateFmt
    {
        get => CIUtil.SDateToShowSDate(NyukinDate);
    }

    /// <summary>
    /// 入金日 (MM/dd(mmm))
    /// </summary>
    public string NyukinDateFmt2 { get; set; }

    /// <summary>
    /// 診療科ID
    /// </summary>
    public string KaId { get; set; }

    /// <summary>
    /// 診療科略称
    /// </summary>
    public string KaSname { get; set; }

    /// <summary>
    /// 担当医ID
    /// </summary>
    public string TantoId { get; set; }

    /// <summary>
    /// 担当医略称
    /// </summary>
    public string TantoSname { get; set; }

    /// <summary>
    /// 初診件数
    /// </summary>
    public string SyosinCount { get; set; }

    /// <summary>
    /// 再診件数
    /// </summary>
    public string SaisinCount { get; set; }

    /// <summary>
    /// 合計件数
    /// </summary>
    public string Count { get; set; }

    /// <summary>
    /// 新患件数
    /// </summary>
    public string NewPtCount { get; set; }

    /// <summary>
    /// 患者実人数
    /// </summary>
    public string PtCount { get; set; }

    /// <summary>
    /// 合計点数
    /// </summary>
    public string Tensu { get; set; }

    /// <summary>
    /// 合計点数(新)
    /// </summary>
    public string NewTensu { get; set; }

    /// <summary>
    /// 負担金額
    /// </summary>
    public string PtFutan { get; set; }

    /// <summary>
    /// 保険外金額
    /// </summary>
    public string JihiFutan { get; set; }

    /// <summary>
    /// 消費税
    /// </summary>
    public string JihiTax { get; set; }

    /// <summary>
    /// 調整額
    /// </summary>
    public string AdjustFutan { get; set; }

    /// <summary>
    /// 合計請求額
    /// </summary>
    public string SeikyuGaku { get; set; }

    /// <summary>
    /// 合計請求額(新)
    /// </summary>
    public string NewSeikyuGaku { get; set; }

    /// <summary>
    /// 免除額
    /// </summary>
    public string MenjyoGaku { get; set; }

    /// <summary>
    /// 入金額
    /// </summary>
    public string NyukinGaku { get; set; }

    /// <summary>
    /// 入金額（社保）
    /// </summary>
    public string NyukinGakuSyaho { get; set; }

    /// <summary>
    /// 入金額（国保）
    /// </summary>
    public string NyukinGakuKokho { get; set; }

    /// <summary>
    /// 入金額（公費）
    /// </summary>
    public string NyukinGakuKohi { get; set; }

    /// <summary>
    /// 入金額（自費）
    /// </summary>
    public string NyukinGakuJihi { get; set; }

    /// <summary>
    /// 入金額（自費レセ）
    /// </summary>
    public string NyukinGakuJihiRece { get; set; }

    /// <summary>
    /// 入金額（労災）
    /// </summary>
    public string NyukinGakuRousai { get; set; }

    /// <summary>
    /// 入金額（自賠）
    /// </summary>
    public string NyukinGakuJibai { get; set; }

    /// <summary>
    /// 未収額
    /// </summary>
    public string MisyuGaku { get; set; }

    /// <summary>
    /// 期間外入金額
    /// </summary>
    public string PostNyukinGaku { get; set; }

    /// <summary>
    /// 期間外入金額（社保）
    /// </summary>
    public string PostNyukinGakuSyaho { get; set; }

    /// <summary>
    /// 期間外入金額（国保）
    /// </summary>
    public string PostNyukinGakuKokho { get; set; }

    /// <summary>
    /// 期間外入金額（公費）
    /// </summary>
    public string PostNyukinGakuKohi { get; set; }

    /// <summary>
    /// 期間外入金額（自費）
    /// </summary>
    public string PostNyukinGakuJihi { get; set; }

    /// <summary>
    /// 期間外入金額（自費レセ）
    /// </summary>
    public string PostNyukinGakuJihiRece { get; set; }

    /// <summary>
    /// 期間外入金額（労災）
    /// </summary>
    public string PostNyukinGakuRousai { get; set; }

    /// <summary>
    /// 期間外入金額（自賠）
    /// </summary>
    public string PostNyukinGakuJibai { get; set; }

    /// <summary>
    /// 期間外調整額
    /// </summary>
    public string PostAdjustFutan { get; set; }

    /// <summary>
    /// 保険種別ごとの金額
    /// </summary>
    public List<string> JihiSbtFutans { get; set; }
}
