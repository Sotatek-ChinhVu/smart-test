namespace Domain.Models.PatientInfor
{
    public class SavePatientInfoModel
    {
        public SavePatientInfoModel(PatientInforSaveModel patient, PtInfSanteiConfModel ptSantei, List<PtInfHokenPartternModel> hokenParterns, List<PtGrpInfModel> ptGrpInfs)
        {
            Patient = patient;
            PtSantei = ptSantei;
            HokenParterns = hokenParterns;
            PtGrpInfs = ptGrpInfs;
        }

        public PatientInforSaveModel Patient { get; private set; }
        public PtInfSanteiConfModel PtSantei { get; private set; }
        public List<PtInfHokenPartternModel> HokenParterns { get; private set; } = new List<PtInfHokenPartternModel>();
        public List<PtGrpInfModel> PtGrpInfs { get; private set; } = new List<PtGrpInfModel>();
    }
}
