using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceCheckOptionList;

public class GetReceCheckOptionListInputData : IInputData<GetReceCheckOptionListOutputData>
{
    public GetReceCheckOptionListInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
