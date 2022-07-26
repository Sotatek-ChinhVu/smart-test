using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchSimple
{
    public class SearchPatientInfoSimpleOutputData : IOutputData
    {
        public List<PatientInforModel> PatientInfoList { get; private set; }

        public SearchPatientInfoSimpleStatus Status { get; private set; }

        public SearchPatientInfoSimpleOutputData(List<PatientInforModel> patientInfoList, SearchPatientInfoSimpleStatus status)
        {
            PatientInfoList = patientInfoList;
            Status = status;
        }
    }
}
