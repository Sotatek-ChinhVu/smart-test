using Helper.Enum;

namespace Domain.Models.TodayOdr
{
    public class CheckSpecialItemModel
    {
        /// <summary>
        /// 種別
        /// </summary>
        public CheckSpecialType CheckingType { get; private set; }

        public string CheckingTypeDisplay
        {
            get
            {
                switch (CheckingType)
                {
                    case CheckSpecialType.AgeLimit:
                        return "年齢制限";
                    case CheckSpecialType.Expiration:
                        return "有効期限";
                    case CheckSpecialType.CalculationCount:
                        return "算定回数";
                    case CheckSpecialType.ItemComment:
                        return "コメント";
                    case CheckSpecialType.Duplicate:
                        return "項目重複";
                    default:
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// レベル
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// チェック内容
        /// </summary>
        public string CheckingContent { get; private set; }

        /// <summary>
        /// 項目コード
        /// </summary>
        public string ItemCd { get; private set; }

        public CheckSpecialItemModel(CheckSpecialType checkingType, string label, string checkingContent, string itemCd)
        {
            CheckingType = checkingType;
            Label = label;
            CheckingContent = checkingContent;
            ItemCd = itemCd;
        }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(ItemCd);
        }
    }
}
