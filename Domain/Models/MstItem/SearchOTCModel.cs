namespace Domain.Models.MstItem
{

    public class SearchOTCModel
    {
        public SearchOTCModel(List<SearchOTCBaseModel> model, int total)
        {
            Model = model;
            Total = total;
        }

        public List<SearchOTCBaseModel> Model { get; set; }
        public int Total { get; set; }
    }

    public class SearchOTCBaseModel
    {
        public int SerialNum { get; set; }
        public string OtcCd { get; set; } = String.Empty;
        public string TradeName { get; set; } = String.Empty;
        public string TradeKana { get; set; } = String.Empty;
        public string ClassCd { get; set; } = String.Empty;
        public string CompanyCd { get; set; } = String.Empty;
        public string TradeCd { get; set; } = String.Empty;
        public string DrugFormCd { get; set; } = String.Empty;
        public string YohoCd { get; set; } = String.Empty;
        public string Form { get; set; } = String.Empty;
        public string MakerName { get; set; } = String.Empty;
        public string MakerKana { get; set; } = String.Empty;
        public string Yoho { get; set; } = String.Empty;
        public string ClassName { get; set; } = String.Empty;
        public string MajorDivCd { get; set; } = String.Empty;
    }
}
