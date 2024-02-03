namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class StrokeHistoryRequest
    {
        public Yousiki1InfDetailRequest Type { get; set; } = new();

        public Yousiki1InfDetailRequest OnsetDate { get; set; } = new();

        public bool IsDeleted { get; set; }

        public int SortNo { get; set; }

        public bool IsEnableOnsetDate { get => Type.Value == "1" || Type.Value == "2" || Type.Value == "3" || Type.Value == "4"; }
    }
}
