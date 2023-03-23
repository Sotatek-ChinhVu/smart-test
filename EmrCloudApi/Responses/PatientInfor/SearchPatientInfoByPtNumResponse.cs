using Domain.Models.PatientInfor;

namespace EmrCloudApi.Responses.PatientInfor;

public class SearchPatientInfoByPtNumResponse
{
    public SearchPatientInfoByPtNumResponse(PatientInforModel patientInfor)
    {
        PatientInfor = patientInfor;
    }

    public PatientInforModel PatientInfor { get;private set; }
}
