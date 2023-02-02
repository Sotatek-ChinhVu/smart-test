using Domain.Common;

namespace Domain.Models.Receipt;

public interface IReceiptRepository : IRepositoryBase
{
    List<ReceiptListModel> GetData(int hpId, int seikyuYm, ReceiptListAdvancedSearchInput searchModel);
}
