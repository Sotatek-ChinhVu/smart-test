using Domain.Models.Receipt.Recalculation;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceCheckOptionList;

public class GetReceCheckOptionListOutputData : IOutputData
{
    public GetReceCheckOptionListOutputData(Dictionary<string, ReceCheckOptModel> receCheckOptionData, GetReceCheckOptionListStatus status)
    {
        ReceCheckOptionData = receCheckOptionData;
        Status = status;
    }

    public Dictionary<string, ReceCheckOptModel> ReceCheckOptionData { get; private set; }

    public GetReceCheckOptionListStatus Status { get; private set; }
}
