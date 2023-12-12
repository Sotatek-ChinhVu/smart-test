using System.Text.Json.Serialization;

namespace Reporting.OutDrug.Model;

public class PrintOutData
{
    public PrintOutData()
    {
    }

    /// <summary>
    /// 変更不可マーク
    /// </summary>
    public string HenkoMark { get; set; } = string.Empty;
    /// <summary>
    /// RP番号
    /// </summary>
    public string RpInf { get; set; } = string.Empty;
    /// <summary>
    /// 薬剤名や用法名など
    /// </summary>
    public string Data { get; set; } = string.Empty;
    /// <summary>
    /// 数量
    /// </summary>
    public string Suryo { get; set; } = string.Empty;
    /// <summary>
    /// 単位名
    /// </summary>
    public string UnitName { get; set; } = string.Empty;
    /// <summary>
    /// 回数
    /// </summary>
    public string Kaisu { get; set; } = string.Empty;
    /// <summary>
    /// 用法単位
    /// </summary>
    public string YohoUnit { get; set; } = string.Empty;
    /// <summary>
    /// 文革指示
    /// </summary>
    public string Bunkatu { get; set; } = string.Empty;

    public bool IsClearData
    {
        get
        {
            return (string.IsNullOrEmpty(HenkoMark) &&
                    string.IsNullOrEmpty(RpInf) &&
                    string.IsNullOrEmpty(Data) &&
                    string.IsNullOrEmpty(Suryo) &&
                    string.IsNullOrEmpty(UnitName) &&
                    string.IsNullOrEmpty(Kaisu) &&
                    string.IsNullOrEmpty(YohoUnit) &&
                    string.IsNullOrEmpty(Bunkatu)
                );
        }
    }
}