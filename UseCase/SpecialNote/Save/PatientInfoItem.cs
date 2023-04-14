using Domain.Models.PtCmtInf;
using Domain.Models.SpecialNote.PatientInfo;

namespace UseCase.SpecialNote.Save
{
    public class PatientInfoItem
    {
        public PatientInfoItem(List<PtPregnancyItem> pregnancyItems, PtCmtInfModel ptCmtInfItems, SeikaturekiInfModel seikatureInfItems, List<KensaInfDetailModel> kensaInfDetailModels)
        {
            PregnancyItems = pregnancyItems;
            PtCmtInfItems = ptCmtInfItems;
            SeikatureInfItems = seikatureInfItems;
            KensaInfDetailModels = kensaInfDetailModels;
        }

        public List<PtPregnancyItem> PregnancyItems { get; private set; }

        public PtCmtInfModel PtCmtInfItems { get; private set; }

        public SeikaturekiInfModel SeikatureInfItems { get; private set; }

        public List<KensaInfDetailModel> KensaInfDetailModels { get; private set; }
    }
}
