namespace CommonChecker.Models
{
    public class PtAlrgyFoodModel
    {
        public PtAlrgyFoodModel(int hpId, long ptId, int seqNo, int sortNo, string alrgyKbn, int startDate, int endDate, string cmt, int isDeleted, string foodName)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            SortNo = sortNo;
            AlrgyKbn = alrgyKbn;
            StartDate = startDate;
            EndDate = endDate;
            Cmt = cmt;
            IsDeleted = isDeleted;
            FoodName = foodName;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SeqNo { get; private set; }

        public int SortNo { get; private set; }

        public string FoodName { get; private set; }

        public string AlrgyKbn { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public string Cmt { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
