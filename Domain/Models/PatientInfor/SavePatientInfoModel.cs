namespace Domain.Models.PatientInfor
{
    public class SavePatientInfoModel
    {
        public SavePatientInfoModel(int hpId, string memo, PatientInforModel patient, PtInfSanteiConfModel ptSantei, PtInfSanteiConfModel ptSanteiConf, List<PtInfHokenPartternModel> hokenParterns, List<PtGrpInfModel> ptGrpInfs)
        {
            HpId = hpId;
            Memo = memo;
            Patient = patient;
            PtSantei = ptSantei;
            PtSanteiConf = ptSanteiConf;
            HokenParterns = hokenParterns;
            PtGrpInfs = ptGrpInfs;
        }

        public int HpId { get; private set; }
        public string Memo { get; private set; }
        public PatientInforModel Patient { get;private set; }
        public PtInfSanteiConfModel PtSantei { get; private set; }
        public PtInfSanteiConfModel PtSanteiConf { get; private set; }
        public List<PtInfHokenPartternModel> HokenParterns { get; private set; } = new List<PtInfHokenPartternModel>();
        public List<PtGrpInfModel> PtGrpInfs { get; private set; } = new List<PtGrpInfModel>();
    }
}
