using Domain.Models.PtCmtInf;
using Domain.Models.SpecialNote.PatientInfo;

namespace UseCase.SpecialNote.Get
{
    public class PatientInfoTabItem
    {
        public PatientInfoTabItem(PtPregnancyModel? pregnancyItems, PtCmtInfModel? ptCmtInfItems, SeikaturekiInfModel? seikatureInfItems, List<PhysicalInfoModel> physicalInfItems)
        {
            PregnancyItems = pregnancyItems;
            PtCmtInfItems = ptCmtInfItems;
            SeikatureInfItems = seikatureInfItems;
            PhysicalInfItems = physicalInfItems;
        }

        public PtPregnancyModel? PregnancyItems { get; private set; }
        public PtCmtInfModel? PtCmtInfItems { get; private set; }
        public SeikaturekiInfModel? SeikatureInfItems { get; private set; }
        public List<PhysicalInfoModel> PhysicalInfItems { get; private set; }
    }
}
