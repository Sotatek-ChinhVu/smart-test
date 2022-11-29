using Domain.Models.ReceptionInsurance;

namespace EmrCloudApi.Responses.ReceptionInsurance
{
    public class ReceptionInsuranceResponse
    {
        public List<ReceptionInsuranceModel> ListData { get; private set; }

        public ReceptionInsuranceResponse(List<ReceptionInsuranceModel> listData)
        {
            ListData = listData;
        }
    }
}
