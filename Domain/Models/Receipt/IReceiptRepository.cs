using Domain.Common;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;

namespace Domain.Models.Receipt;

public interface IReceiptRepository : IRepositoryBase
{
    List<ReceiptListModel> GetListReceipt(int hpId, int seikyuYm, ReceiptListAdvancedSearchInput searchModel);

    List<ReceCmtModel> GetListReceCmt(int hpId, int sinYm, long ptId, int hokenId);
}
