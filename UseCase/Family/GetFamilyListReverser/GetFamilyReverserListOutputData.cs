using UseCase.Core.Sync.Core;

namespace UseCase.Family.GetListFamilyReverser;

public class GetFamilyReverserListOutputData : IOutputData
{
    public GetFamilyReverserListOutputData(List<FamilyReverserOutputItem> familyList, GetFamilyReverserListStatus status)
    {
        FamilyList = familyList;
        Status = status;
    }
    
    public GetFamilyReverserListOutputData(GetFamilyReverserListStatus status)
    {
        FamilyList = new();
        Status = status;
    }

    public List<FamilyReverserOutputItem> FamilyList { get; private set; }

    public GetFamilyReverserListStatus Status { get; private set; }
}
