using Domain.Models.MstItem;

namespace Domain.Models.RaiinKubunMst
{
    public class RaiinKbnItemModel
    {
        public RaiinKbnItemModel(TenItemModel tenMstModel, int grpCd, int kbnCd, long seqNo, string itemCd, int isExclude, int isDeleted)
        {
            TenMstModel = tenMstModel;
            GrpCd = grpCd;
            KbnCd = kbnCd;
            SeqNo = seqNo;
            ItemCd = itemCd;
            IsExclude = isExclude;
            IsDeleted = isDeleted;
        }

        public TenItemModel TenMstModel { get; private set; }
        public int GrpCd { get; private set; }

        public int KbnCd { get; private set; }

        public long SeqNo { get; private set; }

        public string ItemCd { get; private set; }

        public int IsExclude { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
