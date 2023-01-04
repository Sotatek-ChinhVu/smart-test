using CommonChecker.Models.OrdInfDetailModel;
using CommonChecker.Types;

namespace CommonChecker.Models.OrdInf
{
    public class OrdInfoModel : IOdrInfoModel<OrdInfoDetailModel>
    {
        public int HpId { get; set; }
        public long RaiinNo { get; set; }
        public long RpNo { get; set; }
        public long RpEdaNo { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public int HokenPid { get; set; }
        public int OdrKouiKbn { get; set; }
        public string RpName { get; set; }
        public int InoutKbn { get; set; }
        public int SikyuKbn { get; set; }
        public int SyohoSbt { get; set; }
        public int SanteiKbn { get; set; }
        public int TosekiKbn { get; set; }
        public int DaysCnt { get; set; }
        public int SortNo { get; set; }
        public int IsDeleted { get; set; }
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }
        public string CreateName { get; set; }

        public List<OrdInfoDetailModel> OrdInfDetails { get; set; }
        public DateTime UpdateDate { get; set; }

        public OrdInfoModel(int hpId, long raiinNo, long rpNo, long rpEdaNo, long ptId, int sinDate, int hokenPid, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, int isDeleted, long id, List<OrdInfoDetailModel> ordInfDetails, DateTime createDate, int createId, string createName, DateTime updateDate)
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
            IsDeleted = isDeleted;
            Id = id;
            OrdInfDetails = ordInfDetails;
            CreateDate = createDate;
            CreateId = createId;
            CreateName = createName;
            UpdateDate = updateDate;
        }

        public OrdInfoModel()
        {
            HpId = 0;
            RaiinNo = 0;
            RpNo = 0;
            RpEdaNo = 0;
            PtId = 0;
            SinDate = 0;
            HokenPid = 0;
            OdrKouiKbn = 0;
            RpName = string.Empty;
            InoutKbn = 0;
            SikyuKbn = 0;
            SyohoSbt = 0;
            SanteiKbn = 0;
            TosekiKbn = 0;
            DaysCnt = 0;
            SortNo = 0;
            IsDeleted = 0;
            Id = 0;
            OrdInfDetails = new List<OrdInfoDetailModel>();
            CreateDate = DateTime.MinValue;
            CreateId = 0;
            CreateName = string.Empty;
            UpdateDate = DateTime.MinValue;
        }

        // 処方 - Drug
        public bool IsDrug
        {
            get
            {
                return OdrKouiKbn >= 21 && OdrKouiKbn <= 23;
            }
        }

        // 注射 - Injection
        public bool IsInjection
        {
            get
            {
                return OdrKouiKbn >= 30 && OdrKouiKbn <= 34;
            }
        }

        public List<OrdInfoDetailModel> OdrInfDetailModelsIgnoreEmpty
        {
            get
            {
                if (OrdInfDetails == null)
                {
                    return new List<OrdInfoDetailModel>();
                }
                return new List<OrdInfoDetailModel>(OrdInfDetails.Where(o => !o.IsEmpty).ToList());
            }
        }

        public OrdInfoModel ChangeOdrDetail(List<OrdInfoDetailModel> ordInfDetails)
        {
            OrdInfDetails = ordInfDetails;
            return this;
        }
    }
}
