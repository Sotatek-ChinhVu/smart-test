using UseCase.Core.Sync.Core;

namespace UseCase.ReceiptCheck.ReceiptInfEdit
{
    public class DeleteReceiptInfEditInputData : IInputData<DeleteReceiptInfEditOutputData>
    {
        public DeleteReceiptInfEditInputData(int hpId, int userId, int ptId, int seikyuYm, int sinYm, int hokenId)
        {
            HpId = hpId;
            UserId = userId;
            PtId = ptId;
            SeikyuYm = seikyuYm;
            SinYm = sinYm;
            HokenId = hokenId;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public int PtId { get; private set; }

        public int SeikyuYm { get; private set; }

        public int SinYm { get; private set; }

        public int HokenId { get; private set; }
    }
}
