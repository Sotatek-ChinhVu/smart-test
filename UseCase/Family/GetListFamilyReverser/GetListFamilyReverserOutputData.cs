using UseCase.Core.Sync.Core;

namespace UseCase.Family.GetListFamilyReverser;

public class GetListFamilyReverserOutputData : IOutputData
{
    public GetListFamilyReverserOutputData(List<FamilyReverserOutputItem> listFamily, GetListFamilyReverserStatus status)
    {
        ListFamily = listFamily;
        Status = status;
    }
    
    public GetListFamilyReverserOutputData(GetListFamilyReverserStatus status)
    {
        ListFamily = new();
        Status = status;
    }

    public List<FamilyReverserOutputItem> ListFamily { get; private set; }

    public GetListFamilyReverserStatus Status { get; private set; }
}
