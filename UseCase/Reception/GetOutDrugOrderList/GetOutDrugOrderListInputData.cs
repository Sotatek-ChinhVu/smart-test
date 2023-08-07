using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetOutDrugOrderList;

public class GetOutDrugOrderListInputData : IInputData<GetOutDrugOrderListOutputData>
{
    public GetOutDrugOrderListInputData(int hpId, int fromDate, int toDate)
    {
        HpId = hpId;
        FromDate = fromDate;
        ToDate = toDate;
    }

    public int HpId { get; private set; }

    public int FromDate { get; private set; }

    public int ToDate { get; private set; }
}
