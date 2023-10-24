namespace EmrCloudApi.Responses.PatientInfor;

public class GetPtInfModelsByRefNoResponse
{
    public GetPtInfModelsByRefNoResponse(List<PatientInfoDto> patientInfoList)
    {
        PatientInfoList = patientInfoList;
    }

    public List<PatientInfoDto> PatientInfoList { get; private set; }
}
