namespace EmrCloudApi.Responses.PatientInfor;

public class GetPtInfByRefNoResponse
{
    public GetPtInfByRefNoResponse(PatientInfoDto patientInfo)
    {
        PatientInfo = patientInfo;
    }

    public PatientInfoDto PatientInfo { get; private set; }
}
