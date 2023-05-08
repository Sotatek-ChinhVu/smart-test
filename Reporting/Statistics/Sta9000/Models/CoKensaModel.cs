using System.Text.RegularExpressions;

namespace Reporting.Statistics.Sta9000.Models;

public class CoKensaModel
{
    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId { get; set; }

    /// <summary>
    /// 依頼日
    /// </summary>
    public int IraiDate { get; set; }

    /// <summary>
    /// センターコード
    /// </summary>
    public string CenterCd { get; set; } = string.Empty;

    /// <summary>
    /// 検査項目コード
    /// </summary>
    public string KensaItemCd { get; set; } = string.Empty;

    /// <summary>
    /// 検査項目名
    /// </summary>
    public string KensaName { get; set; } = string.Empty;

    /// <summary>
    /// 結果値
    /// </summary>
    public string ResultVal { get; set; } = string.Empty;

    /// <summary>
    /// 結果値（数値）
    /// </summary>
    public double NumResultVal
    {
        get
        {
            var strVal = Regex.Replace(ResultVal, @"[^0-9.]", "");

            double numVal;
            double.TryParse(strVal, out numVal);

            return numVal;
        }
    }

    /// <summary>
    /// 結果値形態
    /// </summary>
    public string ResultType { get; set; } = string.Empty;

    /// <summary>
    /// 結果値 + 結果値形態
    /// </summary>
    public string ResultValue
    {
        get => ResultVal + ResultType;
    }

    /// <summary>
    /// 異常値区分
    /// </summary>
    public string AbnormalKbn { get; set; } = string.Empty;

    /// <summary>
    /// 単位名称
    /// </summary>
    public string UnitName { get; set; } = string.Empty;

    /// <summary>
    /// 基準値
    /// </summary>
    public string StandardVal { get; set; } = string.Empty;

    /// <summary>
    /// 並び順
    /// </summary>
    public string SortKey { get; set; } = string.Empty;
}
