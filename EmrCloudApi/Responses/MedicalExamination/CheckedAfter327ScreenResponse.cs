using Domain.Models.Medical;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class CheckedAfter327ScreenResponse
    {
        public CheckedAfter327ScreenResponse(List<string> messages, List<SinKouiCountModel> sinKouiCountModels)
        {
            Messages = messages;
            SinKouiCountModels = sinKouiCountModels;
        }

        public List<string> Messages { get; private set; }

        public List<SinKouiCountModel> SinKouiCountModels { get; private set; }
    }
}
