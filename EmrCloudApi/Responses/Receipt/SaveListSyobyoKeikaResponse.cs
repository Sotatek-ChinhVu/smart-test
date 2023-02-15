namespace EmrCloudApi.Responses.Receipt;

public class SaveListSyobyoKeikaResponse
{
    public SaveListSyobyoKeikaResponse(bool status)
    {
        Status = status;
    }

    public bool Status { get; private set; }
}
