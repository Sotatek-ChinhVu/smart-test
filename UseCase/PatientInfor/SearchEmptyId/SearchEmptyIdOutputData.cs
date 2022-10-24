using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchEmptyId
{
    public class SearchEmptyIdOutputData : IOutputData
    {
        public SearchEmptyIdOutputData(List<PatientInforModel> patientInforModels, SearchEmptyIdStatus status)
        {
            PatientInforModels = patientInforModels;
            Status = status;
        }

        public List<PatientInforModel> PatientInforModels { get; private set; }
        public SearchEmptyIdStatus Status { get; private set; }
    }
}
