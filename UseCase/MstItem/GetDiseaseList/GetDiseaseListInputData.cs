using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetDiseaseList;

public class GetDiseaseListInputData : IInputData<GetDiseaseListOutputData>
{
    public GetDiseaseListInputData(List<string> itemCdList)
    {
        ItemCdList = itemCdList;
    }

    public List<string> ItemCdList { get; private set; }
}
