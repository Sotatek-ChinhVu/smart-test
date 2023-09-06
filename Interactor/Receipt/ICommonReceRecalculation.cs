using Domain.Models.Receipt.Recalculation;
using Helper.Messaging;

namespace Interactor.Receipt;

public interface ICommonReceRecalculation
{
    bool CheckErrorInMonth(int hpId, List<long> ptIds, int sinYm, int userId, List<ReceRecalculationModel> receRecalculationList, int allCheckCount, IMessenger messenger, bool receCheckCalculate = false, bool isReceiptAggregationCheckBox = true, bool isCheckErrorCheckBox = true);

    void ReleaseResource();
}

