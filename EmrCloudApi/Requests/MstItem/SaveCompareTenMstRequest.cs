using Domain.Models.MstItem;

namespace EmrCloudApi.Requests.MstItem
{
    public class SaveCompareTenMstRequest
    {
        public List<SaveCompareTenMstModel> ListData { get; set; } = new List<SaveCompareTenMstModel>();
        public ComparisonSearchModel Comparions { get; set; } = 0;
    }
}
