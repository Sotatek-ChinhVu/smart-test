using Reporting.ReceiptList.Model;

namespace Reporting.ReceiptList.DB;

public interface ICoReceiptListFinder
{
    List<ReceiptListModel> AdvancedSearchReceList(int hpId, int sinym);

    List<ReceiptListModel> GetDataReceReport(int hpId, int seikyuYm, List<ReceiptInputModel> receiptInputList);
}
