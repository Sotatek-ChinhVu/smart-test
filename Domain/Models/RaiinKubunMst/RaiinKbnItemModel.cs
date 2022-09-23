using Domain.Models.MstItem;

namespace Domain.Models.RaiinKubunMst
{
    public class RaiinKbnItemModel
    {
        public RaiinKbnItemModel(int hpId, int grpCd, int kbnCd, long seqNo, string itemCd, int isExclude, int isDeleted,int sortNo)
        {
            HpId = hpId;
            GrpCd = grpCd;
            KbnCd = kbnCd;
            SeqNo = seqNo;
            ItemCd = itemCd;
            IsExclude = isExclude;
            IsDeleted = isDeleted;
            SortNo = sortNo;
        }

        public int HpId { get; private set; }
        public int GrpCd { get; private set; }

        public int KbnCd { get; private set; }

        public long SeqNo { get; private set; }

        public string ItemCd { get; private set; }

        public int IsExclude { get; private set; }

        public int IsDeleted { get; private set; }
        public int SortNo { get; private set; }

    }
}
