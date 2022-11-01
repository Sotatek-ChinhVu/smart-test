using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.Save
{
    public class SavePatientInfoInputData : IInputData<SavePatientInfoOutputData>
    {
        public SavePatientInfoInputData(PatientInforSaveModel patient, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrps)
        {
            Patient = patient;
            PtKyuseis = ptKyuseis;
            PtSanteis = ptSanteis;
            Insurances = insurances;
            PtGrps = ptGrps;
            HokenInfs = hokenInfs;
            HokenKohis = hokenKohis;
        }
        public PatientInforSaveModel Patient { get; private set; }
        public List<PtKyuseiModel> PtKyuseis { get; private set; } = new List<PtKyuseiModel>();
        public List<CalculationInfModel> PtSanteis { get; private set; } = new List<CalculationInfModel>();
        public List<InsuranceModel> Insurances { get; private set; } = new List<InsuranceModel>();
        public List<GroupInfModel> PtGrps { get; private set; } = new List<GroupInfModel>();
        public List<HokenInfModel> HokenInfs { get; private set; } = new List<HokenInfModel>();
        public List<KohiInfModel> HokenKohis { get; private set; } = new List<KohiInfModel>();
    }
}
