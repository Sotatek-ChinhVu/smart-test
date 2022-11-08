namespace Domain.Models.MstItem
{
    public class ItemCmtModel
    {
        public ItemCmtModel(string itemCd, string comment, int sortNo)
        {
            ItemCd = itemCd;
            Comment = comment;
            SortNo = sortNo;
        }

        public string ItemCd { get; private set; }

        public string Comment { get; private set; }

        public int SortNo { get; private set; }
    }
}
