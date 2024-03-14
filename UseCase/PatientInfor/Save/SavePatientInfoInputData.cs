using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.MaxMoney;
using Domain.Models.PatientInfor;
using System.Text.Json.Serialization;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.Save
{
    public class SavePatientInfoInputData : IInputData<SavePatientInfoOutputData>
    {
        [JsonConstructor]
        public SavePatientInfoInputData(PatientInforSaveModel patient, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrps, ReactSavePatientInfo reactSave, List<LimitListModel> maxMoneys, IEnumerable<InsuranceScanModel> insuranceScans,List<int> hokenIdList, int userId, int hpId)
        {
            Patient = patient;
            PtKyuseis = ptKyuseis;
            PtSanteis = ptSanteis;
            Insurances = insurances;
            PtGrps = ptGrps;
            HokenInfs = hokenInfs;
            HokenKohis = hokenKohis;
            ReactSave = reactSave;
            MaxMoneys = maxMoneys;
            InsuranceScans = insuranceScans;
            HokenIdList = hokenIdList;
            UserId = userId;
            HpId = hpId;
        }

        public SavePatientInfoInputData()
        {
            Patient = new();
            PtKyuseis = new();
            PtSanteis = new();
            Insurances = new();
            MaxMoneys = new();
            InsuranceScans = Enumerable.Empty<InsuranceScanModel>(); ;
            PtGrps = new();
            HokenInfs = new();
            HokenKohis = new();
            ReactSave = new();
            MaxMoneys = new();
            HokenIdList = new();
        }

        public PatientInforSaveModel Patient { get; private set; }

        public List<PtKyuseiModel> PtKyuseis { get; private set; }

        public List<CalculationInfModel> PtSanteis { get; private set; }

        public List<InsuranceModel> Insurances { get; private set; }

        public List<GroupInfModel> PtGrps { get; private set; }

        public List<HokenInfModel> HokenInfs { get; private set; }

        public List<KohiInfModel> HokenKohis { get; private set; }

        public List<LimitListModel> MaxMoneys { get; private set; }

        public ReactSavePatientInfo ReactSave { get; private set; }

        public IEnumerable<InsuranceScanModel> InsuranceScans { get; private set; }

        public List<int> HokenIdList { get; private set; }

        public int UserId { get; set; }

        public int HpId { get; set; }
    }
}
