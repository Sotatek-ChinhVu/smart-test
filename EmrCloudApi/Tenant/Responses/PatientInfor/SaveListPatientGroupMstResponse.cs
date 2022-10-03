namespace EmrCloudApi.Tenant.Responses.PatientInfor;

public class SaveListPatientGroupMstResponse
{
    public SaveListPatientGroupMstResponse(bool status)
    {
        Status = status;
    }

    public bool Status { get; private set; }
}
