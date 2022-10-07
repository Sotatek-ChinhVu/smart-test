using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.Save
{
    public class SavePatientInfoInputData : IInputData<SavePatientInfoOutputData>
    {
        public SavePatientInfoInputData(PatientInforSaveModel patient, PtInfSanteiConfModel ptSanteis, List<PtInfHokenPartternModel> hokenPartterns, List<PtGrpInfModel> ptGrps)
        {
            Patient = patient;
            PtSanteis = ptSanteis;
            HokenPartterns = hokenPartterns;
            PtGrps = ptGrps;
        }
        public PatientInforSaveModel Patient { get; private set; }
        public PtInfSanteiConfModel PtSanteis { get; private set; }
        public List<PtInfHokenPartternModel> HokenPartterns { get; private set; }
        public List<PtGrpInfModel> PtGrps { get; private set; }
    }
}
