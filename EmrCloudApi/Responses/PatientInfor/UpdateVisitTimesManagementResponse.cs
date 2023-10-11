namespace EmrCloudApi.Responses.PatientInfor;

public class UpdateVisitTimesManagementResponse
{
    public UpdateVisitTimesManagementResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
