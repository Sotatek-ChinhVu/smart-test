using UseCase.MedicalExamination.TrailAccounting;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetTrialAccountingMeiHoGaiResponse
    {
        public GetTrialAccountingMeiHoGaiResponse(GetTrialAccountingMeiHoGaiStatus status)
        {
            Status = status;
        }

        public GetTrialAccountingMeiHoGaiStatus Status { get; private set; }
    }
}
