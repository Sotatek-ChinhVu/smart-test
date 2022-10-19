using Entity.Tenant;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class SpecialNoteModel
    {
        public List<PtAlrgyDrug> ListPtAlrgyDrug { get; set; } = new List<PtAlrgyDrug>();

        public List<PtAlrgyFood> ListPtAlrgyFood { get; set; } = new List<PtAlrgyFood>();

        public List<PtOtherDrug> ListPtOtherDrug { get; set; } = new List<PtOtherDrug>();

        public List<PtOtcDrug> ListPtOtcDrug { get; set; } = new List<PtOtcDrug>();

        public List<PtSupple> ListPtSupple { get; set; } = new List<PtSupple>();

        public List<PtKioReki> ListPtKioReki { get; set; } = new List<PtKioReki>();

        public double Height { get; set; }

        public double Weight { get; set; }
    }
}
