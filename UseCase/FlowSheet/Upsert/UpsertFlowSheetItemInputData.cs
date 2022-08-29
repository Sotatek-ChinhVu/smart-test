namespace UseCase.FlowSheet.Upsert
{
    public class UpsertFlowSheetItemInputData
    {
        public UpsertFlowSheetItemInputData(long rainNo, long ptId, int sinDate, int tagNo, int cmtKbn, string text, long rainListCmtSeqNo, int rainListTagSeqNo)
        {
            RainNo = rainNo;
            PtId = ptId;
            SinDate = sinDate;
            TagNo = tagNo;
            CmtKbn = cmtKbn;
            Text = text;
            RainListCmtSeqNo = rainListCmtSeqNo;
            RainListTagSeqNo = rainListTagSeqNo;
        }

        public long RainNo { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int TagNo { get; private set; }

        public int CmtKbn { get; private set; }

        public string Text { get; private set; }


        public long RainListCmtSeqNo { get; set; }

        public int RainListTagSeqNo { get; set; }
    }
}
