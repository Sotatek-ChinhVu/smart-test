using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.Receipt
{
    public class GetMedicalDetailsResponse
    {
        public GetMedicalDetailsResponse(List<SinMeiModel> sinMeiModels, Dictionary<int, string> holidays)
        {
            SinMeiModels = sinMeiModels;
            Holidays = holidays;
        }

        public List<SinMeiModel> SinMeiModels { get; private set; }
        public Dictionary<int, string> Holidays { get; private set; }
    }
}
