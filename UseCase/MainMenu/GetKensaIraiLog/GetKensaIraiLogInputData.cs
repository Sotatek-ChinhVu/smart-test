using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetKensaIraiLog;

public class GetKensaIraiLogInputData : IInputData<GetKensaIraiLogOutputData>
{
    public GetKensaIraiLogInputData(int hpId, int startDate, int endDate)
    {
        HpId = hpId;
        StartDate = startDate;
        EndDate = endDate;
    }

    public int HpId { get; private set; }

    public int StartDate { get; private set; }

    public int EndDate { get; private set; }
}
