using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetListRaiinInfResponse
{
    public GetListRaiinInfResponse(List<RaiinInfItem> raiinInfList)
    {
        RaiinInfList = raiinInfList;
    }

    public List<RaiinInfItem> RaiinInfList { get; private set; }
}
