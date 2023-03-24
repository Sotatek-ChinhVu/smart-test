using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt.ReceCmtHistory;

namespace EmrCloudApi.Responses.Receipt;

public class ReceCmtHistoryResponse
{
    public ReceCmtHistoryResponse(List<ReceCmtHistoryOutputItem> output)
    {
        Data = output.Select(item => new ReceCmtHistoryDto(item)).ToList();
    }

    public List<ReceCmtHistoryDto> Data { get; private set; }
}
