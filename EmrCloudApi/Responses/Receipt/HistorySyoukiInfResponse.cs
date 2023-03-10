using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt.HistorySyoukiInf;

namespace EmrCloudApi.Responses.Receipt;

public class HistorySyoukiInfResponse
{
    public HistorySyoukiInfResponse(List<HistorySyoukiInfOutputItem> output)
    {
        Data = output.Select(item => new HistorySyoukiInfDto(item)).ToList();
    }

    public List<HistorySyoukiInfDto> Data { get; private set; }
}
