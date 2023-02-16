using UseCase.Core.Sync.Core;

namespace UseCase.Family.GetFamilyList;

public class GetFamilyListOutputData : IOutputData
{
    public GetFamilyListOutputData(List<FamilyItem> familyList, GetFamilyListStatus status)
    {
        FamilyList = familyList;
        Status = status;
    }

    public GetFamilyListOutputData(GetFamilyListStatus status)
    {
        FamilyList = new();
        Status = status;
    }

    public List<FamilyItem> FamilyList { get; private set; }

    public GetFamilyListStatus Status { get; private set; }
}
