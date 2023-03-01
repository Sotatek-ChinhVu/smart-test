namespace EmrCloudApi.Responses.Receipt;

public class SaveReceCheckCmtListResponse
{
    public SaveReceCheckCmtListResponse(bool status)
    {
        Status = status;
    }

    public bool Status { get; private set; }
}
