using static Helper.Constants.OrderInfConst;

namespace Domain.Models.MedicalExamination
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

        public CheckedOrderModel()
        {
            CheckingContent = string.Empty;
            ItemCd = string.Empty;
            ItemName = string.Empty;
        }

        public CheckingType CheckingType { get; private set; }

        public bool Santei { get; private set; }

        public string CheckingContent { get; private set; }

        public string ItemCd { get; private set; }

        public int SinKouiKbn { get; private set; }

        public string ItemName { get; private set; }

        public int InOutKbn { get; set; }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(ItemCd);
        }

        public bool IsEnableSantei
        {
            get => !CheckDefaultValue();
        }

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

        public CheckedOrderModel ChangeSantei(bool santei)
        {
            Santei = santei;
            return this;
        }
    }
}

