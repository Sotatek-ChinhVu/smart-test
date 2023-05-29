using Helper.Constants;

namespace Domain.Models.RaiinListSetting
{
    public class RaiinListKouiModel
    {
        public RaiinListKouiModel(int hpId, int grpId, int kbnCd, long seqNo, int kouiKbnId, int isDeleted)
        {
            HpId = hpId;
            GrpId = grpId;
            KbnCd = kbnCd;
            SeqNo = seqNo;
            KouiKbnId = kouiKbnId;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public long SeqNo { get; private set; }

        public int KouiKbnId { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsChecked => IsDeleted == DeleteTypes.None;
    }
}
