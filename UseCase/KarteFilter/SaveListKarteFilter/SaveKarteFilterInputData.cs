using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.SaveListKarteFilter;

public class SaveKarteFilterInputData : IInputData<SaveKarteFilterOutputData>
{
    public SaveKarteFilterInputData(List<SaveKarteFilterMstInputItem> saveKarteFilterMstModelInputItems, int hpId, int userId)
    {
        SaveKarteFilterMstModelInputs = saveKarteFilterMstModelInputItems;
        HpId = hpId;
        UserId = userId;
    }
    public List<SaveKarteFilterMstInputItem> SaveKarteFilterMstModelInputs { get; private set; }
    public int HpId { get; private set; }
    public int UserId { get; private set; }
}
