namespace UseCase.MedicalExamination.Calculate;

public class ReceCalculateRequest
{
    public ReceCalculateRequest(int hpId, List<long> ptIds, int seikyuYm, string uniqueKey)
    {
        HpId = hpId;
        PtIds = ptIds;
        SeikyuYm = seikyuYm;
        UniqueKey = uniqueKey;
    }

    public int HpId { get; private set; }

    public List<long> PtIds { get; private set; }

    public int SeikyuYm { get; private set; }

    public string UniqueKey { get; private set; }
}
