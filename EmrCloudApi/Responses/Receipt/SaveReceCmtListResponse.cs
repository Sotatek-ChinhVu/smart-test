namespace EmrCloudApi.Responses.Receipt;

public class SaveReceCmtListResponse
{
    public SaveReceCmtListResponse(bool status)
    {
        Status = status;
    }

    public bool Status { get; private set; }
}
