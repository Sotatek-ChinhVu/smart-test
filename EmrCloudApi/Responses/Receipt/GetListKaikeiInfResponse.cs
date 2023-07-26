using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetListKaikeiInfResponse
{
    public GetListKaikeiInfResponse(List<PtHokenInfKaikeiItem> ptHokenInfKaikeiList)
    {
        PtHokenInfKaikeiList = ptHokenInfKaikeiList.Select(item => new PtHokenInfKaikeiDto(item)).ToList();
    }

    public List<PtHokenInfKaikeiDto> PtHokenInfKaikeiList { get; private set; }
}
