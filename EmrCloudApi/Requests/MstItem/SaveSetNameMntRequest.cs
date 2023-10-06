using Domain.Models.MstItem;
using EmrCloudApi.Requests.MstItem.RequestItem;

namespace EmrCloudApi.Requests.MstItem
{
    public class SaveSetNameMntRequest
    {
        public List<SetNameMntRequestItem> ListData { get; set; } = new();

        public int SinDate { get; set; } = 0;
    }
}
