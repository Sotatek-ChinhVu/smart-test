using UseCase.PatientInfor;

namespace EmrCloudApi.Responses.PatientInfor;

public class SearchPatientInfoAdvancedResponse
{
    public SearchPatientInfoAdvancedResponse(List<PatientInfoWithGroup> patientInfos)
    {
        PatientInfos = patientInfos;
    }

    public List<PatientInfoWithGroup> PatientInfos { get; private set; }
}
