using Domain.Models.UketukeSbtMst;
using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtMst.GetBySinDate;

public class GetUketukeSbtMstBySinDateOutputData : IOutputData
{
    public GetUketukeSbtMstBySinDateOutputData(GetUketukeSbtMstBySinDateStatus status)
    {
        Status = status;
    }

    public GetUketukeSbtMstBySinDateOutputData(GetUketukeSbtMstBySinDateStatus status, UketukeSbtMstModel? receptionType)
    {
        Status = status;
        ReceptionType = receptionType;
    }

    public GetUketukeSbtMstBySinDateStatus Status { get; private set; }
    public UketukeSbtMstModel? ReceptionType { get; private set; }
}
