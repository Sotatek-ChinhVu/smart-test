using Domain.Models.GroupInf;

namespace Domain.Models.PatientInfor
{
    public class SavePatientInfoModel
    {
        public SavePatientInfoModel(PatientInforSaveModel patient, PtInfSanteiConfModel ptSantei, List<PtInfHokenPartternModel> hokenParterns, List<GroupInfModel> ptGrpInfs)
        {
            Patient = patient;
            PtSantei = ptSantei;
            HokenParterns = hokenParterns;
            PtGrpInfs = ptGrpInfs;
        }

        public PatientInforSaveModel Patient { get; private set; }
        public PtInfSanteiConfModel PtSantei { get; private set; }
        public List<PtInfHokenPartternModel> HokenParterns { get; private set; } = new List<PtInfHokenPartternModel>();
        public List<GroupInfModel> PtGrpInfs { get; private set; } = new List<GroupInfModel>();
    }
}