using Domain.Models.NextOrder;

namespace UseCase.NextOrder.Get
{
    public class RsvKrtOrderInfItem
    {
        public RsvKrtOrderInfItem(RsvkrtOrderInfModel rsvkrtOrderInfModel)
        {
            HpId = rsvkrtOrderInfModel.HpId;
            PtId = rsvkrtOrderInfModel.PtId;
            RsvDate = rsvkrtOrderInfModel.RsvDate;
            RsvkrtNo = rsvkrtOrderInfModel.RsvkrtNo;
            RpNo = rsvkrtOrderInfModel.RpNo;
            RpEdaNo = rsvkrtOrderInfModel.RpEdaNo;
            Id = rsvkrtOrderInfModel.Id;
            HokenPid = rsvkrtOrderInfModel.HokenPid;
            OdrKouiKbn = rsvkrtOrderInfModel.OdrKouiKbn;
            RpName = rsvkrtOrderInfModel.RpName;
            InoutKbn = rsvkrtOrderInfModel.InoutKbn;
            SikyuKbn = rsvkrtOrderInfModel.SikyuKbn;
            SyohoSbt = rsvkrtOrderInfModel.SyohoSbt;
            SanteiKbn = rsvkrtOrderInfModel.SanteiKbn;
            TosekiKbn = rsvkrtOrderInfModel.TosekiKbn;
            DaysCnt = rsvkrtOrderInfModel.DaysCnt;
            IsDeleted = rsvkrtOrderInfModel.IsDeleted;
            GroupOdrKouiKbn = rsvkrtOrderInfModel.GroupKoui.Value;
            SortNo = rsvkrtOrderInfModel.SortNo;
            CreateDate = rsvkrtOrderInfModel.CreateDate;
            CreateId = rsvkrtOrderInfModel.CreateId;
            CreateName = rsvkrtOrderInfModel.CreateName;
            RsvKrtOrderInfDetailItems = rsvkrtOrderInfModel.OrderInfDetailModels.Select(od => new RsvKrtOrderInfDetailItem(od)).ToList();
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

        public int GroupOdrKouiKbn { get; private set; }

        public DateTime CreateDate { get; private set; }

        public int CreateId { get; private set; }

        public string CreateName { get; private set; }

        public List<RsvKrtOrderInfDetailItem> RsvKrtOrderInfDetailItems { get; private set; }
    }
}
