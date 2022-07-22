using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MedicalExamination.GetHistory
{
    public class OdrInfHistoryItem : OdrInfItem
    {
        public OdrInfHistoryItem(int hpId, long raiinNo, long rpNo, long rpEdaNo, long ptId, int sinDate, int hokenPid, int odrKouiKbn, string? rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, long id, int groupOdrKouiKbn, List<OdrInfDetailItem> odrDetails) : base(hpId, raiinNo, rpNo, rpEdaNo, ptId, sinDate, hokenPid, odrKouiKbn, rpName, inoutKbn, sikyuKbn, syohoSbt, santeiKbn, tosekiKbn, daysCnt, sortNo, id, groupOdrKouiKbn, odrDetails)
        {
        }

        public int IsDeleted { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public DateTime CreateDate { get; private set; }
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
