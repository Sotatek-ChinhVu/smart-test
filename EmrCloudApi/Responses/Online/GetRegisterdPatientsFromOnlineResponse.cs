using EmrCloudApi.Responses.Online.Dto;

namespace EmrCloudApi.Responses.Online;

public class GetRegisterdPatientsFromOnlineResponse
{
    public GetRegisterdPatientsFromOnlineResponse(List<PatientInfoDto> patientInfoList)
    {
        PatientInfoList = patientInfoList;
    }

    public List<PatientInfoDto> PatientInfoList { get; private set; }
}
