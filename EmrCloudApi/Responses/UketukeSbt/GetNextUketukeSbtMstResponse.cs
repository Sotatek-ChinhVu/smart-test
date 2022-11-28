using Domain.Models.UketukeSbtMst;

namespace EmrCloudApi.Responses.UketukeSbt;

public class GetNextUketukeSbtMstResponse
{
    public GetNextUketukeSbtMstResponse(UketukeSbtMstModel? receptionType)
    {
        ReceptionType = receptionType;
    }

    public UketukeSbtMstModel? ReceptionType { get; private set; }
}
