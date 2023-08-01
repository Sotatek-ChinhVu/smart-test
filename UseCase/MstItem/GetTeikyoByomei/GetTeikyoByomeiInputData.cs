using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTeikyoByomei;

public class GetTeikyoByomeiInputData : IInputData<GetTeikyoByomeiOutputData>
{
    public GetTeikyoByomeiInputData(int hpId, string itemCd)
    {
        HpId = hpId;
        ItemCd = itemCd;
    }

    public int HpId { get;private set; }

    public string ItemCd { get;private set; }
}
