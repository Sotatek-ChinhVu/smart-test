using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.Calculate
{
    public class CalculateOutputData : IOutputData
    {
        public CalculateOutputData(long ptId, int sinDate, CalculateStatus status)
        {
            PtId = ptId;
            SinDate = sinDate;
            Status = status;
        }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public CalculateStatus Status { get; private set; }
    }
}
