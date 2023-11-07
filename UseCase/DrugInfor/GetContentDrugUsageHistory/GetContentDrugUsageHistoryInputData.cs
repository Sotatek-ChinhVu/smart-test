using UseCase.Core.Sync.Core;

namespace UseCase.DrugInfor.GetContentDrugUsageHistory;

public class GetContentDrugUsageHistoryInputData : IInputData<GetContentDrugUsageHistoryOutputData>
{
    public GetContentDrugUsageHistoryInputData(int hpId, long ptId, int grpCd, int startDate, int endDate)
    {
        HpId = hpId;
        PtId = ptId;
        GrpCd = grpCd;
        StartDate = startDate;
        EndDate = endDate;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int GrpCd { get; private set; }

    public int StartDate { get; private set; }

    public int EndDate { get; private set; }
}
