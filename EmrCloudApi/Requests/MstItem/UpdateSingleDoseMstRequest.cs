using Domain.Models.MstItem;

namespace EmrCloudApi.Requests.MstItem
{
    public class UpdateSingleDoseMstRequest
    {
        public List<SingleDoseMstModel> SingleDoseMsts { get; set; } = new();
    }
}
