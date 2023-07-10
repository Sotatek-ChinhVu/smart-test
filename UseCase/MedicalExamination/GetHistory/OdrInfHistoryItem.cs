using System.Text.Json.Serialization;
using UseCase.OrdInfs.GetListTrees;

namespace UseCase.MedicalExamination.GetHistory
{
    public class OdrInfHistoryItem : OdrInfItem
    {
        public OdrInfHistoryItem(int hpId, long raiinNo, long rpNo, long rpEdaNo, long ptId, int sinDate, int hokenPid, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, long id, int groupOdrKouiKbn, List<OdrInfDetailItem> odrDetails, DateTime createDate, int createId, string createName, DateTime updateDate, int isDeleted, string createMachine, string updateMachine, string updateName) : base(hpId, raiinNo, rpNo, rpEdaNo, ptId, sinDate, hokenPid, odrKouiKbn, rpName, inoutKbn, sikyuKbn, syohoSbt, santeiKbn, tosekiKbn, daysCnt, sortNo, id, groupOdrKouiKbn, odrDetails, createDate, createId, createName, isDeleted, updateDate, createMachine, updateMachine, updateName)
        {
        }

        [JsonPropertyName("updateDateDisplay")]
        public string UpdateDateDisplay
        {
            get => UpdateDate.ToString("yyyy/MM/dd HH:mm");
        }

        [JsonPropertyName("createDateDisplay")]
        public string CreateDateDisplay
        {
            get => CreateDate.ToString("yyyy/MM/dd HH:mm");
        }
    }
}
