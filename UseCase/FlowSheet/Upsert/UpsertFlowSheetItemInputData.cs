namespace UseCase.FlowSheet.Upsert
{
    public class UpsertFlowSheetItemInputData
    {
        public UpsertFlowSheetItemInputData(long rainNo, long ptId, int sinDate, int tagNo, string cmt)
        {
            RainNo = rainNo;
            PtId = ptId;
            SinDate = sinDate;
            TagNo = tagNo;
            Cmt = cmt;
        }
        public long RainNo { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int TagNo { get; private set; }

        public string Cmt { get; private set; }
    }
}
