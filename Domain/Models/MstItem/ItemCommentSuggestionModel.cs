namespace Domain.Models.MstItem
{
    public class ItemCommentSuggestionModel
    {
        public ItemCommentSuggestionModel(string itemCd, string itemName, string santeiItemCd, List<RecedenCmtSelectModel> itemCmtModels)
        {
            ItemCd = itemCd;
            ItemName = itemName;
            SanteiItemCd = santeiItemCd;
            ItemCmtModels = itemCmtModels;
        }

        public string ItemCd { get; private set; }

        public string ItemName { get; private set; }

        public string SanteiItemCd { get; private set; }

        public List<RecedenCmtSelectModel> ItemCmtModels { get; private set; }

        public ItemCommentSuggestionModel SetData(List<RecedenCmtSelectModel> itemCmtModels)
        {
            ItemCmtModels = itemCmtModels;
            return this;
        }
    }
}
