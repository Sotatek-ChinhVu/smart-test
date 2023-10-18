﻿namespace UseCase.Lock.Unlock
{
    public class LockCalcStatusInputItem
    {
        public LockCalcStatusInputItem(long calcId, long ptId, long ptNum, int sinDate, DateTime createDate, string createMachine, int createId)
        {
            CalcId = calcId;
            PtId = ptId;
            PtNum = ptNum;
            SinDate = sinDate;
            CreateDate = createDate;
            CreateMachine = createMachine;
            CreateId = createId;
        }

        public long CalcId { get; private set; }

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

        public string CreateMachine { get; set; }

        public int CreateId {  get; private set; }
    }
}