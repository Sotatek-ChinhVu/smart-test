using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Responses.Reception;

public class GetListRaiinInfResponse
{
    public GetListRaiinInfResponse(List<ReceptionModel> raiinInfs)
    {
        RaiinInf = raiinInfs;
    }

    public List<ReceptionModel> RaiinInf { get; private set; } = new List<ReceptionModel>();
}
