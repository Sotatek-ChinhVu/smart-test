namespace Reporting.Accounting.Model;

public class CoJihiSbtKingakuModel
{
    public CoJihiSbtKingakuModel(int jihiSbt, double kingaku)
    {
        JihiSbt = jihiSbt;
        Kingaku = kingaku;
    }
    /// <summary>
    /// 自費種別
    /// TEN_MST.自費種別
    /// </summary>
    public int JihiSbt { get; set; } = 0;
    /// <summary>
    /// 金額
    /// </summary>
    public double Kingaku { get; set; } = 0;
}
