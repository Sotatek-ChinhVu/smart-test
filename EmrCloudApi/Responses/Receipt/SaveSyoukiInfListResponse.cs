namespace EmrCloudApi.Responses.Receipt;

public class SaveSyoukiInfListResponse
{
    public SaveSyoukiInfListResponse(bool status)
    {
        Status = status;
    }

    public bool Status { get; private set; }
}
