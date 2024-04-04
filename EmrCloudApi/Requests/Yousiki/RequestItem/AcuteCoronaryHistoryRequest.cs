namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class AcuteCoronaryHistoryRequest
    {
        public Yousiki1InfDetailRequest? Type { get; set; } 

        public Yousiki1InfDetailRequest? OnsetDate { get; set; }

        public int SortNo { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEnableOnsetDate { get => Type?.Value == "1" || Type?.Value == "2"; }
    }
}
