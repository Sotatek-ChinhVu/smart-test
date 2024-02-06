using EmrCloudApi.Requests.Yousiki.RequestItem;

namespace EmrCloudApi.Requests.Yousiki
{
    public class UpdateYosikiRequest
    {
        public Yousiki1InfRequest Yousiki1Inf { get; set; } = new();

        public bool IsTemporarySave { get; set; }
    }
}
