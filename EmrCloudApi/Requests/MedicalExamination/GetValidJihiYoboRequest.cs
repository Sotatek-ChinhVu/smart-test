using Domain.Models.OrdInfs;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetValidJihiYoboRequest
    {
        public int SyosaiKbn { get; set; }
        public int SinDate { get; set; }
        public List<OrdInfModel> AllOdrInf { get; set; } = new();
    }
}
