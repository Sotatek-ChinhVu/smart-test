using Domain.Models.Medical;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class CheckedAfter327ScreenResponse
    {
        public CheckedAfter327ScreenResponse(string message, List<SinKouiCountModel> sinKouiCountModels)
        {
            Message = message;
            SinKouiCountModels = sinKouiCountModels;
        }

        public string Message { get; private set; }

        public List<SinKouiCountModel> SinKouiCountModels { get; private set; }
    }
}
