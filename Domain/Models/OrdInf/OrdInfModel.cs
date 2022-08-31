﻿using Domain.Models.OrdInfDetails;

namespace Domain.Models.OrdInfs
{
    public class OrdInfModel
    {
        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public long RpNo { get; private set; }
        public long RpEdaNo { get; private set; }
        public long PtId { get; set; }
        public int SinDate { get; private set; }
        public int HokenPid { get; private set; }
        public int OdrKouiKbn { get; private set; }
        public string? RpName { get; private set; }
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
        public int DisplaySetName { get; private set; }
        public int DisplayUserInput { get; private set; }
        public int DisplayTimeInput { get; private set; }
        public int DisplayDrugPrice { get; private set; }

        public GroupKoui GroupKoui { get; private set; }
        public List<OrdInfDetailModel> OrdInfDetails { get; private set; }

        public OrdInfModel(int hpId, long raiinNo, long rpNo, long rpEdaNo, long ptId, int sinDate, int hokenPid, int odrKouiKbn, string? rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, int isDeleted, long id, List<OrdInfDetailModel> ordInfDetails, DateTime createDate, int createId, int displaySetName, int displayUserInput, int displayTimeInput, int displayDrugPrice)
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
            DisplaySetName = displaySetName;
            DisplayUserInput = displayUserInput;
            DisplayTimeInput = displayTimeInput;
            DisplayDrugPrice = displayDrugPrice;
        }
    }
}
