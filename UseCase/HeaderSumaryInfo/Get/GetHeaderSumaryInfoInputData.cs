using UseCase.Core.Sync.Core;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class GetHeaderSumaryInfoInputData : IInputData<GetHeaderSumaryInfoOutputData>
    {
        public GetHeaderSumaryInfoInputData(int hpId, int userId, long ptId, int sinDate, long rainNo)
        {
            HpId = hpId;
            UserId = userId;
            PtId = ptId;
            SinDate = sinDate;
            RainNo = rainNo;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public long RainNo { get; private set; }
    }
}
