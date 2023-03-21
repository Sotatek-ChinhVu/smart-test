namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class OdrInfItem
    {
        public OdrInfItem(int hpId, long ptId, int sinDate, long raiinNo, long rpNo, long rpEdaNo, int hokenPid, int odrKouiKbn, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int daysCnt, int sortNo, int isDeleted, List<OdrInfDetailItem> detailInfoList)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            HokenPid = hokenPid;
            OdrKouiKbn = odrKouiKbn;
            InoutKbn = inoutKbn;
            SikyuKbn = sikyuKbn;
            SyohoSbt = syohoSbt;
            SanteiKbn = santeiKbn;
            DaysCnt = daysCnt;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            DetailInfoList = detailInfoList;
        }

        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public long RpNo { get; set; }

        public long RpEdaNo { get; set; }

        public int HokenPid { get; set; }

        public int OdrKouiKbn { get; set; }

        public int InoutKbn { get; set; }

        public int SikyuKbn { get; set; }

        public int SyohoSbt { get; set; }

        public int SanteiKbn { get; set; }

        public int DaysCnt { get; set; }

        public int SortNo { get; set; }

        public int IsDeleted { get; set; }
        public List<OdrInfDetailItem> DetailInfoList { get; private set; }
    }
}
