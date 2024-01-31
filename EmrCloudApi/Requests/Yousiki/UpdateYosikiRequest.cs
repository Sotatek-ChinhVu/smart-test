using EmrCloudApi.Requests.Yousiki.RequestItem;

namespace EmrCloudApi.Requests.Yousiki
{
    public class UpdateYosikiRequest
    {
        public UpdateYosikiInfRequestItem Yousiki1InfModel { get; set; } = new();

        public CommonForm1ModelRequest CommonModelRequest { get; set; } = new();

        public LivingHabitModelRequest LivingHabitModelRequest { get; set; } = new();

        public AtHomeModelRequest AtHomeModelRequest { get; set; } = new();

        public RehabilitationModelRequest RehabilitationModelRequest { get; set; } = new();

        public Dictionary<int, int> DataTypes { get; set; } = new();

        public bool IsTemporarySave { get; set; }
    }
}
