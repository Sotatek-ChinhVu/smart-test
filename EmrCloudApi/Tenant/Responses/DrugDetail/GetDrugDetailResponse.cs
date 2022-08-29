using Domain.Models.DrugDetail;

namespace EmrCloudApi.Tenant.Responses.DrugDetail
{
    public class GetDrugDetailResponse
    {
        public GetDrugDetailResponse(List<DrugMenuItemModel> listData)
        {
            ListData = listData;
        }

        public List<DrugMenuItemModel> ListData { get; private set; }
    }
}
