using Domain.Models.Receipt.Recalculation;

namespace Interactor.Receipt;

public interface ICommonReceRecalculation
{
    bool CheckErrorInMonth(int hpId, List<long> ptIds, int sinYm, int userId, List<ReceRecalculationModel> receRecalculationList, int allCheckCount, bool receCheckCalculate = false, bool isReceiptAggregationCheckBox = true);
    
    void ReleaseResource();
}

