using Domain.Models.PtCmtInf;
using Domain.Models.SpecialNote.PatientInfo;
using System.Text.Json.Serialization;

namespace UseCase.SpecialNote.Save
{
    public class PatientInfoItem
    {
        [JsonConstructor]
        public PatientInfoItem(List<PtPregnancyItem> pregnancyItems, PtCmtInfModel ptCmtInfItems, SeikaturekiInfModel seikatureInfItems, List<KensaInfDetailItem> kensaInfDetailItems)
        {
            PregnancyItems = pregnancyItems;
            PtCmtInfItems = ptCmtInfItems;
            SeikatureInfItems = seikatureInfItems;
            KensaInfDetailItems = kensaInfDetailItems;
        }

        public PatientInfoItem()
        {
            PregnancyItems = new();
            PtCmtInfItems = new();
            SeikatureInfItems = new();
            KensaInfDetailItems = new();
        }

        public List<PtPregnancyItem> PregnancyItems { get; private set; }

        public PtCmtInfModel PtCmtInfItems { get; private set; }

        public SeikaturekiInfModel SeikatureInfItems { get; private set; }

        public List<KensaInfDetailItem> KensaInfDetailItems { get; private set; }
    }
}
