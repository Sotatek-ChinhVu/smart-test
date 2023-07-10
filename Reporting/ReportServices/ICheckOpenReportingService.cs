﻿using Domain.Common;
using Reporting.Accounting.Model;
using Reporting.CommonMasters.Enums;

namespace Reporting.ReportServices;

public interface ICheckOpenReportingService : IRepositoryBase
{
    bool CheckOpenAccountingForm(int hpId, ConfirmationMode mode, long ptId, List<CoAccountDueListModel> multiAccountDueListModels, bool isPrintMonth, bool ryoshusho, bool meisai);

    bool CheckOpenAccountingForm(int hpId, long ptId, int printTypeInput, List<long> raiinNoList, List<long> raiinNoPayList, bool isCalculateProcess = false);

    bool ExistedReceInfs(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId);
}
