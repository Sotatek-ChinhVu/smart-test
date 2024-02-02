namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class HospitalizationStatusListRequestItem
    {
        public List<UpdateYousiki1InfDetailRequestItem> UpdateYousiki1InfDetailRequestItem { get; set; } = new();

        public UpdateYousiki1InfDetailRequestItem InjuryNameLast { get; set; } = new();

        public UpdateYousiki1InfDetailRequestItem InjuryNameCode {  get; set; } = new(); 

        public UpdateYousiki1InfDetailRequestItem ModifierCode { get; set; } = new();

        public string ByomeiCdRequest { get; set; } = string.Empty;

        public string FullByomeiRequest { get; set; } = string.Empty;

        public List<PrefixSuffixRequest> PrefixSuffixRequests { get; set; } = new();
    }
}
