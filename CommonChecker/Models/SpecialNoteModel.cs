
using CommonChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class SpecialNoteModel
    {
        public List<PtAlrgyDrugModel> ListPtAlrgyDrug { get; set; } = new List<PtAlrgyDrugModel>();

        public List<PtAlrgyFoodModel> ListPtAlrgyFood { get; set; } = new List<PtAlrgyFoodModel>();

        public List<PtOtherDrugModel> ListPtOtherDrug { get; set; } = new List<PtOtherDrugModel>();

        public List<PtOtcDrugModel> ListPtOtcDrug { get; set; } = new List<PtOtcDrugModel>();

        public List<PtSuppleModel> ListPtSupple { get; set; } = new List<PtSuppleModel>();

        public List<PtKioRekiModel> ListPtKioReki { get; set; } = new List<PtKioRekiModel>();

        public double Height { get; set; }

        public double Weight { get; set; }
    }
}
