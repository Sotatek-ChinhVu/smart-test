using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.SaveListKarteFilter;

public class SaveKarteFilterInputData : IInputData<SaveKarteFilterOutputData>
{
    public SaveKarteFilterInputData(List<SaveKarteFilterMstModelInputItem>? saveKarteFilterMstModelInputItems)
    {
        this.saveKarteFilterMstModelInputs = saveKarteFilterMstModelInputItems;
    }
    public List<SaveKarteFilterMstModelInputItem>? saveKarteFilterMstModelInputs { get; private set; }
}
