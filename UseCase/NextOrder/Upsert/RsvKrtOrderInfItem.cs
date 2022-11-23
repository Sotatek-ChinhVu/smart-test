namespace UseCase.NextOrder.Upsert
{
    public class RsvKrtOrderInfItem
    {
        public RsvKrtOrderInfItem(int hpId, long ptId, int rsvDate, long rsvkrtNo, long rpNo, long rpEdaNo, long id, int hokenPid, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int isDeleted, int sortNo, List<RsvKrtOrderInfDetailItem> rsvKrtOrderInfDetailItems)
        {
            HpId = hpId;
            PtId = ptId;
            RsvDate = rsvDate;
            RsvkrtNo = rsvkrtNo;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            Id = id;
            HokenPid = hokenPid;
            OdrKouiKbn = odrKouiKbn;
            RpName = rpName;
            InoutKbn = inoutKbn;
            SikyuKbn = sikyuKbn;
            SyohoSbt = syohoSbt;
            SanteiKbn = santeiKbn;
            TosekiKbn = tosekiKbn;
            DaysCnt = daysCnt;
            IsDeleted = isDeleted;
            SortNo = sortNo;
            RsvKrtOrderInfDetailItems = rsvKrtOrderInfDetailItems;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int RsvDate { get; private set; }

        public long RsvkrtNo { get; private set; }

        public long RpNo { get; private set; }

        public long RpEdaNo { get; private set; }

        public long Id { get; private set; }

        public int HokenPid { get; private set; }

        public int OdrKouiKbn { get; private set; }

        public string RpName { get; private set; }

        public int InoutKbn { get; private set; }

        public int SikyuKbn { get; private set; }

        public int SyohoSbt { get; private set; }

        public int SanteiKbn { get; private set; }

        public int TosekiKbn { get; private set; }

        public int DaysCnt { get; private set; }

        public int IsDeleted { get; private set; }

        public int SortNo { get; private set; }

        public List<RsvKrtOrderInfDetailItem> RsvKrtOrderInfDetailItems { get; private set; }
    }
}
