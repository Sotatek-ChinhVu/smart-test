using Domain.Models.MstItem;

namespace EmrCloudApi.Requests.MstItem
{
    public class UpdateJihiMstRequest
    {
        public List<JihiSbtMstModel> JihiSbtMsts { get; set; } = new List<JihiSbtMstModel>();
    }
}
