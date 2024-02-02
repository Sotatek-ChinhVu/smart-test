namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class HospitalizationStatusListRequestItem
    {
        public UpdateYosiki1InfDetailRequestItem UpdateYosiki1InfDetailRequestItem { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem InjuryNameLast { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem InjuryNameCode {  get; set; } = new(); 

        public UpdateYosiki1InfDetailRequestItem ModifierCode { get; set; } = new();

        public string ByomeiCdRequest { get; set; } = string.Empty;

        public string FullByomeiRequest { get; set; } = string.Empty;

        public List<PrefixSuffixRequest> PrefixSuffixRequests { get; set; } = new();
    }
}
