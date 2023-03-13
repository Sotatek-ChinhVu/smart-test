namespace UseCase.MedicalExamination.Calculate
{
    public class ReceCalculateRequest
    {
        public ReceCalculateRequest(List<long> ptIds, int seikyuYm)
        {
            PtIds = ptIds;
            SeikyuYm = seikyuYm;
        }

        public List<long> PtIds { get; private set; }

        public int SeikyuYm { get; private set; }
    }
}
