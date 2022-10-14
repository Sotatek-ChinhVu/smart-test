using Domain.Models.GroupInf;
using Domain.Models.InsuranceInfor;
using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.Save
{
    public class SavePatientInfoInputData : IInputData<SavePatientInfoOutputData>
    {
        public SavePatientInfoInputData(PatientInforSaveModel patient, List<PtKyuseiModel> ptKyuseis, PtInfSanteiConfModel ptSanteis, List<InsuranceModel> insurances, List<GroupInfModel> ptGrps)
        {
            Patient = patient;
            PtKyuseis = ptKyuseis;
            PtSanteis = ptSanteis;
            Insurances = insurances;
            PtGrps = ptGrps;
        }
        public PatientInforSaveModel Patient { get; private set; }
        public List<PtKyuseiModel> PtKyuseis { get; private set; } = new List<PtKyuseiModel>();
        public PtInfSanteiConfModel PtSanteis { get; private set; }
        public List<InsuranceModel> Insurances { get; private set; }
        public List<GroupInfModel> PtGrps { get; private set; }
    }
}
