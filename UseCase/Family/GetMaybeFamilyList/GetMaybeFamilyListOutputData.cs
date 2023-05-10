using UseCase.Core.Sync.Core;

namespace UseCase.Family.GetMaybeFamilyList;

public class GetMaybeFamilyListOutputData : IOutputData
{
    public GetMaybeFamilyListOutputData(List<FamilyItem> familyList, GetMaybeFamilyListStatus status)
    {
        FamilyList = familyList;
        Status = status;
    }

    public GetMaybeFamilyListOutputData(GetMaybeFamilyListStatus status)
    {
        FamilyList = new();
        Status = status;
    }

    public List<FamilyItem> FamilyList { get; private set; }

    public GetMaybeFamilyListStatus Status { get; private set; }
}
