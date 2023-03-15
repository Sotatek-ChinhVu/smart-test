using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetRecePreviewListResponse
{
    public GetRecePreviewListResponse(List<ReceInfItem> recePreviewList)
    {
        RecePreviewList = recePreviewList.Select(item => new RecePreviewDto(item)).ToList();
    }

    public List<RecePreviewDto> RecePreviewList { get;private set; }
}
