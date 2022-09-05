using UseCase.OrdInfs.GetListTrees;

namespace UseCase.MedicalExamination.GetHistory
{
    public class OdrInfHistoryItem : OdrInfItem
    {
        public OdrInfHistoryItem(int hpId, long raiinNo, long rpNo, long rpEdaNo, long ptId, int sinDate, int hokenPid, int odrKouiKbn, string? rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, long id, int groupOdrKouiKbn, List<OdrInfDetailItem> odrDetails, DateTime createDate, int createId, string createName) : base(hpId, raiinNo, rpNo, rpEdaNo, ptId, sinDate, hokenPid, odrKouiKbn, rpName, inoutKbn, sikyuKbn, syohoSbt, santeiKbn, tosekiKbn, daysCnt, sortNo, id, groupOdrKouiKbn, odrDetails, createDate, createId, createName)
        {
        }

        public int IsDeleted { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public string UpdateDateDisplay
        {
            get => UpdateDate.ToString("yyyy/MM/dd hh:mm");
        }
        public string CreateDateDisplay
        {
            get => CreateDate.ToString("yyyy/MM/dd hh:mm");
        }
    }
}
