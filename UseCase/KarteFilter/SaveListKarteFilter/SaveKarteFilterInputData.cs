using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.SaveListKarteFilter;

public class SaveKarteFilterInputData : IInputData<SaveKarteFilterOutputData>
{
    public SaveKarteFilterInputData(int hpId, int userId, List<SaveKarteFilterMstInputItem> saveKarteFilterMstModelInputs)
    {
        HpId = hpId;
        UserId = userId;
        SaveKarteFilterMstModelInputs = saveKarteFilterMstModelInputs;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<SaveKarteFilterMstInputItem> SaveKarteFilterMstModelInputs { get; private set; }
}
