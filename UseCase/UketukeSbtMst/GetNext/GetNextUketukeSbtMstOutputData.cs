using Domain.Models.UketukeSbtMst;
using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtMst.GetNext;

public class GetNextUketukeSbtMstOutputData : IOutputData
{
    public GetNextUketukeSbtMstOutputData(GetNextUketukeSbtMstStatus status)
    {
        Status = status;
    }

    public GetNextUketukeSbtMstOutputData(GetNextUketukeSbtMstStatus status, UketukeSbtMstModel? receptionType)
    {
        Status = status;
        ReceptionType = receptionType;
    }

    public GetNextUketukeSbtMstStatus Status { get; private set; }
    public UketukeSbtMstModel? ReceptionType { get; private set; }
}
