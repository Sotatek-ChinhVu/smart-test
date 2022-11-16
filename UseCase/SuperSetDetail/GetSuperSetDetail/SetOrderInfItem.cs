namespace UseCase.SuperSetDetail.GetSuperSetDetail
{
    public class SetOrderInfItem
    {
        public SetOrderInfItem(long id, int hpId, int setCd, long rpNo, long rpEdaNo, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, int isDeleted, DateTime createDate, int createId, string createName, int groupKoui, bool isSelfInjection, bool isDrug, bool isInjection, bool isKensa, bool isShohoComment, bool isShohoBiko, bool isShohosenComment, List<SetOrderInfDetailItem> ordInfDetails)
        {
            Id = id;
            HpId = hpId;
            SetCd = setCd;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            OdrKouiKbn = odrKouiKbn;
            RpName = rpName;
            InoutKbn = inoutKbn;
            SikyuKbn = sikyuKbn;
            SyohoSbt = syohoSbt;
            SanteiKbn = santeiKbn;
            TosekiKbn = tosekiKbn;
            DaysCnt = daysCnt;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            CreateDate = createDate;
            CreateId = createId;
            CreateName = createName;
            GroupKoui = groupKoui;
            IsSelfInjection = isSelfInjection;
            IsDrug = isDrug;
            IsInjection = isInjection;
            IsKensa = isKensa;
            IsShohoComment = isShohoComment;
            IsShohoBiko = isShohoBiko;
            IsShohosenComment = isShohosenComment;
            OrdInfDetails = ordInfDetails;
        }

        public long Id { get; private set; }

        public int HpId { get; private set; }

        public int SetCd { get; private set; }

        public long RpNo { get; private set; }

        public long RpEdaNo { get; private set; }

        public int OdrKouiKbn { get; private set; }

        public string RpName { get; private set; }

        public int InoutKbn { get; private set; }

        public int SikyuKbn { get; private set; }

        public int SyohoSbt { get; private set; }

        public int SanteiKbn { get; private set; }

        public int TosekiKbn { get; private set; }

        public int DaysCnt { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }

        public DateTime CreateDate { get; private set; }

        public int CreateId { get; private set; }

        public string CreateName { get; private set; }

        public int GroupKoui { get; private set; }

        public bool IsSelfInjection { get; private set; }

        public bool IsDrug { get; private set; }

        public bool IsInjection { get; private set; }

        public bool IsKensa { get; private set; }

        public bool IsShohoComment { get; private set; }

        public bool IsShohoBiko { get; private set; }

        public bool IsShohosenComment { get; private set; }

        public List<SetOrderInfDetailItem> OrdInfDetails { get; private set; }
    }
}
