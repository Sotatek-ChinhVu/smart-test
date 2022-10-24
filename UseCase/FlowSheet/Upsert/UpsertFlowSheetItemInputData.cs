namespace UseCase.FlowSheet.Upsert
{
    public class UpsertFlowSheetItemInputData
    {
        public UpsertFlowSheetItemInputData(long rainNo, long ptId, int sinDate, string value, bool flag)
        {
            RainNo = rainNo;
            PtId = ptId;
            SinDate = sinDate;
            Flag = flag;
            Value = value;
        }
        public bool Flag { get; private set; }

        public long RainNo { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public string Value { get; private set; }
    }
}
