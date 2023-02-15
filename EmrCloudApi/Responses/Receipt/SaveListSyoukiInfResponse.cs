namespace EmrCloudApi.Responses.Receipt;

public class SaveListSyoukiInfResponse
{
    public SaveListSyoukiInfResponse(bool status)
    {
        Status = status;
    }

    public bool Status { get; private set; }
}
