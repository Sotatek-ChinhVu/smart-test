using Domain.Models.MstItem;

namespace EmrCloudApi.Requests.MstItem
{
    public class SaveAddressMstRequest
    {
        public List<PostCodeMstModel> PostCodeMsts { get; set; } = new();
    }
}
