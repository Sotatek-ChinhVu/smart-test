using Domain.Models.GroupInf;
using Domain.Models.InsuranceInfor;

namespace Domain.Models.PatientInfor
{
    public class SavePatientInfoModel
    {
        public SavePatientInfoModel(PatientInforSaveModel patient, List<PtKyuseiModel> ptKyuseis, PtInfSanteiConfModel ptSantei, List<InsuranceModel> insurances, List<GroupInfModel> ptGrpInfs)
        {
            Patient = patient;
            PtKyuseis = ptKyuseis;
            PtSantei = ptSantei;
            Insurances = insurances;
            PtGrpInfs = ptGrpInfs;
        }

        public PatientInforSaveModel Patient { get; private set; }
        public List<PtKyuseiModel> PtKyuseis { get; private set; } = new List<PtKyuseiModel>();
        public PtInfSanteiConfModel PtSantei { get; private set; }
        public List<InsuranceModel> Insurances { get; private set; } = new List<InsuranceModel>();
        public List<GroupInfModel> PtGrpInfs { get; private set; } = new List<GroupInfModel>();
        
    }
}