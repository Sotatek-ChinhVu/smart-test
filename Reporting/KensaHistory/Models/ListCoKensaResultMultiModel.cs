namespace Reporting.KensaHistory.Models
{
    public class ListCoKensaResultMultiModel
    {
        public ListCoKensaResultMultiModel(List<CoKensaResultMultiModel> coKensaResultMultiModels) 
        {
            CoKensaResultMultiModels = coKensaResultMultiModels;
        }

        public List<CoKensaResultMultiModel> CoKensaResultMultiModels { get; set; }
    }
}
