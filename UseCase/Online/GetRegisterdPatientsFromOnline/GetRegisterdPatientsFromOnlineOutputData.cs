using UseCase.Core.Sync.Core;

namespace UseCase.Online.GetRegisterdPatientsFromOnline;

public class GetRegisterdPatientsFromOnlineOutputData : IOutputData
{
    public GetRegisterdPatientsFromOnlineOutputData(List<PatientInfoItem> patientInfoList, GetRegisterdPatientsFromOnlineStatus status)
    {
        PatientInfoList = patientInfoList;
        Status = status;
    }

    public List<PatientInfoItem> PatientInfoList { get; private set; }

    public GetRegisterdPatientsFromOnlineStatus Status { get; private set; }
}
