using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.Save
{
    public class SavePatientInfoInputData : IInputData<SavePatientInfoOutputData>
    {
        public SavePatientInfoInputData(int hpId, PatientInforModel patient, string memo, PtInfSanteiConfModel ptSanteis, List<PtInfHokenPartternModel> hokenPartterns, List<PtGrpInfModel> ptGrps)
        {
            HpId = hpId;
            Patient = patient;
            Memo = memo;
            PtSanteis = ptSanteis;
            HokenPartterns = hokenPartterns;
            PtGrps = ptGrps;
        }

        public int HpId { get; private set; }
        public PatientInforModel Patient { get; private set; }
        public string Memo { get; private set; }
        public PtInfSanteiConfModel PtSanteis { get; private set; }
        public List<PtInfHokenPartternModel> HokenPartterns { get; private set; }
        public List<PtGrpInfModel> PtGrps { get; private set; }
    }
}
