using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetKensaInf;

public class GetKensaInfInputData : IInputData<GetKensaInfOutputData>
{
    public GetKensaInfInputData(int hpId, int startDate, int endDate, string centerCd)
    {
        HpId = hpId;
        StartDate = startDate;
        EndDate = endDate;
        CenterCd = centerCd;
    }

    public int HpId { get; private set; }

    public int StartDate { get; private set; }

    public int EndDate { get; private set; }

    public string CenterCd { get; private set; }
}
