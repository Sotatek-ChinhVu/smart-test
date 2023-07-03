namespace Domain.Models.RaiinListSetting
{
    public class RaiinListItemModel
    {
        public RaiinListItemModel(int hpId, int grpId, int kbnCd, string itemCd, long seqNo, string inputName, int isExclude, bool isAddNew, int isDeleted)
        {
            HpId = hpId;
            GrpId = grpId;
            KbnCd = kbnCd;
            ItemCd = itemCd;
            SeqNo = seqNo;
            InputName = inputName;
            IsExclude = isExclude;
            IsAddNew = isAddNew;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public string ItemCd{ get; private set; }

        public long SeqNo { get; private set; }

        public string InputName { get; private set; }

        public int IsExclude { get; private set; }

        public bool IsAddNew { get; private set; }

        public int IsDeleted { get; private set; }

        public bool CheckDefaultValue()
        {
            return IsAddNew && string.IsNullOrWhiteSpace(ItemCd) && IsExclude == 0;
        }
    }
}
