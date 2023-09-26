using Domain.Models.MstItem;

namespace EmrCloudApi.Requests.MstItem
{
    public class CompareTenMstRequest
    {
        public int SinDate { get; set; } = 0;

        public List<ActionCompareSearchModel> Actions { get; set; } = new List<ActionCompareSearchModel>();

        public ComparisonSearchModel Comparison { get; set; } = 0;

    }
}
