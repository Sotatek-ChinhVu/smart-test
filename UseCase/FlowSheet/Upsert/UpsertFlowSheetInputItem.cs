using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.Upsert
{
    public class UpsertFlowSheetInputItem : IInputData<UpsertFlowSheetOutputData>
    {
        public UpsertFlowSheetInputItem(long rainNo, long ptId, int sinDate, int tagNo, int cmtKbn, string text, int tagSeqNo, int cmtSeqNo)
        {
            RainNo = rainNo;
            PtId = ptId;
            SinDate = sinDate;
            TagNo = tagNo;
            CmtKbn = cmtKbn;
            Text = text;
            TagSeqNo = tagSeqNo;
            CmtSeqNo = cmtSeqNo;
        }

        public long RainNo { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int TagNo { get; private set; }

        public int CmtKbn { get; private set; }

        public string Text { get; private set; }
        public int TagSeqNo { get; private set; }
        public int CmtSeqNo { get; private set; }
    }
}
