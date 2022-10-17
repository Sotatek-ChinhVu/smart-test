using Domain.Models.DrugDetail;

namespace EmrCloudApi.Tenant.Responses.DrugDetail
{
    public class GetDrugDetailDataResponse
    {
        public GetDrugDetailDataResponse(DrugDetailModel data)
        {
            Data = data;
        }

        public DrugDetailModel Data { get; private set; }
    }
}
