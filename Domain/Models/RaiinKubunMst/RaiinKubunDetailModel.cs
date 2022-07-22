namespace Domain.Models.RaiinKubunMst
{
    public class RaiinKubunDetailModel
    {
        public int GroupId { get; private set; }

        public int KubunCd { get; private set; }

        public int SortNo { get; private set; }

        public string KubunName { get; private set; }

        public string ColorCd { get; private set; }

        public bool IsConfirmed { get; private set; }

        public bool IsAuto { get; private set; }

        public bool IsAutoDeleted { get; private set; }

        public bool IsDeleted { get; private set; }

        public RaiinKubunDetailModel(int groupId, int kubunCd, int sortNo, string kubunName, string colorCd, bool isConfirmed, bool isAuto, bool isAutoDeleted, bool isDeleted)
        {
            GroupId = groupId;
            KubunCd = kubunCd;
            SortNo = sortNo;
            KubunName = kubunName;
            ColorCd = colorCd;
            IsConfirmed = isConfirmed;
            IsAuto = isAuto;
            IsAutoDeleted = isAutoDeleted;
            IsDeleted = isDeleted;
        }
    }
}
