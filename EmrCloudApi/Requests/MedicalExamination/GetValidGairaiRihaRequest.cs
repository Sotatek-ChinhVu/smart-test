using Domain.Models.OrdInfs;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetValidGairaiRihaRequest
    {
        public int PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }
        public int SyosaiKbn { get; set; }
        public List<OrdInfModel> AllOdrInf { get; set; } = new();
    }
}
