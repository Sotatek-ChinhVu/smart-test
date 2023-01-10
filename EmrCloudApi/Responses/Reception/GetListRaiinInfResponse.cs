using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Responses.Reception;

public class GetListRaiinInfResponse
{
    public GetListRaiinInfResponse(List<ReceptionModel> raiinInfs)
    {
        RaiinInfs = raiinInfs;
    }

    public List<ReceptionModel> RaiinInfs { get; private set; } = new List<ReceptionModel>();
}
