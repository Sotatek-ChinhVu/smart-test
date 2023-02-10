using UseCase.Reception;
using UseCase.Reception.GetListRaiinInfs;

namespace EmrCloudApi.Tenant.Responses.Reception;

public class GetListRaiinInfResponse
{
    public GetListRaiinInfResponse(List<ReceptionGetDto> raiinInfs)
    {
        RaiinInfs = raiinInfs;
    }

    public List<ReceptionGetDto> RaiinInfs { get; private set; } = new List<ReceptionGetDto>();
}
