using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt.HistoryReceCmt;

namespace EmrCloudApi.Responses.Receipt;

public class HistoryReceCmtResponse
{
    public HistoryReceCmtResponse(List<HistoryReceCmtOutputItem> output)
    {
        Data = output.Select(item => new HistoryReceCmtDto(item)).ToList();
    }

    public List<HistoryReceCmtDto> Data { get; private set; }
}
