using Domain.Common;
using Reporting.ReceiptList.Model;

namespace Reporting.ReceiptList.DB;

public interface ICoReceiptListFinder : IRepositoryBase
{
    List<ReceiptListModel> AdvancedSearchReceList(int hpId, int sinym);

    List<ReceiptListModel> GetDataReceReport(int hpId, int seikyuYm, List<ReceiptInputModel> receiptInputList);
}
