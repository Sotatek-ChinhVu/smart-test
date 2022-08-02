using Domain.Models.UketukeSbtMst;

namespace EmrCloudApi.Tenant.Responses.UketukeSbt;

public class GetUketukeSbtMstBySinDateResponse
{
    public GetUketukeSbtMstBySinDateResponse(UketukeSbtMstModel? receptionType)
    {
        ReceptionType = receptionType;
    }

    public UketukeSbtMstModel? ReceptionType { get; private set; }
}
