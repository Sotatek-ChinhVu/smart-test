using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchSimple
{
    public class SearchPatientInfoSimpleOutputData : IOutputData
    {
        public List<PatientInfoWithGroup> PatientInfoList { get; private set; }

        public SearchPatientInfoSimpleStatus Status { get; private set; }

        public SearchPatientInfoSimpleOutputData(List<PatientInfoWithGroup> patientInfoList, SearchPatientInfoSimpleStatus status)
        {
            PatientInfoList = patientInfoList;
            Status = status;
        }
    }
}