using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.GetConversion;

public class GetConversionOutputData : IOutputData
{
    public GetConversionOutputData(GetConversionStatus status, ConversionItemModel conversionSourceItem, List<ConversionItemModel> conversionCandidateItemList)
    {
        Status = status;
        ConversionSourceItem = conversionSourceItem;
        ConversionCandidateItemList = conversionCandidateItemList;
    }

    public GetConversionStatus Status { get; private set; }

    public ConversionItemModel ConversionSourceItem { get; private set; }

    public List<ConversionItemModel> ConversionCandidateItemList { get; private set; }
}
