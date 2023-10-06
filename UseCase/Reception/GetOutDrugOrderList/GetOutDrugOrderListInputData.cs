using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetOutDrugOrderList;

public class GetOutDrugOrderListInputData : IInputData<GetOutDrugOrderListOutputData>
{
    public GetOutDrugOrderListInputData(int hpId, bool isPrintPrescription, bool isPrintAccountingCard, int fromDate, int toDate, int sinDate)
    {
        HpId = hpId;
        IsPrintPrescription = isPrintPrescription;
        IsPrintAccountingCard = isPrintAccountingCard;
        FromDate = fromDate;
        ToDate = toDate;
        SinDate = sinDate;
    }

    public int HpId { get; private set; }

    public bool IsPrintPrescription { get; private set; }

    public bool IsPrintAccountingCard { get; private set; }

    public int FromDate { get; private set; }

    public int ToDate { get; private set; }

    public int SinDate { get; private set; }
}
