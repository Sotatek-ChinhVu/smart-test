namespace Domain.Models.NextOrder
{
    public class NextOrderModel
    {
        public NextOrderModel(int hpId, long ptId, long rsvkrtNo, int rsvkrtKbn, int rsvDate, string rsvName, int isDeleted, int sortNo)
        {
            HpId = hpId;
            PtId = ptId;
            RsvkrtNo = rsvkrtNo;
            RsvkrtKbn = rsvkrtKbn;
            RsvDate = rsvDate;
            RsvName = rsvName;
            IsDeleted = isDeleted;
            SortNo = sortNo;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long RsvkrtNo { get; private set; }

        public int RsvkrtKbn { get; private set; }

        public int RsvDate { get; private set; }

        public string RsvName { get; private set; }

        public int IsDeleted { get; private set; }

        public int SortNo { get; private set; }
    }
}
