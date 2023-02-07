using Domain.Models.Reception;
using UseCase.Reception.GetListRaiinInfs;

namespace EmrCloudApi.Tenant.Responses.Reception;

public class GetListRaiinInfResponse
{
    public GetListRaiinInfResponse(List<GetListRaiinInfsInputItem> raiinInfs)
    {
        RaiinInfs = raiinInfs;
    }

    public List<GetListRaiinInfsInputItem> RaiinInfs { get; private set; } = new List<GetListRaiinInfsInputItem>();
}
