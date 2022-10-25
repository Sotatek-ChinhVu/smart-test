using static Helper.Constants.OrderInfConst;

namespace Domain.Models.TodayOdr
{
    public class CheckedOrderModel
    {
        public CheckedOrderModel(CheckingType checkingType, bool santei, string checkingContent, string itemCd, int sinKouiKbn, string itemName, int inOutKbn)
        {
            CheckingType = checkingType;
            Santei = santei;
            CheckingContent = checkingContent;
            ItemCd = itemCd;
            SinKouiKbn = sinKouiKbn;
            ItemName = itemName;
            InOutKbn = inOutKbn;
        }

        /// <summary>
        /// 種別
        /// </summary>
        public CheckingType CheckingType { get; private set; }

        public string CheckingTypeDisplay
        {
            get
            {
                switch (CheckingType)
                {
                    case CheckingType.MissingCalculate:
                        return "算定漏れ";
                    case CheckingType.Order:
                        return "オーダー";
                    default:
                        return string.Empty;
                }
            }
        }

        public bool Santei { get; private set; }

        public string CheckingContent { get; private set; }

        public string ItemCd { get; private set; }

        public int SinKouiKbn { get; private set; }

        public string ItemName { get; private set; }

        public int InOutKbn { get; private set; }


        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(ItemCd);
        }

        public bool IsEnableSantei
        {
            get => !CheckDefaultValue();
        }
    }
}
