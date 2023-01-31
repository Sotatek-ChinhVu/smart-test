using CommonCheckers.OrderRealtimeChecker.Enums;

namespace CommonChecker.Models
{
    public class ErrorInfoModel
    {
        public string Id { get; set; }

        public string FirstCellContent { get; set; }

        public string SecondCellContent { get; set; }

        public string ThridCellContent { get; set; }

        public string FourthCellContent { get; set; }

        public string SuggestedContent { get; set; }

        public List<LevelInfoModel> ListLevelInfo { get; set; }

    }
}
