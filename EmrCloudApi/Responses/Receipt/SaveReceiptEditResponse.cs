namespace EmrCloudApi.Responses.Receipt;

public class SaveReceiptEditResponse
{
    public SaveReceiptEditResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
