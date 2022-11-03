using CommonChecker.Models.OrdInfDetailModel;
using CommonChecker.Types;

namespace CommonChecker.Models.OrdInf
{
    public class OrdInfoModel : IOdrInfoModel<OrdInfoDetailModel>
    {
        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public long RpNo { get; private set; }
        public long RpEdaNo { get; private set; }
        public long PtId { get; set; }
        public int SinDate { get; private set; }
        public int HokenPid { get; private set; }
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
        public long Id { get; private set; }
        public DateTime CreateDate { get; private set; }
        public int CreateId { get; private set; }
        public string CreateName { get; private set; }

        public List<OrdInfoDetailModel> OrdInfDetails { get; private set; }
        public DateTime UpdateDate { get; private set; }

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
