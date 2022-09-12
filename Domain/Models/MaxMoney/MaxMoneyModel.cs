using Helper.Common;
using Helper.Extension;

namespace Domain.Models.MaxMoney
{
    public class MaxMoneyModel
    {
        public MaxMoneyModel(int hokenPid, string sortKey, long raiinNo, int futanGaku, int totalGaku, string biko, int isDeleted)
        {
            HokenPid = hokenPid;
            SortKey = sortKey;
            RaiinNo = raiinNo;
            FutanGaku = futanGaku;
            TotalGaku = totalGaku;
            Biko = biko;
            IsDeleted = isDeleted;
            TotalMoney = totalMoney;
        }

        public long Id { get; private set; }
        public int KohiId { get; private set; }
        public int SinDate { get; private set; }
        public int SinDateY
        {
            get => CIUtil.Copy(SinDate.AsString(), 1, 4).AsInteger();
        }

        public int SinDateM
        {
            get => CIUtil.Copy(SinDate.AsString(), 5, 2).AsInteger();
        }

        public int SinDateD
        {
            get => CIUtil.Copy(SinDate.AsString(), 7, 2).AsInteger();
        }
        public int HokenPid { get; private set; }
        public string SortKey { get; private set; }
        public long RaiinNo { get; private set; }
        public int FutanGaku { get; private set; }
        public int TotalGaku { get; private set; }
        public string Biko { get; private set; }
        public int IsDeleted { get; private set; }
        public string Code
        {
            get => (RaiinNo == 0) ? "他院分" : string.Empty;
        }
    }
}
