
using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.PatientInfor;
using EmrCloudApi.Tenant.Requests.Insurance;

namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class SavePatientInfoRequest
    {
        public PatientInforDto Patient { get; private set; }
        public List<HokenPartternDto> Insurances { get; private set; }
        public List<PatientInforDto> HokenInfs { get; private set; }
        public List<HokenKohiDto> HokenKohis { get; private set; }
        public List<PtKyuseiModel> PtKyuseis { get; private set; } = new List<PtKyuseiModel>();
        public List<CalculationInfModel> PtSanteis { get; private set; } = new List<CalculationInfModel>();
        public List<GroupInfModel> PtGrps { get; private set; } = new List<GroupInfModel>();
    }
}