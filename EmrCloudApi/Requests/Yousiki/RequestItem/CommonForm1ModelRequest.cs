namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class CommonForm1ModelRequest
    {
        public List<HospitalizationStatusListRequestItem> HospitalizationStatusListRequestItems { get; set; } = new();

        public List<UpdateYosiki1InfDetailRequestItem> CommonForm1ModelRequestItems { get; set; } = new();
    }
}
