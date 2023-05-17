namespace UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder
{
    public class SetOrderInfItem
    {
        public SetOrderInfItem(int hpId, int setCd, long rpNo, long rpEdaNo, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int groupKoui, List<SetOrderInfDetailItem> ordInfDetails, DateTime createDate, string createName)
        {
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
            GroupKoui = groupKoui;
            OrdInfDetails = ordInfDetails;
            CreateDate = createDate;
            CreateName = createName;
        }

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

        public int GroupKoui { get; private set; }

        public List<SetOrderInfDetailItem> OrdInfDetails { get; private set; }

        public DateTime CreateDate { get; private set; }

        public string CreateName { get; private set; }
    }
}
