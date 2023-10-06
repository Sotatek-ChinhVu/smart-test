using Domain.Models.MstItem;

namespace EmrCloudApi.Requests.MstItem
{
    public class SaveSetNameMntRequest
    {
        public List<SetNameMntModel> ListData { get; set; } = new();

        public int SinDate { get; set; } = 0;
    }
}
