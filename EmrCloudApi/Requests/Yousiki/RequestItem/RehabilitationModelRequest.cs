using Domain.Models.Yousiki.CommonModel.CommonOutputModel;

namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class RehabilitationModelRequest
    {
        public List<OutpatientConsultationModelRequest> OutpatientConsultationList { get; set; } = new();

        public List<CommonForm1Model> ByomeiRehabilitationList { get; set; } = new();
    }
}
