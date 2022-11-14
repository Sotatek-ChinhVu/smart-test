using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.Get
{
    public class GetNextOrderInputData : IInputData<GetNextOrderOutputData>
    {
        public long PtId { get; private set; }

        public int HpId { get; private set; }

        public long RsvkrtNo { get; private set; }

        public int SinDate { get; private set; }

        public int Type { get; private set; }

        public GetNextOrderInputData(long ptId, int hpId, long rsvkrtNo, int sinDate, int type)
        {
            PtId = ptId;
            HpId = hpId;
            RsvkrtNo = rsvkrtNo;
            SinDate = sinDate;
            Type = type;
        }
    }
}
