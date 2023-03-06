using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class CheckedExpiredResponse
    {
        public CheckedExpiredResponse(Dictionary<string, (string, List<TenItemModel>)> messages)
        {
            Messages = messages;
        }

        public Dictionary<string, (string, List<TenItemModel>)> Messages { get; private set; }
    }
}
