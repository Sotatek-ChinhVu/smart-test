namespace Domain.Models.RaiinKbn
{
    public class RaiinKbnDetailModel
    {
        public RaiinKbnDetailModel(int hpId, int grpCd, int kbnCd, int sortNo, string kbnName, string colorCd, int isConfirmed, int isAuto, int isAutoDelete, int isDeleted)
        {
            HpId = hpId;
            GrpCd = grpCd;
            KbnCd = kbnCd;
            SortNo = sortNo;
            KbnName = kbnName;
            ColorCd = colorCd;
            IsConfirmed = isConfirmed;
            IsAuto = isAuto;
            IsAutoDelete = isAutoDelete;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public int GrpCd { get; private set; }

        public int KbnCd { get; private set; }

        public int SortNo { get; private set; }

        public string KbnName { get; private set; }

        public string ColorCd { get; private set; }

        public int IsConfirmed { get; private set; }

        public int IsAuto { get; private set; }

        public int IsAutoDelete { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsTodayOrderChecked => IsAuto == 1 || IsAuto == 3;

        public bool IsNextOrderChecked => IsAuto == 2 || IsAuto == 3;
    }
}
