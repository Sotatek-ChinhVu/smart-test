namespace Domain.Models.MstItem
{
    public class OtcItemModel
    {
        public OtcItemModel(int serialNum, string otcCd, string tradeName, string tradeKana, string classCd, string companyCd, string tradeCd, string drugFormCd, string yohoCd, string form, string makerName, string makerKana, string yoho, string className, string majorDivCd)
        {
            SerialNum = serialNum;
            OtcCd = otcCd;
            TradeName = tradeName;
            TradeKana = tradeKana;
            ClassCd = classCd;
            CompanyCd = companyCd;
            TradeCd = tradeCd;
            DrugFormCd = drugFormCd;
            YohoCd = yohoCd;
            Form = form;
            MakerName = makerName;
            MakerKana = makerKana;
            Yoho = yoho;
            ClassName = className;
            MajorDivCd = majorDivCd;
        }

        public int SerialNum { get; private set; }
        public string OtcCd { get; private set; }
        public string TradeName { get; private set; }
        public string TradeKana { get; private set; }
        public string ClassCd { get; private set; }
        public string CompanyCd { get; private set; }
        public string TradeCd { get; private set; }
        public string DrugFormCd { get; private set; }
        public string YohoCd { get; private set; }
        public string Form { get; private set; }
        public string MakerName { get; private set; }
        public string MakerKana { get; private set; }
        public string Yoho { get; private set; }
        public string ClassName { get; private set; }
        public string MajorDivCd { get; private set; }
    }
}
