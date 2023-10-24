using Domain.Models.MstItem;

namespace EmrCloudApi.Requests.MstItem
{
    public class UpdateByomeiMstRequest
    {
        public List<UpdateByomeiMstModel> ListData { get; set; } = new List<UpdateByomeiMstModel>();
    }
}
