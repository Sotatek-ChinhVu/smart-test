using static Reporting.Karte3.Enum.CoKarte3Column;

namespace Reporting.Karte3.Model;

class CoKarte3PrintDataModel
{
    public CoKarte3PrintDataModel()
    {
        ColumnDatas = new List<double>();

        for (int i = 0; i < System.Enum.GetNames(typeof(Karte3Column)).Length; i++)
        {
            ColumnDatas.Add(0);
        }
    }

    /// <summary>
    /// データの種類
    /// 0:通常データ
    /// 1:合計行
    /// 2:ブランク
    /// </summary>
    public int DataType { get; set; }
    /// <summary>
    /// 診療日
    /// </summary>
    public string Date { get; set; }
    /// <summary>
    /// カラムのデータ
    /// </summary>
    public List<double> ColumnDatas { get; set; }

    public double this[Karte3Column index]
    {
        get { return ColumnDatas[(int)index]; }
        set { ColumnDatas[(int)index] = value; }
    }
    /// <summary>
    /// 合計点数
    /// </summary>
    public int GokeiTensu { get; set; }
    /// <summary>
    /// 合計負担額
    /// </summary>
    public int GokeiFutan { get; set; }

}
