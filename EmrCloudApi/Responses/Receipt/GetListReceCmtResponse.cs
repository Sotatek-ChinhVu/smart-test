using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetListReceCmtResponse
{
    public GetListReceCmtResponse(List<ReceCmtItem> receCmtList)
    {
        ReceCmtList = receCmtList.Select(item => new ReceCmtDto(item)).ToList();
    }

    public List<ReceCmtDto> ReceCmtList { get; private set; }
}
