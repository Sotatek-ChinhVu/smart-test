namespace Reporting.Statistics.Sta1001.Models
{
    public class CoJihiSbtFutan
    {
        public CoJihiSbtFutan(long ptId, long raiinNo, int jihiSbt, int jihiFutan)
        {
            PtId = ptId;
            RaiinNo = raiinNo;
            JihiSbt = jihiSbt;
            JihiFutan = jihiFutan;
        }

        public long PtId { get; private set; }

        public long RaiinNo { get; private set; }

        public int JihiSbt { get; private set; }

        public int JihiFutan { get; private set; }
    }
}
