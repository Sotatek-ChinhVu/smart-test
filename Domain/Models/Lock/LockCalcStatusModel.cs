namespace Domain.Models.Lock
{
    public class LockCalcStatusModel
    {
        public LockCalcStatusModel(long ptId, long ptNum, int sinDate, DateTime createDate)
        {
            PtId = ptId;
            PtNum = ptNum;
            SinDate = sinDate;
            CreateDate = createDate;
        }

        public long PtId { get; private set; }

        public long PtNum { get; private set; }

        public int SinDate { get; private set; }

        public DateTime CreateDate { get; private set; }

        public DateTime LockDate
        {
            get
            {
                return CreateDate;
            }
        }
    }
}
