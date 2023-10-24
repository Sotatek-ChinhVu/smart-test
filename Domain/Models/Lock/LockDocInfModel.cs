namespace Domain.Models.Lock
{
    public class LockDocInfModel
    {
        public LockDocInfModel(long ptId, long ptNum, int sinDate, long raiinNo, int seqNo, int categoryCd, string fileName, string dspFileName, int isLocked, DateTime lockDate, int lockId, string lockMachine, int isDeleted)
        {
            PtId = ptId;
            PtNum = ptNum;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            SeqNo = seqNo;
            CategoryCd = categoryCd;
            FileName = fileName;
            DspFileName = dspFileName;
            IsLocked = isLocked;
            LockDate = lockDate;
            LockId = lockId;
            LockMachine = lockMachine;
            IsDeleted = isDeleted;
        }

        public long PtId { get; private set; }

        public long PtNum { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int SeqNo { get; private set; }

        public int CategoryCd { get; private set; }

        public string DspFileName { get; private set; }

        public int IsLocked { get; private set; }

        public DateTime LockDate { get; private set; }

        public int LockId { get; private set; }

        public string LockMachine { get; private set; }

        public string FileName { get; private set; }

        public int IsDeleted { get; private set; }

        public string FunctionName
        {
            get
            {
                return DspFileName != null ? "[文書]" + DspFileName : string.Empty;
            }
        }
    }
}
