namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class AcuteAorticDissectionHistoryRequest
    {
        public Yousiki1InfDetailRequest OnsetDate { get; set; } = new();

        public bool IsDeleted { get; set; }
    }
}
