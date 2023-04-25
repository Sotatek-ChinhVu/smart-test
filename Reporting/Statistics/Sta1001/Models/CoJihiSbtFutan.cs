namespace Reporting.Statistics.Sta1001.Models;

public class CoJihiSbtFutan
{
    public CoJihiSbtFutan(long ptId, long raiinNo, int jihiSbt, int jihiFutan)
    {
        PtId = ptId;
        RaiinNo = raiinNo;
        JihiSbt = jihiSbt;
        JihiFutan = jihiFutan;
    }

    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId { get; private set; }

    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo { get; private set; }

    /// <summary>
    /// 自費種別
    /// 代表項目のJIHI_SBT_MST.JIHI_SBT
    /// </summary>
    public int JihiSbt { get; private set; }

    /// <summary>
    /// 保険外金額
    /// </summary>
    public int JihiFutan { get; private set; }
}
