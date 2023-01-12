namespace UseCase.NextOrder
{
    public class NextOrderLabelItem
    {
        public NextOrderLabelItem(long rsvkrtNo, int rsvkrtKbn, int rsvDate, string rsvName, int sortNo, int isDeleted)
        {
            RsvkrtNo = rsvkrtNo;
            RsvkrtKbn = rsvkrtKbn;
            RsvDate = rsvDate;
            RsvName = rsvName;
            SortNo = sortNo;
            IsDeleted = isDeleted;
        }

        public long RsvkrtNo { get; private set; }

        public int RsvkrtKbn { get; private set; }

        public int RsvDate { get; private set; }

        public string RsvName { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
