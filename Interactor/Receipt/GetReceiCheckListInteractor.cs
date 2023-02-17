using Domain.Models.Receipt;
using UseCase.Receipt;
using UseCase.Receipt.GetReceiCheckList;

namespace Interactor.Receipt;

public class GetReceiCheckListInteractor : IGetReceiCheckListInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetReceiCheckListInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetReceiCheckListOutputData Handle(GetReceiCheckListInputData inputData)
    {
        try
        {
            var receCheckCmtList = _receiptRepository.GetReceCheckCmtList(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
            var receCheckErrList = _receiptRepository.GetReceCheckErrList(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
            var result = ConvertToResultData(receCheckCmtList, receCheckErrList);
            return new GetReceiCheckListOutputData(result, GetReceiCheckListStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }

    private List<ReceiptCheckCmtErrListItem> ConvertToResultData(List<ReceCheckCmtModel> receCheckCmtList, List<ReceCheckErrModel> receCheckErrList)
    {
        List<ReceiptCheckCmtErrListItem> result = new();

        // Convert receCheckCmtModel to receCheckCmtOutput
        foreach (var cmt in receCheckCmtList)
        {
            var receCheckCmtItem = new ReceiptCheckCmtErrListItem(
                                        cmt.IsPending,
                                        cmt.SortNo,
                                        cmt.IsChecked == 1,
                                        cmt.Cmt
                                       );
            result.Add(receCheckCmtItem);
        }

        // Convert receCheckErrModel to receCheckErrOutput
        foreach (var err in receCheckErrList)
        {
            var receCheckErrItem = new ReceiptCheckCmtErrListItem(
                                        err.IsChecked == 1,
                                        err.Message1,
                                        err.Message2
                                       );
            result.Add(receCheckErrItem);
        }

        return result;
    }
}
