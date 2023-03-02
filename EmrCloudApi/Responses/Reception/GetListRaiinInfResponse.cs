using UseCase.Reception.GetListRaiinInf;

namespace EmrCloudApi.Tenant.Responses.Reception;

public class GetListRaiinInfResponse
{
    public GetListRaiinInfResponse(List<GetListRaiinInfOutputItem> raiinInfs)
    {
        RaiinInfs = raiinInfs;
    }

    public List<GetListRaiinInfOutputItem> RaiinInfs { get; private set; } = new();
}
