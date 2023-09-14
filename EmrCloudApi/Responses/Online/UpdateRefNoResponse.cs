namespace EmrCloudApi.Responses.Online;

public class UpdateRefNoResponse
{
    public UpdateRefNoResponse(long nextRefNo)
    {
        NextRefNo = nextRefNo;
    }

    public long NextRefNo { get; private set; }
}
