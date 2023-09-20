namespace Domain.Models.MstItem
{
    public class ItemCmtModel
    {
        public ItemCmtModel(string itemCd, int hpId, int seqNo, string comment, int sortNo, int isDeleted)
        {
            ItemCd = itemCd;
            HpId = hpId;
            SeqNo = seqNo;
            Comment = comment;
            SortNo = sortNo;
            IsDeleted = isDeleted;
        }

        public string ItemCd { get; private set; }
        public int HpId { get; private set; }
        public int SeqNo { get; private set; }

        public string Comment { get; private set; }

        public int SortNo { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
