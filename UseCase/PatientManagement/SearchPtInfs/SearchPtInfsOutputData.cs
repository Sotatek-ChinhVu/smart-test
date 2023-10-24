using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientManagement.SearchPtInfs
{
    public class SearchPtInfsOutputData : IOutputData
    {
        public SearchPtInfsOutputData(int totalCount, List<PatientInforModel> ptInfs, SearchPtInfsStatus status)
        {
            TotalCount = totalCount;
            PtInfs = ptInfs;
            Status = status;
        }

        public int TotalCount { get; private set; }

        public List<PatientInforModel> PtInfs { get; private set; }

        public SearchPtInfsStatus Status { get; private set; }
    }
}
