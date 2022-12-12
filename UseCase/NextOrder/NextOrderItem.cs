namespace UseCase.NextOrder
{
    public class NextOrderItem
    {
        public NextOrderItem(long rsvkrtNo, int rsvkrtKbn, int rsvDate, string rsvName, int sortNo, int isDeleted, List<RsvKrtByomeiItem> rsvKrtByomeiItems, RsvKrtKarteInfItem rsvkrtKarteInf, List<RsvKrtOrderInfItem> rsvKrtOrderInfItems)
        {
            RsvkrtNo = rsvkrtNo;
            RsvkrtKbn = rsvkrtKbn;
            RsvDate = rsvDate;
            RsvName = rsvName;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            this.rsvKrtByomeiItems = rsvKrtByomeiItems;
            this.rsvkrtKarteInf = rsvkrtKarteInf;
            this.rsvKrtOrderInfItems = rsvKrtOrderInfItems;
        }

        public long RsvkrtNo { get; private set; }

        public int RsvkrtKbn { get; private set; }

        public int RsvDate { get; private set; }

        public string RsvName { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }

        public List<RsvKrtByomeiItem> rsvKrtByomeiItems { get; private set; }

        public RsvKrtKarteInfItem rsvkrtKarteInf { get; private set; }

        public List<RsvKrtOrderInfItem> rsvKrtOrderInfItems { get; private set; }
    }
}
