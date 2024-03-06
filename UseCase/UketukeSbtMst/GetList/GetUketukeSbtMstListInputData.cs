using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtMst.GetList;

public class GetUketukeSbtMstListInputData : IInputData<GetUketukeSbtMstListOutputData>
{
    public GetUketukeSbtMstListInputData(int hpId)
    {
        HpId = hpId;
    }
    public int HpId { get; private set; }
}
