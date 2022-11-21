using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.GetList
{
    public class GetNextOrderListInputData : IInputData<GetNextOrderListOutputData>
    {
        public GetNextOrderListInputData(long ptId, int hpId, int rsvkrtKbn, bool isDeleted)
        {
            PtId = ptId;
            HpId = hpId;
            RsvkrtKbn = rsvkrtKbn;
            IsDeleted = isDeleted;
        }

        public long PtId { get; private set; }

        public int HpId { get; private set; }

        public int RsvkrtKbn { get; private set; }

        public bool IsDeleted { get; private set; }
    }
}
