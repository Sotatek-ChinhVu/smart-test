using UseCase.PatientInfor.GetPatientInfoBetweenTimesList;

namespace EmrCloudApi.Responses.PatientInfor;

public class GetPatientInfoBetweenTimesListResponse
{
    public GetPatientInfoBetweenTimesListResponse(List<PatientInfoOutputItem> patientInfoList)
    {
        PatientInfoList = patientInfoList;
    }

    public List<PatientInfoOutputItem> PatientInfoList { get; private set; }
}
