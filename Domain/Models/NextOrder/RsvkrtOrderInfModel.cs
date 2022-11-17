using Domain.Types;

namespace Domain.Models.NextOrder
{
    public class RsvkrtOrderInfModel
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
            OrderInfDetailModels = orderInfDetailModels;
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

        public List<RsvKrtOrderInfDetailModel> OrderInfDetailModels { get; private set; }
    }
}
