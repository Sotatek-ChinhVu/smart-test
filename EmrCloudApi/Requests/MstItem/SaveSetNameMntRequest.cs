using Domain.Models.MstItem;

namespace EmrCloudApi.Requests.MstItem
{
    public class SaveSetNameMntRequest
    {
        public List<SaveSetNameMntModel> ListData { get; set; } = new List<SaveSetNameMntModel>();
        public int SinDate { get; set; } = 0;
    }
}
