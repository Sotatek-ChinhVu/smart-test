namespace EmrCloudApi.Responses.PatientInfor;

public class GetPtInfModelsByNameResponse
{
    public GetPtInfModelsByNameResponse(List<PatientInfoDto> patientInfoList)
    {
        PatientInfoList = patientInfoList;
    }

    public List<PatientInfoDto> PatientInfoList { get; private set; }
}
