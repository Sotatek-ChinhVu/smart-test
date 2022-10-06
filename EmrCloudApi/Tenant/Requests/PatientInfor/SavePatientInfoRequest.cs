using Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class SavePatientInfoRequest
    {
        public int HpId { get; set; }
        public string Memo { get; set; } = string.Empty;
        public PatientInforModel Patient { get; set; } = new();
        public PtInfSanteiConfModel PtSantei { get; set; }
        public PtInfSanteiConfModel PtSanteiConf { get; set; }
        public List<PtInfHokenPartternModel> HokenParterns { get; set; } = new List<PtInfHokenPartternModel>();
        public List<PtGrpInfModel> PtGrpInfs { get; set; } = new List<PtGrpInfModel>();

    }
}
