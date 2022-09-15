using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.GetListTrees
{
    public class GetOrdInfListTreeInputData : IInputData<GetOrdInfListTreeOutputData>
    {
        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public bool IsDeleted { get; private set; }
        public int UserId { get; private set; }

        public GetOrdInfListTreeInputData(long ptId, int hpId, long raiinNo, int sinDate, bool isDeleted, int userId)
        {
            PtId = ptId;
            HpId = hpId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            IsDeleted = isDeleted;
            UserId = userId;
        }
    }
}
