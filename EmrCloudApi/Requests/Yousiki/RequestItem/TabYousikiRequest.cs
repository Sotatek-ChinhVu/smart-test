namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class TabYousikiRequest
    {
        public CommonRequest CommonRequest { get; set; } = new();

        public AtHomeRequest AtHomeRequest { get; set; } = new();

        public LivingHabitRequest LivingHabitRequest { get; set; } = new();

        public RehabilitationRequest RehabilitationRequest { get; set; } = new();
    }
}
