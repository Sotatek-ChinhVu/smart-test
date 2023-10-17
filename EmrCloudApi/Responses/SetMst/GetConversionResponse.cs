using EmrCloudApi.Responses.SetMst.Dto;

namespace EmrCloudApi.Responses.SetMst;

public class GetConversionResponse
{
    public GetConversionResponse(ConversionItemDto conversionSourceItem, List<ConversionItemDto> conversionCandidateItemList)
    {
        ConversionSourceItem = conversionSourceItem;
        ConversionCandidateItemList = conversionCandidateItemList;
    }

    public ConversionItemDto ConversionSourceItem { get; private set; }

    public List<ConversionItemDto> ConversionCandidateItemList { get; private set; }
}
