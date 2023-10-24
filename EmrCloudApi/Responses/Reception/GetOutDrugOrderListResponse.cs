namespace EmrCloudApi.Responses.Reception;

public class GetOutDrugOrderListResponse
{
    public GetOutDrugOrderListResponse(List<RaiinInfToPrintDto> raiinInfToPrintList)
    {
        RaiinInfToPrintList = raiinInfToPrintList;
    }

    public List<RaiinInfToPrintDto> RaiinInfToPrintList { get; private set; }
}
