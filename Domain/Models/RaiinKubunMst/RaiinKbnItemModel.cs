namespace Domain.Models.RaiinKubunMst
{
    public class RaiinKbnItemModel
    {
        public RaiinKbnItemModel(int hpId, int grpCd, int kbnCd, long seqNo, string itemCd, int isExclude, bool isDeleted, int sortNo)
        {
            HpId = hpId;
            GrpCd = grpCd;
            KbnCd = kbnCd;
            SeqNo = seqNo;
            ItemCd = itemCd;
            IsExclude = isExclude;
            IsDeleted = isDeleted;
            SortNo = sortNo;
            InputName = string.Empty;
        }

        public RaiinKbnItemModel(int hpId, int grpCd, int kbnCd, long seqNo, string itemCd, int isExclude, bool isDeleted, int sortNo, string inputName)
        {
            HpId = hpId;
            GrpCd = grpCd;
            KbnCd = kbnCd;
            SeqNo = seqNo;
            ItemCd = itemCd;
            IsExclude = isExclude;
            IsDeleted = isDeleted;
            SortNo = sortNo;
            InputName = inputName;
        }

        public int HpId { get; private set; }

        public int GrpCd { get; private set; }

        public int KbnCd { get; private set; }

        public long SeqNo { get; private set; }

        public string ItemCd { get; private set; }

        public int IsExclude { get; private set; }

        public bool IsDeleted { get; private set; }

        public int SortNo { get; private set; }

        public string InputName { get; private set; }

        public RaiinKbnItemModel ChangeSeqNo(long seqNo)
        {
            SeqNo = seqNo;
            return this;
        }
    }
}
