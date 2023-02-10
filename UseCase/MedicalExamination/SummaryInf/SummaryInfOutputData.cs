using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.SummaryInf
{
    public class SummaryInfOutputData : IOutputData
    {
        public SummaryInfOutputData(List<SummaryInfItem> summaryInfs, SummaryInfStatus status)
        {
            SummaryInfs = summaryInfs;
            Status = status;
        }

        public List<SummaryInfItem> SummaryInfs { get; private set; }

        public SummaryInfStatus Status { get; private set; }
    }
}
