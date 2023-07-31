namespace EmrCloudApi.Responses.ReceSeikyu;

public class CancelSeikyuResponse
{
    public CancelSeikyuResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
