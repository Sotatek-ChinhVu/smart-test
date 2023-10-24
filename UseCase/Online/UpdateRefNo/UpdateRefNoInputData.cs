using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateRefNo;

public class UpdateRefNoInputData : IInputData<UpdateRefNoOutputData>
{
    public UpdateRefNoInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }
}
