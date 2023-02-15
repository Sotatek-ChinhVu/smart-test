namespace EmrCloudApi.Responses.Receipt;

public class SaveListReceCmtResponse
{
    public SaveListReceCmtResponse(bool status)
    {
        Status = status;
    }

    public bool Status { get; private set; }
}
