namespace EmrCloudApi.Responses.SetMst;

public class SaveSuperSetDetailResponse
{
    public SaveSuperSetDetailResponse(bool status)
    {
        Status = status;
    }
    public bool Status { get; private set; } = false;
}
