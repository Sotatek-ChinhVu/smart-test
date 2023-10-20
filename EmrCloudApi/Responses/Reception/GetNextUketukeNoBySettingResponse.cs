namespace EmrCloudApi.Responses.Reception;

public class GetNextUketukeNoBySettingResponse
{
    public GetNextUketukeNoBySettingResponse(int nextUketukeNo)
    {
        NextUketukeNo = nextUketukeNo;
    }

    public int NextUketukeNo { get; private set; }
}
