using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetKensaIrai;

public class GetKensaIraiInputData : IInputData<GetKensaIraiOutputData>
{
    public GetKensaIraiInputData(int hpId, long ptId, int startDate, int endDate, string kensaCenterMstCenterCd, int kensaCenterMstPrimaryKbn)
    {
        HpId = hpId;
        PtId = ptId;
        StartDate = startDate;
        EndDate = endDate;
        KensaCenterMstCenterCd = kensaCenterMstCenterCd;
        KensaCenterMstPrimaryKbn = kensaCenterMstPrimaryKbn;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int StartDate { get; private set; }

    public int EndDate { get; private set; }

    public string KensaCenterMstCenterCd { get; private set; }

    public int KensaCenterMstPrimaryKbn { get; private set; }
}
