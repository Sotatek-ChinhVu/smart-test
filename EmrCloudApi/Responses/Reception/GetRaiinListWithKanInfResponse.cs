using UseCase.Reception.GetRaiinListWithKanInf;

namespace EmrCloudApi.Responses.Reception;

public class GetRaiinListWithKanInfResponse
{
    public GetRaiinListWithKanInfResponse(List<RaiinInfItem> raiinInfList)
    {
        RaiinInfList = raiinInfList;
    }

    public List<RaiinInfItem> RaiinInfList { get;private set; }
}
