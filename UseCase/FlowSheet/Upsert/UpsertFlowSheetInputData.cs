using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.Upsert
{
    public class UpsertFlowSheetInputData : IInputData<UpsertFlowSheetOutputData>
    {
        public UpsertFlowSheetInputData(long rainNo, long ptId, int sinDate, int tagNo, int cmtKbn, string text, int seqNo)
        {
            RainNo = rainNo;
            PtId = ptId;
            SinDate = sinDate;
            TagNo = tagNo;
            CmtKbn = cmtKbn;
            Text = text;
            SeqNo = seqNo;
        }

        public long RainNo { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int TagNo { get; private set; }

        public int CmtKbn { get; private set; }

        public string Text { get; private set; }
        public int SeqNo { get; private set; }
    }
}
