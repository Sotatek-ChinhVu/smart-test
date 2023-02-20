using Domain.Models.Receipt;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using Helper.Enum;
using UseCase.Receipt.ReceiptListAdvancedSearch;

namespace Interactor.Receipt;

public class ReceiptListAdvancedSearchInteractor : IReceiptListAdvancedSearchInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public ReceiptListAdvancedSearchInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public ReceiptListAdvancedSearchOutputData Handle(ReceiptListAdvancedSearchInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetReceiptList(inputData.HpId, inputData.SeikyuYm, ConvertToInputAdvancedSearch(inputData));
            return new ReceiptListAdvancedSearchOutputData(result, ReceiptListAdvancedSearchStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }

    private ReceiptListAdvancedSearchInput ConvertToInputAdvancedSearch(ReceiptListAdvancedSearchInputData inputData)
    {
        var itemList = inputData.ItemList.Select(item => new ItemSearchModel(
                                                                                item.ItemCd,
                                                                                item.InputName,
                                                                                item.RangeSeach,
                                                                                item.Amount,
                                                                                item.OrderStatus,
                                                                                item.IsComment
                                                                            ))
                                                                            .ToList();

        var byomeiCdList = inputData.ByomeiCdList.Select(item => new SearchByoMstModel(
                                                                                            item.ByomeiCd,
                                                                                            item.InputName,
                                                                                            item.IsComment
                                                                                       ))
                                                                                       .ToList();

        return new ReceiptListAdvancedSearchInput(
                                                    inputData.IsAdvanceSearch,
                                                    inputData.Tokki,
                                                    inputData.HokenSbts,
                                                    inputData.IsAll,
                                                    inputData.IsNoSetting,
                                                    inputData.IsSystemSave,
                                                    inputData.IsSave1,
                                                    inputData.IsSave2,
                                                    inputData.IsSave3,
                                                    inputData.IsTempSave,
                                                    inputData.IsDone,
                                                    inputData.ReceSbtCenter,
                                                    inputData.ReceSbtRight,
                                                    inputData.HokenHoubetu,
                                                    inputData.Kohi1Houbetu,
                                                    inputData.Kohi2Houbetu,
                                                    inputData.Kohi3Houbetu,
                                                    inputData.Kohi4Houbetu,
                                                    inputData.IsIncludeSingle,
                                                    inputData.HokensyaNoFrom,
                                                    inputData.HokensyaNoTo,
                                                    inputData.HokensyaNoFromLong,
                                                    inputData.HokensyaNoToLong,
                                                    inputData.PtId,
                                                    inputData.PtIdFrom,
                                                    inputData.PtIdTo,
                                                    (PtIdSearchOptionEnum)inputData.PtSearchOption,
                                                    inputData.TensuFrom,
                                                    inputData.TensuTo,
                                                    inputData.LastRaiinDateFrom,
                                                    inputData.LastRaiinDateTo,
                                                    inputData.BirthDayFrom,
                                                    inputData.BirthDayTo,
                                                    itemList,
                                                    (QuerySearchEnum)inputData.ItemQuery,
                                                    inputData.IsOnlySuspectedDisease,
                                                    (QuerySearchEnum)inputData.ByomeiQuery,
                                                    byomeiCdList,
                                                    inputData.IsFutanIncludeSingle,
                                                    inputData.FutansyaNoFromLong,
                                                    inputData.FutansyaNoToLong,
                                                    inputData.KaId,
                                                    inputData.DoctorId,
                                                    inputData.Name,
                                                    inputData.IsTestPatientSearch,
                                                    inputData.IsNotDisplayPrinted,
                                                    inputData.GroupSearchModels,
                                                    inputData.SeikyuKbnAll,
                                                    inputData.SeikyuKbnDenshi,
                                                    inputData.SeikyuKbnPaper
                                                );
    }
}
