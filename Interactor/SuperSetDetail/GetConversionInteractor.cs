using Domain.Models.MstItem;
using Domain.Models.SuperSetDetail;
using UseCase.SuperSetDetail.GetConversion;

namespace Interactor.SuperSetDetail;

public class GetConversionInteractor : IGetConversionInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public GetConversionInteractor(ISuperSetDetailRepository superSetDetailRepository, IMstItemRepository mstItemRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
        _mstItemRepository = mstItemRepository;
    }

    public GetConversionOutputData Handle(GetConversionInputData inputData)
    {
        try
        {
            var conversionCandidateItemList = GetConversionCandidateItem(inputData);
            var conversionSourceItem = GetConversionSourceItem(inputData);
            return new GetConversionOutputData(GetConversionStatus.Successed, conversionSourceItem, conversionCandidateItemList);
        }
        finally
        {
            _superSetDetailRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private List<ConversionItemModel> GetConversionCandidateItem(GetConversionInputData inputData)
    {
        var conversionItemInfList = _superSetDetailRepository.GetConversionItem(inputData.HpId, inputData.ItemCd, inputData.SinDate);
        var conversionCandidateItemList = conversionItemInfList.Select(item => new ConversionItemModel(
                                                                                   item.DestItemCd,
                                                                                   item.Name,
                                                                                   item.Quantity,
                                                                                   string.Empty,
                                                                                   string.Empty,
                                                                                   item.UnitName,
                                                                                   item.Ten,
                                                                                   item.HandanGrpKbn,
                                                                                   item.MasterSbt,
                                                                                   item.EndDate,
                                                                                   item.SortNo,
                                                                                   item.KensaItemCd,
                                                                                   item.KensaItemSeqNo,
                                                                                   item.IpnNameCd))
                                                               .ToList();
        foreach (var conversionItem in conversionCandidateItemList)
        {
            if (conversionItem.IsCommentMaster)
            {
                conversionItem.UpdateCmtName(conversionItem.ItemName, string.Empty);
            }
        }
        return conversionCandidateItemList;
    }

    private ConversionItemModel GetConversionSourceItem(GetConversionInputData inputData)
    {
        var tenMst = _mstItemRepository.GetTenMst(inputData.HpId, inputData.ItemCd, inputData.SinDate);
        var conversionItem = new ConversionItemModel(inputData.ItemCd, inputData.ItemName);
        if (conversionItem.IsCommentMaster)
        {
            conversionItem.UpdateCmtName(conversionItem.ItemName, string.Empty);
        }
        conversionItem.UpdateConversionItem(
                       inputData.Quantity,
                       inputData.UnitName,
                       tenMst?.Ten ?? 0,
                       tenMst?.HandanGrpKbn ?? 0,
                       tenMst?.EndDate ?? 0,
                       tenMst?.KensaItemCd ?? string.Empty,
                       tenMst?.KensaItemSeqNo ?? 0,
                       tenMst?.IpnNameCd ?? string.Empty);
        return conversionItem;
    }
}
