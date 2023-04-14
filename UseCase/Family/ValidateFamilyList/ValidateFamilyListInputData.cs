using UseCase.Core.Sync.Core;

namespace UseCase.Family.ValidateFamilyList;

public class ValidateFamilyListInputData : IInputData<ValidateFamilyListOutputData>
{
    public ValidateFamilyListInputData(int hpId, long ptId, List<FamilyItem> listFamily)
    {
        HpId = hpId;
        PtId = ptId;
        ListFamily = listFamily;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public List<FamilyItem> ListFamily { get; private set; }
}
