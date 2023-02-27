using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.GetSystemConfList;

public class GetSystemConfListInputData : IInputData<GetSystemConfListOutputData>
{
    public GetSystemConfListInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
