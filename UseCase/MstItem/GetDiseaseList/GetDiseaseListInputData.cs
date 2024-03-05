using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetDiseaseList;

public class GetDiseaseListInputData : IInputData<GetDiseaseListOutputData>
{
    public GetDiseaseListInputData(int hpId, List<string> itemCdList)
    {
        HpId = hpId;
        ItemCdList = itemCdList;
    }

    public int HpId { get; private set; }
    public List<string> ItemCdList { get; private set; }
}
