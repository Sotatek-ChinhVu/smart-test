using Domain.Common;
using Domain.Types;
using static Helper.Constants.OrderInfConst;

namespace Domain.Models.NextOrder
{
    public class RsvkrtOrderInfModel : IOdrInfModel<RsvKrtOrderInfDetailModel>
    {
        public RsvkrtOrderInfModel(int hpId, long ptId, int rsvDate, long rsvkrtNo, long rpNo, long rpEdaNo, long id, int hokenPid, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int isDeleted, int sortNo, DateTime createDate, int createId, string createName, List<RsvKrtOrderInfDetailModel> orderInfDetailModels)
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
            GroupKoui = GroupKoui.From(odrKouiKbn);
            CreateDate = createDate;
            CreateId = createId;
            CreateName = createName;
            OrdInfDetails = orderInfDetailModels;
        }
        public KeyValuePair<string, OrdInfValidationStatus> Validation(int flag)
        {

            #region Validate common

            if (flag == 0)
            {
                if (RsvkrtNo < 0)
                {
                    return new("-1", OrdInfValidationStatus.InvalidRaiinNo);
                }
                if (PtId <= 0)
                {
                    return new("-1", OrdInfValidationStatus.InvalidPtId);
                }
                if (RsvDate <= 0)
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
                    return new(count.ToString(), validate);
                }
                count++;
            }
            #endregion

            #region Validate business
            var refillSetting = OrdInfDetails?.FirstOrDefault()?.RefillSetting ?? 999;
            return ValidationIOdrInfModel<RsvkrtOrderInfModel, RsvKrtOrderInfDetailModel>.Validation(this, flag, RsvDate, refillSetting);
            #endregion
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

        public GroupKoui GroupKoui { get; private set; }

        public DateTime CreateDate { get; private set; }

        public int CreateId { get; private set; }

        public string CreateName { get; private set; }

        public bool IsAutoAddItem { get; private set; }

        public List<RsvKrtOrderInfDetailModel> OrdInfDetails { get; private set; }

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
    }
}
