using UseCase.Core.Sync.Core;

namespace UseCase.Family.GetFamilyReverserList;

public class GetFamilyReverserListOutputData : IOutputData
{
    public GetFamilyReverserListOutputData(List<FamilyReverserItem> familyList, GetFamilyReverserListStatus status)
    {
        FamilyList = familyList;
        Status = status;
    }
    
    public GetFamilyReverserListOutputData(GetFamilyReverserListStatus status)
    {
        FamilyList = new();
        Status = status;
    }

    public List<FamilyReverserItem> FamilyList { get; private set; }

    public GetFamilyReverserListStatus Status { get; private set; }
}
