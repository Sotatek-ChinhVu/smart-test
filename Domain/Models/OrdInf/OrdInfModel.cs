using Domain.Common;
using Domain.Models.OrdInfDetails;
using Domain.Types;
using static Helper.Constants.OrderInfConst;

namespace Domain.Models.OrdInfs
{
    public class OrdInfModel : IOdrInfModel<OrdInfDetailModel>
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

        public GroupKoui GroupKoui { get; private set; }
        public List<OrdInfDetailModel> OrdInfDetails { get; private set; }
        public DateTime UpdateDate { get; private set; }

        public OrdInfModel(int hpId, long raiinNo, long rpNo, long rpEdaNo, long ptId, int sinDate, int hokenPid, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, int isDeleted, long id, List<OrdInfDetailModel> ordInfDetails, DateTime createDate, int createId, string createName, DateTime updateDate)
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
            GroupKoui = GroupKoui.From(odrKouiKbn);
            OrdInfDetails = ordInfDetails;
            CreateDate = createDate;
            CreateId = createId;
            CreateName = createName;
            UpdateDate = updateDate;
        }

        public OrdInfModel()
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
            GroupKoui = GroupKoui.From(0);
            OrdInfDetails = new List<OrdInfDetailModel>();
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

        public KeyValuePair<string, OrdInfValidationStatus> Validation(int flag)
        {

            #region Validate common

            if (flag == 0)
            {
                if (RaiinNo <= 0)
                {
                    return new("-1", OrdInfValidationStatus.InvalidRaiinNo);
                }
                if (PtId <= 0)
                {
                    return new("-1", OrdInfValidationStatus.InvalidPtId);
                }
                if (SinDate <= 0)
                {
                    return new("-1", OrdInfValidationStatus.InvalidSinDate);
                }
                if (HokenPid <= 0)
                {
                    return new("-1", OrdInfValidationStatus.InvalidHokenPId);
                }
            }

            var count = 0;
            foreach (var item in OrdInfDetails)
            {
                var validate = item.Validation(flag);
                if (validate != OrdInfValidationStatus.Valid)
                {
                    return new(count.ToString(), OrdInfValidationStatus.InvalidHokenPId);
                }
                count++;
            }
            #endregion

            #region Validate business
            var refillSetting = OrdInfDetails?.FirstOrDefault()?.RefillSetting ?? 999;
            return ValidationIOdrInfModel<OrdInfModel, OrdInfDetailModel>.Validation(this, flag, SinDate, refillSetting);
            #endregion
        }
        public OrdInfModel ChangeOdrDetail(List<OrdInfDetailModel> ordInfDetails)
        {
            OrdInfDetails = ordInfDetails;
            return this;
        }
    }
}
