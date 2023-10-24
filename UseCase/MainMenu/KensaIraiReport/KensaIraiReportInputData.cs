using Domain.Models.KensaIrai;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.KensaIraiReport;

public class KensaIraiReportInputData : IInputData<KensaIraiReportOutputData>
{
    public KensaIraiReportInputData(int hpId, int userId, string centerCd, int systemDate, int fromDate, int toDate, List<KensaIraiModel> kensaIraiList)
    {
        HpId = hpId;
        UserId = userId;
        CenterCd = centerCd;
        SystemDate = systemDate;
        FromDate = fromDate;
        ToDate = toDate;
        KensaIraiList = kensaIraiList;
    }
    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public string CenterCd { get; private set; }

    public int SystemDate { get; private set; }

    public int FromDate { get; private set; }

    public int ToDate { get; private set; }

    public List<KensaIraiModel> KensaIraiList { get; private set; }
}
