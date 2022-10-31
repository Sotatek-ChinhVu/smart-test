
using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.PatientInfor;
using EmrCloudApi.Tenant.Requests.Insurance;

namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class SavePatientInfoRequest
    {
        public SavePatientInfoRequest(PatientInforDto patient, List<HokenPartternDto> insurances, List<HokenInfDto> hokenInfs, List<HokenKohiDto> hokenKohis, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<GroupInfModel> ptGrps)
        {
            Patient = patient;
            Insurances = insurances;
            HokenInfs = hokenInfs;
            HokenKohis = hokenKohis;
            PtKyuseis = ptKyuseis;
            PtSanteis = ptSanteis;
            PtGrps = ptGrps;
        }

        public PatientInforDto Patient { get; private set; }
        public List<HokenPartternDto> Insurances { get; private set; } = new List<HokenPartternDto>();
        public List<HokenInfDto> HokenInfs { get; private set; } = new List<HokenInfDto>();
        public List<HokenKohiDto> HokenKohis { get; private set; } = new List<HokenKohiDto>();
        public List<PtKyuseiModel> PtKyuseis { get; private set; } = new List<PtKyuseiModel>();
        public List<CalculationInfModel> PtSanteis { get; private set; } = new List<CalculationInfModel>();
        public List<GroupInfModel> PtGrps { get; private set; } = new List<GroupInfModel>();
    }
}