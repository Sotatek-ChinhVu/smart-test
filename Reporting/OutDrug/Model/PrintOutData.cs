namespace Reporting.OutDrug.Model;

public class PrintOutData
{
    public PrintOutData()
    {
        HenkoMark = string.Empty;
        RpInf = string.Empty;
        Data = string.Empty;
        Suryo = string.Empty;
        UnitName = string.Empty;
        Kaisu = string.Empty;
        YohoUnit = string.Empty;
        Bunkatu = string.Empty;
    }

    /// <summary>
    /// 変更不可マーク
    /// </summary>
    public string HenkoMark { get; set; }
    /// <summary>
    /// RP番号
    /// </summary>
    public string RpInf { get; set; }
    /// <summary>
    /// 薬剤名や用法名など
    /// </summary>
    public string Data { get; set; }
    /// <summary>
    /// 数量
    /// </summary>
    public string Suryo { get; set; }
    /// <summary>
    /// 単位名
    /// </summary>
    public string UnitName { get; set; }
    /// <summary>
    /// 回数
    /// </summary>
    public string Kaisu { get; set; }
    /// <summary>
    /// 用法単位
    /// </summary>
    public string YohoUnit { get; set; }
    /// <summary>
    /// 文革指示
    /// </summary>
    public string Bunkatu { get; set; }

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