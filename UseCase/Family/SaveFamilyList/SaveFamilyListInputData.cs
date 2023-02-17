using UseCase.Core.Sync.Core;

namespace UseCase.Family.SaveFamilyList;

public class SaveFamilyListInputData : IInputData<SaveFamilyListOutputData>
{
    public SaveFamilyListInputData(int hpId, int userId, long ptId, List<FamilyItem> listFamily)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        ListFamily = listFamily;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public List<FamilyItem> ListFamily { get; private set; }
}
