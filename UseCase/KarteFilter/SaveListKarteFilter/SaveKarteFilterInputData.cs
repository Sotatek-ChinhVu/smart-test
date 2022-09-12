using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.SaveListKarteFilter;

public class SaveKarteFilterInputData : IInputData<SaveKarteFilterOutputData>
{
    public SaveKarteFilterInputData(List<SaveKarteFilterMstInputItem> saveKarteFilterMstModelInputItems)
    {
        SaveKarteFilterMstModelInputs = saveKarteFilterMstModelInputItems;
    }
    public List<SaveKarteFilterMstInputItem> SaveKarteFilterMstModelInputs { get; private set; }
}
