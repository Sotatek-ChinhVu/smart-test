using Domain.Models.UketukeSbtMst;

namespace EmrCloudApi.Tenant.Responses.UketukeSbt;

public class GetUketukeSbtMstListResponse
{
    public GetUketukeSbtMstListResponse(List<UketukeSbtMstModel> receptionTypes)
    {
        ReceptionTypes = receptionTypes;
    }

    public List<UketukeSbtMstModel> ReceptionTypes { get; private set; }
}
