using UseCase.Core.Sync.Core;

namespace UseCase.Family.GetListFamily;

public class GetListFamilyOutputData : IOutputData
{
    public GetListFamilyOutputData(List<FamilyOutputItem> listFamily, GetListFamilyStatus status)
    {
        ListFamily = listFamily;
        Status = status;
    }

    public GetListFamilyOutputData(GetListFamilyStatus status)
    {
        ListFamily = new();
        Status = status;
    }

    public List<FamilyOutputItem> ListFamily { get; private set; }

    public GetListFamilyStatus Status { get; private set; }
}
