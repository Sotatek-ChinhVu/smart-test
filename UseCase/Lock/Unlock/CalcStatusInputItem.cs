namespace UseCase.Lock.Unlock
{
    public class CalcStatusInputItem
    {
        public CalcStatusInputItem(long ptId, long ptNum, int sinDate, int seikyuUp, int calcMode, int clearReceChk, int status, string biko)
        {
            PtId = ptId;
            PtNum = ptNum;
            SinDate = sinDate;
            SeikyuUp = seikyuUp;
            CalcMode = calcMode;
            ClearReceChk = clearReceChk;
            Status = status;
            Biko = biko;
        }

        public long PtId { get; private set; }

        public long PtNum { get; private set; }

        public int SinDate { get; private set; }

        public int SeikyuUp { get; private set; }

        public int CalcMode { get; private set; }

        public int ClearReceChk { get; private set; }

        public int Status { get; private set; }

        public string Biko { get; private set; }
    }
}
