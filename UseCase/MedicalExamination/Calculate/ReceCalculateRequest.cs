namespace UseCase.MedicalExamination.Calculate;

public class ReceCalculateRequest
{
    public ReceCalculateRequest(List<long> ptIds, int seikyuYm, string hostName, string uniqueKey)
    {
        PtIds = ptIds;
        SeikyuYm = seikyuYm;
        HostName = hostName;
        UniqueKey = uniqueKey;
    }

    public List<long> PtIds { get; private set; }

    public int SeikyuYm { get; private set; }

    public string HostName { get; private set; }

    public string UniqueKey { get; private set; }
}
