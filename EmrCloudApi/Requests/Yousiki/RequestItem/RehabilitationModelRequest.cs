namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class RehabilitationModelRequest
    {
        public List<UpdateYosiki1InfDetailRequestItem> UpdateYosiki1InfDetailRequestItems { get; set; } = new();

        public List<OutpatientConsultationModelRequest> OutpatientConsultationList { get; set; } = new();

        public List<CommonForm1ModelRequest> ByomeiRehabilitationList { get; set; } = new();

        public List<BarthelIndexListRequest> BarthelIndexLists { get; set; } = new();

        public List<FimListRequest> FimLists {  get; set; } = new();
    }
}
