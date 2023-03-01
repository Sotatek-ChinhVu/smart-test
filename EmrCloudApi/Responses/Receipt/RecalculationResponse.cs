namespace EmrCloudApi.Responses.Receipt;

public class RecalculationResponse
{
    public RecalculationResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public string ErrorMessage { get;private set; }
}
