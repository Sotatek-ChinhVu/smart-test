using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Delete
{
    public class DeleteReceptionInputData : IInputData<DeleteReceptionOutputData>
    {
        public DeleteReceptionInputData(int hpId, long ptId, int sinDate, bool flag, int userId, List<DeleteReception> raiinNos)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            Flag = flag;
            UserId = userId;
            RaiinNos = raiinNos;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public bool Flag { get; private set; }

        public int UserId { get; private set; }

        public List<DeleteReception> RaiinNos { get; private set; }
    }

    public class DeleteReception
    {
        public DeleteReception(long raiinNo, long oyaRaiinNo, int status)
        {
            RaiinNo = raiinNo;
            OyaRaiinNo = oyaRaiinNo;
            Status = status;
        }

        public long RaiinNo { get; private set; }

        public long OyaRaiinNo { get; private set; }

        public int Status { get; private set; }
    }
}
