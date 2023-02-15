using Domain.Common;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;

namespace Domain.Models.Receipt;

public interface IReceiptRepository : IRepositoryBase
{
    List<ReceiptListModel> GetListReceipt(int hpId, int seikyuYm, ReceiptListAdvancedSearchInput searchModel);

    List<ReceCmtModel> GetListReceCmt(int hpId, int sinYm, long ptId, int hokenId);

    bool SaveListReceCmt(int hpId, int userId, List<ReceCmtModel> receCmtList);

    List<SyoukiInfModel> GetListSyoukiInf(int hpId, int sinYm, long ptId, int hokenId);

    List<SyoukiKbnMstModel> GetListSyoukiKbnMst(int sinYm);

    bool CheckExistSyoukiKbn(int sinYm, List<SyoukiKbnMstModel> syoukiKbnList);

    bool SaveListSyoukiInf(int hpId, int userId, List<SyoukiInfModel> syoukiInfList);
}
