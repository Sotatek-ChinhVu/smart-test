using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Delete
{
    public class DeleteReceptionInputData : IInputData<DeleteReceptionOutputData>
    {
        public DeleteReceptionInputData(int hpId, int userId, List<long> raiinNos)
        {
            HpId = hpId;
            UserId = userId;
            RaiinNos = raiinNos;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<long> RaiinNos { get; private set; }

    }
}
