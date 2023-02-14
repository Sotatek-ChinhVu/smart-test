using Domain.Common;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;

namespace Domain.Models.Receipt;

public interface IReceiptRepository : IRepositoryBase
{
    List<ReceiptListModel> GetData(int hpId, int seikyuYm, ReceiptListAdvancedSearchInput searchModel);
}
