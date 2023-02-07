using Domain.Models.Medical;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.SummaryInf
{
    public class SummaryInfOutputData : IOutputData
    {
        public SummaryInfOutputData(List<SummaryInfItem> summaryInfs)
        {
            SummaryInfs = summaryInfs;
        }

        public List<SummaryInfItem> SummaryInfs { get; private set; }
    }
}
