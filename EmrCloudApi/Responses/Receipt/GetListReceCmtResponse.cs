using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetListReceCmtResponse
{
    public GetListReceCmtResponse(List<ReceCmtItem> listReceCmt)
    {
        ListReceCmt = listReceCmt.Select(item => new ReceCmtDto(item)).ToList();
    }

    public List<ReceCmtDto> ListReceCmt { get; private set; }
}
