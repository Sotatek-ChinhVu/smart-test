using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt.SyoukiInfHistory;

namespace EmrCloudApi.Responses.Receipt;

public class SyoukiInfHistoryResponse
{
    public SyoukiInfHistoryResponse(List<SyoukiInfHistoryOutputItem> output)
    {
        Data = output.Select(item => new SyoukiInfHistoryDto(item)).ToList();
    }

    public List<SyoukiInfHistoryDto> Data { get; private set; }
}
