using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.GetSystemConfList;

public class GetSystemConfListInputData : IInputData<GetSystemConfListOutputData>
{
    public GetSystemConfListInputData(int hpId, List<GetSystemConfListInputItem> grpItemList)
    {
        HpId = hpId;
        GrpItemList = grpItemList;
    }

    public int HpId { get; private set; }

    public List<GetSystemConfListInputItem> GrpItemList { get; private set; }
}
