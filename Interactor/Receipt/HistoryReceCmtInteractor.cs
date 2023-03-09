using Domain.Models.Receipt;
using UseCase.Receipt.HistoryReceCmt;

namespace Interactor.Receipt;

public class HistoryReceCmtInteractor : IHistoryReceCmtInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    public HistoryReceCmtOutputData Handle(HistoryReceCmtInputData inputData)
    {
        throw new NotImplementedException();
    }
}
