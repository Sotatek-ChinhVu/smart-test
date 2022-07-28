using Domain.Models.GroupInf;
using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
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

    public class PatientInfoWithGroup
    {
        public PatientInforModel PatientInfo { get; private set; }

        public List<GroupInfModel> GroupInfList { get; private set; }

        public PatientInfoWithGroup(PatientInforModel patientInfo, List<GroupInfModel> groupInfList)
        {
            PatientInfo = patientInfo;
            GroupInfList = groupInfList;
        }
    }
}
