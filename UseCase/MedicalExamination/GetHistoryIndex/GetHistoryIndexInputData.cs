using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetHistoryIndex
{
    public class GetHistoryIndexInputData : IInputData<GetHistoryIndexOutputData>
    {
        public GetHistoryIndexInputData(int hpId, int userId, long ptId, int filterId, int isDeleted, long raiinNo, List<Tuple<long, bool>> raiinNos)
        {
            HpId = hpId;
            UserId = userId;
            PtId = ptId;
            FilterId = filterId;
            IsDeleted = isDeleted;
            RaiinNo = raiinNo;
            RaiinNos = raiinNos;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public long PtId { get; private set; }

        public int FilterId { get; private set; }

        public int IsDeleted { get; private set; }

        public long RaiinNo { get; private set; }

        public List<Tuple<long, bool>> RaiinNos { get; private set; } = new();
    }
}
