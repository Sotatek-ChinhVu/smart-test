using Domain.Models.MstItem;
using UseCase.MedicalExamination.CheckedExpired;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class CheckedExpiredResponse
    {
        public CheckedExpiredResponse(List<CheckedExpiredOutputItem> messages)
        {
            Messages = messages;
        }

        public List<CheckedExpiredOutputItem> Messages { get; private set; }
    }
}
