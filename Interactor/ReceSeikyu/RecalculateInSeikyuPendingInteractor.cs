using Domain.Models.ReceSeikyu;
using Helper.Messaging;
using Helper.Messaging.Data;
using Interactor.Receipt;
using UseCase.ReceSeikyu.RecalculateInSeikyuPending;

namespace Interactor.ReceSeikyu;

public class RecalculateInSeikyuPendingInteractor : IRecalculateInSeikyuPendingInputPort
{
    private readonly ICommonReceRecalculation _commonReceRecalculation;
    private IMessenger? _messenger;

    public RecalculateInSeikyuPendingInteractor(ICommonReceRecalculation commonReceRecalculation)
    {
        _commonReceRecalculation = commonReceRecalculation;
    }

    public RecalculateInSeikyuPendingOutputData Handle(RecalculateInSeikyuPendingInputData inputData)
    {
        string errorText = string.Empty;
        _messenger = inputData.Messenger;
        try
        {
            List<ReceInfo> receInfList = inputData.ReceInfList
                                                  .Where(item => item.PtId != 0 && item.SinYm != 0)
                                                  .Select(item => new ReceInfo(item.PtId, item.HokenId, item.SinYm, item.SinYm))
                                                  .Distinct()
                                                  .ToList();
            errorText = _commonReceRecalculation.RecalculateInSeikyuPending(_messenger, inputData.HpId, inputData.UserId, receInfList);
            _messenger.Send(new RecalculateInSeikyuPendingStatus(errorText, 100, true, true));
            return new RecalculateInSeikyuPendingOutputData(true);
        }
        finally
        {
            _commonReceRecalculation.ReleaseResource();
        }
    }
}
