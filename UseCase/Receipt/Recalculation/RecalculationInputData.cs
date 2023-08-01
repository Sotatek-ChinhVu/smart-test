using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.Recalculation;

public class RecalculationInputData : IInputData<RecalculationOutputData>
{
    public RecalculationInputData(int hpId, int userId, int sinYm, List<long> ptIdList, bool isRecalculationCheckBox, bool isReceiptAggregationCheckBox, bool isCheckErrorCheckBox, string hostName, string uniqueKey, CancellationToken cancellationToken)
    {
        HpId = hpId;
        UserId = userId;
        SinYm = sinYm;
        PtIdList = ptIdList;
        IsRecalculationCheckBox = isRecalculationCheckBox;
        IsReceiptAggregationCheckBox = isReceiptAggregationCheckBox;
        IsCheckErrorCheckBox = isCheckErrorCheckBox;
        HostName = hostName;
        UniqueKey = uniqueKey;
        CancellationToken = cancellationToken;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int SinYm { get; private set; }

    public List<long> PtIdList { get; private set; }

    public bool IsRecalculationCheckBox { get; private set; }

    public bool IsReceiptAggregationCheckBox { get; private set; }

    public bool IsCheckErrorCheckBox { get; private set; }

    public string HostName { get; private set; }

    public string UniqueKey { get; private set; }

    public CancellationToken CancellationToken { get; private set; }
}
