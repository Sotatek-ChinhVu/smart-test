using Domain.Models.Receipt.Recalculation;
using Domain.Models.ReceSeikyu;
using Helper.Messaging;
using System.Text;

namespace Interactor.Receipt;

public interface ICommonReceRecalculation
{
    /// <summary>
    /// CheckErrorInMonth
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptIds"></param>
    /// <param name="sinYm"></param>
    /// <param name="userId"></param>
    /// <param name="receRecalculationList"></param>
    /// <param name="allCheckCount"></param>
    /// <param name="messenger"></param>
    /// <param name="recalculationStatus">RunReceCheck = 1, RunCalculationInMonth = 2, RunRecalculateInSeikyuPending = 3</param>
    /// <param name="isReceiptAggregationCheckBox"></param>
    /// <param name="isCheckErrorCheckBox"></param>
    /// <returns></returns>
    (bool Success, StringBuilder ErrorText) CheckErrorInMonth(int hpId, List<long> ptIds, int sinYm, int userId, List<ReceRecalculationModel> receRecalculationList, int allCheckCount, IMessenger messenger, int recalculationStatus = 1, bool isReceiptAggregationCheckBox = true, bool isCheckErrorCheckBox = true);

    string RecalculateInSeikyuPending(IMessenger messenger, int hpId, int userId, List<ReceInfo> receInfos);

    void ReleaseResource();
}

