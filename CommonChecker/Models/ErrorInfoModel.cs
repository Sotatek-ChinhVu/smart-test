namespace CommonChecker.Models
{
    public class ErrorInfoModel
    {
        public string Id { get; set; } = string.Empty;

        public string FirstCellContent { get; set; } = string.Empty;

        public string SecondCellContent { get; set; } = string.Empty;

        public string ThridCellContent { get; set; } = string.Empty;

        public string FourthCellContent { get; set; } = string.Empty;

        public string SuggestedContent { get; set; } = string.Empty;

        public string CheckingItemCd { get; set; } = string.Empty;

        public string CurrentItemCd { get; set; } = string.Empty;

        public List<LevelInfoModel> ListLevelInfo { get; set; } = new List<LevelInfoModel>();

    }
}
