namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class SpecialNoteModel
    {
        public List<PtAlrgyDrug> ListPtAlrgyDrug { get; set; }

        public List<PtAlrgyFood> ListPtAlrgyFood { get; set; }

        public List<PtOtherDrug> ListPtOtherDrug { get; set; }

        public List<PtOtcDrug> ListPtOtcDrug { get; set; }

        public List<PtSupple> ListPtSupple { get; set; }

        public List<PtKioReki> ListPtKioReki { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }
    }
}
