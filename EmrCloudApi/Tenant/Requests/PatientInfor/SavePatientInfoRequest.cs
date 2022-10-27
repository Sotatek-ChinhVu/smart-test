using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class SavePatientInfoRequest
    {
        public SavePatientInfoRequest(PatientInforSaveModel patient, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrpInfs)
        {
            Patient = patient;
            PtKyuseis = ptKyuseis;
            PtSanteis = ptSanteis;
            Insurances = insurances;
            PtGrpInfs = ptGrpInfs;
            HokenInfs = hokenInfs;
            HokenKohis = hokenKohis;
        }

        public PatientInforSaveModel Patient { get; private set; }
        public List<PtKyuseiModel> PtKyuseis { get; private set; } = new List<PtKyuseiModel>();
        public List<CalculationInfModel> PtSanteis { get; private set; } = new List<CalculationInfModel>();
        public List<InsuranceModel> Insurances { get; private set; } = new List<InsuranceModel>();
        public List<GroupInfModel> PtGrpInfs { get; private set; } = new List<GroupInfModel>();
        public List<HokenInfModel> HokenInfs { get; private set; } = new List<HokenInfModel>();
        public List<KohiInfModel> HokenKohis { get; private set; } = new List<KohiInfModel>();
    }
}