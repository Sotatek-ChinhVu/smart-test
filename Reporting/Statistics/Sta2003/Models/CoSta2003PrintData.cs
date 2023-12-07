using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta2003.Models;

public class CoSta2003PrintData
{
    public CoSta2003PrintData(RowType rowType = RowType.Data)
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
    public string TotalCaption { get; set; } = string.Empty;

    /// <summary>
    /// 合計行の件数
    /// </summary>
    public string TotalCount { get; set; } = string.Empty;

    /// <summary>
    /// 合計行の実人数
    /// </summary>
    public string TotalPtCount { get; set; } = string.Empty;

    /// <summary>
    /// 入金月
    /// </summary>
    public int NyukinYm { get; set; }

    /// <summary>
    /// 入金月 (yyyy/MM)
    /// </summary>
    public string NyukinYmFmt
    {
        get => CIUtil.SMonthToShowSMonth(NyukinYm);
    }

    /// <summary>
    /// 診療科ID
    /// </summary>
    public string KaId { get; set; } = string.Empty;

    /// <summary>
    /// 診療科略称
    /// </summary>
    public string KaSname { get; set; } = string.Empty;

    /// <summary>
    /// 担当医ID
    /// </summary>
    public string TantoId { get; set; } = string.Empty;

    /// <summary>
    /// 担当医略称
    /// </summary>
    public string TantoSname { get; set; }  = string.Empty;

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
    /// 保険種別
    /// </summary>
    public string HokenSbt { get; set; } = string.Empty;

    /// <summary>
    /// 保険種別コード（条件指定用）
    /// </summary>
    public int HokenSbtCd
    {
        get
        {
            switch (CIUtil.Copy(HokenSbt, 1, 1))
            {
                case "社": return 1;
                case "公": return 2;
                case "国": return 3;
                case "退": return 4;
                case "後": return 5;
                case "労": return 10;
            }

            switch (CIUtil.Copy(HokenSbt, 1, 2))
            {
                case "自賠": return 11;
                case "自費": return 12;
                case "自レ": return 13;
            }

            return 0;
        }
    }

    /// <summary>
    /// 来院回数
    /// </summary>
    public string RaiinCount { get; set; } = string.Empty;

    /// <summary>
    /// 来院日数
    /// </summary>
    public string RaiinDayCount { get; set; } = string.Empty;

    /// <summary>
    /// 合計点数
    /// </summary>
    public string Tensu { get; set; } = string.Empty;

    /// <summary>
    /// 合計点数(新)
    /// </summary>
    public string NewTensu { get; set; } = string.Empty;

    /// <summary>
    /// 負担金額
    /// </summary>
    public string PtFutan { get; set; } = string.Empty;

    /// <summary>
    /// 保険外金額
    /// </summary>
    public string JihiFutan { get; set; } = string.Empty;

    /// <summary>
    /// 消費税
    /// </summary>
    public string JihiTax { get; set; } = string.Empty;

    /// <summary>
    /// 調整額
    /// </summary>
    public string AdjustFutan { get; set; } = string.Empty;

    /// <summary>
    /// 合計請求額
    /// </summary>
    public string SeikyuGaku { get; set; } = string.Empty;

    /// <summary>
    /// 合計請求額(新)
    /// </summary>
    public string NewSeikyuGaku { get; set; } = string.Empty;

    /// <summary>
    /// 免除額
    /// </summary>
    public string MenjyoGaku { get; set; } = string.Empty;

    /// <summary>
    /// 入金額
    /// </summary>
    public string NyukinGaku { get; set; } = string.Empty;

    /// <summary>
    /// 未収額
    /// </summary>
    public string MisyuGaku { get; set; } = string.Empty;

    /// <summary>
    /// 期間外入金額
    /// </summary>
    public string PostNyukinGaku { get; set; } = string.Empty;

    /// <summary>
    /// 期間外調整額
    /// </summary>
    public string PostAdjustFutan { get; set; } = string.Empty;

    /// <summary>
    /// 保険種別ごとの金額
    /// </summary>
    public List<string> JihiSbtFutans { get; set; } = new();
}
