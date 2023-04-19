using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Delete
{
    public class DeleteReceptionOutputData : IOutputData
    {
        public DeleteReceptionOutputData(DeleteReceptionStatus status, List<DeleteReceptionItem> deleteReceptionItems)
        {
            Status = status;
            DeleteReceptionItems = deleteReceptionItems;
        }

        public List<DeleteReceptionItem> DeleteReceptionItems { get; private set; }

        public DeleteReceptionStatus Status { get; private set; }
    }

    public class DeleteReceptionItem
    {
        public DeleteReceptionItem(int sinDate, long raiinNo, long ptId)
        {
            SinDate = sinDate;
            RaiinNo = raiinNo;
            PtId = ptId;
        }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public long PtId { get; private set; }
    }
}
