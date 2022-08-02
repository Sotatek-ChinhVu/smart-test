using Domain.Models.UketukeSbtMst;
using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtMst.GetList;

public class GetUketukeSbtMstListOutputData : IOutputData
{
    public GetUketukeSbtMstListOutputData(GetUketukeSbtMstListStatus status, List<UketukeSbtMstModel> receptionTypes)
    {
        Status = status;
        ReceptionTypes = receptionTypes;
    }

    public GetUketukeSbtMstListStatus Status { get; private set; }
    public List<UketukeSbtMstModel> ReceptionTypes { get; private set; }
}
