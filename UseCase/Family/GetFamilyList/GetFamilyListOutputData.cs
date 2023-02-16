using UseCase.Core.Sync.Core;

namespace UseCase.Family.GetListFamily;

public class GetFamilyListOutputData : IOutputData
{
    public GetFamilyListOutputData(List<FamilyOutputItem> familyList, GetFamilyListStatus status)
    {
        FamilyList = familyList;
        Status = status;
    }

    public GetFamilyListOutputData(GetFamilyListStatus status)
    {
        FamilyList = new();
        Status = status;
    }

    public List<FamilyOutputItem> FamilyList { get; private set; }

    public GetFamilyListStatus Status { get; private set; }
}
