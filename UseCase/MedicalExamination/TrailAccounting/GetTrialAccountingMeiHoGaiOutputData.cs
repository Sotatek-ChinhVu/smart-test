using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.TrailAccounting
{
    public class GetTrialAccountingMeiHoGaiOutputData : IOutputData
    {
        public GetTrialAccountingMeiHoGaiOutputData(GetTrialAccountingMeiHoGaiStatus status)
        {
            Status = status;
        }

        public GetTrialAccountingMeiHoGaiStatus Status { get; private set; }
    }
}
