using UseCase.Reception.GetListRaiinInfs;

namespace EmrCloudApi.Tenant.Responses.Reception;

public class GetListRaiinInfResponse
{
    public GetListRaiinInfResponse(List<GetListRaiinInfInputItem> raiinInfs)
    {
        RaiinInfs = raiinInfs;
    }

    public List<GetListRaiinInfInputItem> RaiinInfs { get; private set; } = new List<GetListRaiinInfInputItem>();
}
