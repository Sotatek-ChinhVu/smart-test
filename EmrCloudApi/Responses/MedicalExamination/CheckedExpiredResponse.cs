namespace EmrCloudApi.Responses.MedicalExamination
{
    public class CheckedExpiredResponse
    {
        public CheckedExpiredResponse(List<string> messages)
        {
            Messages = messages;
        }

        public List<string> Messages { get; private set; }
    }
}
