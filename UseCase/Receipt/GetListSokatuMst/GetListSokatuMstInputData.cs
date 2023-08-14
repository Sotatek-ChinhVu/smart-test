using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetListSokatuMst;

public class GetListSokatuMstInputData : IInputData<GetListSokatuMstOutputData>
{
    public GetListSokatuMstInputData(int hpId, int seikyuYm)
    {
        HpId = hpId;
        SeikyuYm = seikyuYm;
    }

    public int HpId { get; private set; }

    public int SeikyuYm { get; private set; }
}
