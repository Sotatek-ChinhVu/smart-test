﻿using Domain.Common;

namespace Domain.Models.Receipt;

public interface IReceiptRepository : IRepositoryBase
{
    List<ReceiptListModel> GetListReceipt(int hpId, int seikyuYm, ReceiptListAdvancedSearchInput searchModel);

    List<ReceCmtModel> GetListReceCmt(int hpId, int sinDate, long ptId, int hokenId);
}
