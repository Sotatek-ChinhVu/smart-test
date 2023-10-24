using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenMstList;

public class GetTenMstListInputData : IInputData<GetTenMstListOutputData>
{
    public GetTenMstListInputData(int hpId, int sinDate, List<string> itemCdList)
    {
        HpId = hpId;
        SinDate = sinDate;
        ItemCdList = itemCdList;
    }

    public int HpId { get; private set; }

    public int SinDate { get; private set; }

    public List<string> ItemCdList { get; private set; }
}
