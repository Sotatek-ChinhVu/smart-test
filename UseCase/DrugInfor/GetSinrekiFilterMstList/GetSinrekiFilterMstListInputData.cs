using UseCase.Core.Sync.Core;

namespace UseCase.DrugInfor.GetSinrekiFilterMstList;

public class GetSinrekiFilterMstListInputData : IInputData<GetSinrekiFilterMstListOutputData>
{
    public GetSinrekiFilterMstListInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
