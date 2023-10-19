namespace EmrCloudApi.Responses.PatientInfor;

public class UpdateVisitTimesManagementNeedSaveResponse
{
    public UpdateVisitTimesManagementNeedSaveResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
