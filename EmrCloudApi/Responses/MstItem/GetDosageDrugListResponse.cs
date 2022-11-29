using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
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
