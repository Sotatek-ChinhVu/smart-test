using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetPatientInfoBetweenTimesList;

public class GetPatientInfoBetweenTimesListOutputData : IOutputData
{
    public GetPatientInfoBetweenTimesListOutputData(List<PatientInfoOutputItem> patientInfoList, GetPatientInfoBetweenTimesListStatus status)
    {
        PatientInfoList = patientInfoList;
        Status = status;
    }

    public List<PatientInfoOutputItem> PatientInfoList { get; private set; }

    public GetPatientInfoBetweenTimesListStatus Status { get; private set; }
}
