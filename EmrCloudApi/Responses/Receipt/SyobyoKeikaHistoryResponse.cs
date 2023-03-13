using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt.SyobyoKeikaHistory;

namespace EmrCloudApi.Responses.Receipt;

public class SyobyoKeikaHistoryResponse
{
    public SyobyoKeikaHistoryResponse(List<SyobyoKeikaHistoryOutputItem> output)
    {
        Data = output.Select(item => new SyobyoKeikaHistoryDto(item)).ToList();
    }

    public List<SyobyoKeikaHistoryDto> Data { get; private set; }
}
