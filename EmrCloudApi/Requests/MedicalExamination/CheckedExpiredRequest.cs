using UseCase.MedicalExamination.CheckedExpired;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class CheckedExpiredRequest
    {
        public int SinDate { get; set; }

        public List<CheckedExpiredItem> CheckedExpiredItems { get; set; } = new();
    }
}
