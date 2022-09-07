using Domain.Models.InputItem;

namespace EmrCloudApi.Tenant.Responses.InputItem
{
    public class GetDosageDrugListResponse
    {
        public GetDosageDrugListResponse(List<DosageDrugModel> listData)
        {
            ListData = listData;
        }

        public List<DosageDrugModel> ListData { get; private set; }
    }
}
