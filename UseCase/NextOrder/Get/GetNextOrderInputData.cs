using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.Get
{
    public class GetNextOrderInputData : IInputData<GetNextOrderOutputData>
    {
        public long PtId { get; private set; }

        public int HpId { get; private set; }

        public long RsvkrtNo { get; private set; }

        public int SinDate { get; private set; }

        public int RsvkrtKbn { get; private set; }

        public int UserId { get; private set; }

        public GetNextOrderInputData(long ptId, int hpId, long rsvkrtNo, int sinDate, int rsvkrtKbn, int userId)
        {
            PtId = ptId;
            HpId = hpId;
            RsvkrtNo = rsvkrtNo;
            SinDate = sinDate;
            RsvkrtKbn = rsvkrtKbn;
            UserId = userId;
        }
    }
}
