using Domain.Models.PtCmtInf;

namespace Domain.Models.SpecialNote.PatientInfo
{
    public class PatientInfoModel
    {
        public PatientInfoModel(List<PtPregnancyModel> pregnancyItems, PtCmtInfModel ptCmtInfItems, SeikaturekiInfModel seikatureInfItems, List<PhysicalInfoModel> physicalInfItems, List<GcStdInfModel> gcStdInfModels)
        {
            PregnancyItems = pregnancyItems;
            PtCmtInfItems = ptCmtInfItems;
            SeikatureInfItems = seikatureInfItems;
            PhysicalInfItems = physicalInfItems;
            GcStdInfModels = gcStdInfModels;
        }

        public PatientInfoModel()
        {
            PregnancyItems = new List<PtPregnancyModel>();
            PtCmtInfItems = new PtCmtInfModel();
            SeikatureInfItems = new SeikaturekiInfModel();
            PhysicalInfItems = new List<PhysicalInfoModel>();
            GcStdInfModels = new List<GcStdInfModel>();
        }

        public List<PtPregnancyModel> PregnancyItems { get; private set; }

        public PtCmtInfModel PtCmtInfItems { get; private set; }

        public SeikaturekiInfModel SeikatureInfItems { get; private set; }

        public List<PhysicalInfoModel> PhysicalInfItems { get; private set; }

        public List<GcStdInfModel> GcStdInfModels { get; private set; }
    }
}
