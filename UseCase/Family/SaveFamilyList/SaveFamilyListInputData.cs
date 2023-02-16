using UseCase.Core.Sync.Core;

namespace UseCase.Family.SaveListFamily;

public class SaveFamilyListInputData : IInputData<SaveFamilyListOutputData>
{
    public SaveFamilyListInputData(int hpId, int userId, long ptId, List<FamilyInputItem> listFamily)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        ListFamily = listFamily;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public List<FamilyInputItem> ListFamily { get; private set; }
}
