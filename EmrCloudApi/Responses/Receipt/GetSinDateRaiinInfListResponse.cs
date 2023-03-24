namespace EmrCloudApi.Responses.Receipt;

public class GetSinDateRaiinInfListResponse
{
    public GetSinDateRaiinInfListResponse(List<int> sinDateList)
    {
        SinDateList = sinDateList;
    }

    public List<int> SinDateList { get; private set; }
}
