using System.Text.Json.Serialization;

namespace UseCase.OrdInfs.GetListTrees
{
    public class OdrInfItem
    {
        public OdrInfItem(int hpId, long raiinNo, long rpNo, long rpEdaNo, long ptId, int sinDate, int hokenPid, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, long id, int groupOdrKouiKbn, List<OdrInfDetailItem> odrDetails, DateTime createDate, int createId, string createName, int isDeleted, DateTime updateDate, string createMachine, string updateMachine)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            PtId = ptId;
            SinDate = sinDate;
            HokenPid = hokenPid;
            OdrKouiKbn = odrKouiKbn;
            RpName = rpName;
            InoutKbn = inoutKbn;
            SikyuKbn = sikyuKbn;
            SyohoSbt = syohoSbt;
            SanteiKbn = santeiKbn;
            TosekiKbn = tosekiKbn;
            DaysCnt = daysCnt;
            SortNo = sortNo;
            Id = id;
            GroupOdrKouiKbn = groupOdrKouiKbn;
            OdrDetails = odrDetails;
            CreateDate = createDate;
            CreateId = createId;
            CreateName = createName;
            IsDeleted = isDeleted;
            UpdateDate = updateDate;
            CreateMachine = createMachine;
            UpdateMachine = updateMachine;
        }

        [JsonPropertyName("hpId")]
        public int HpId { get; private set; }

        [JsonPropertyName("raiinNo")]
        public long RaiinNo { get; private set; }

        [JsonPropertyName("rpNo")]
        public long RpNo { get; private set; }

        [JsonPropertyName("rpEdaNo")]
        public long RpEdaNo { get; private set; }

        [JsonPropertyName("ptId")]
        public long PtId { get; private set; }

        [JsonPropertyName("sinDate")]
        public int SinDate { get; private set; }

        [JsonPropertyName("hokenPid")]
        public int HokenPid { get; private set; }

        [JsonPropertyName("odrKouiKbn")]
        public int OdrKouiKbn { get; private set; }

        [JsonPropertyName("rpName")]
        public string RpName { get; private set; }

        [JsonPropertyName("inoutKbn")]
        public int InoutKbn { get; private set; }

        [JsonPropertyName("sikyuKbn")]
        public int SikyuKbn { get; private set; }

        [JsonPropertyName("syohoSbt")]
        public int SyohoSbt { get; private set; }

        [JsonPropertyName("santeiKbn")]
        public int SanteiKbn { get; private set; }

        [JsonPropertyName("tosekiKbn")]
        public int TosekiKbn { get; private set; }

        [JsonPropertyName("daysCnt")]
        public int DaysCnt { get; private set; }

        [JsonPropertyName("sortNo")]
        public int SortNo { get; private set; }

        [JsonPropertyName("id")]
        public long Id { get; private set; }

        [JsonPropertyName("groupOdrKouiKbn")]
        public int GroupOdrKouiKbn { get; private set; }

        [JsonPropertyName("odrDetails")]
        public List<OdrInfDetailItem> OdrDetails { get; private set; }

        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; private set; }

        [JsonPropertyName("createId")]
        public int CreateId { get; private set; }

        [JsonPropertyName("createName")]
        public string CreateName { get; private set; }

        [JsonPropertyName("isDeleted")]
        public int IsDeleted { get; private set; }

        [JsonPropertyName("updateDate")]
        public DateTime UpdateDate { get; private set; }

        [JsonPropertyName("createMachine")]
        public string CreateMachine { get; private set; }

        [JsonPropertyName("updateMachine")]
        public string UpdateMachine { get; private set; }
    }
}
