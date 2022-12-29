using Domain.Models.RaiinKbn;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class InitKbnSettingResponse
    {
        public InitKbnSettingResponse(List<RaiinKbnModel> raiinKbnModels)
        {
            RaiinKbnModels = raiinKbnModels;
        }

        public List<RaiinKbnModel> RaiinKbnModels { get; private set; }
    }
}
