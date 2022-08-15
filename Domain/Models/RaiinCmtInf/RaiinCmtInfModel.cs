namespace Domain.Models.RaiinCmtInf
{
    public class RaiinCmtInfModel
    {
        public RaiinCmtInfModel(int hpId, long raiinNo, int cmtKbn, long seqNo, long ptId, int sinDate, string text, int isDelete)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            CmtKbn = cmtKbn;
            SeqNo = seqNo;
            PtId = ptId;
            SinDate = sinDate;
            Text = text;
            IsDelete = isDelete;
        }

        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public int CmtKbn { get; private set; }
        public long SeqNo { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public string Text { get; private set; }
        public int IsDelete { get; private set; }
    }
}
