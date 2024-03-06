using Domain.Models.ReceSeikyu;
using Helper.Messaging;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.RecalculateInSeikyuPending;

public class RecalculateInSeikyuPendingInputData : IInputData<RecalculateInSeikyuPendingOutputData>
{
    public RecalculateInSeikyuPendingInputData(int hpId, int userId, List<ReceInfo> receInfList, IMessenger messenger)
    {
        HpId = hpId;
        UserId = userId;
        ReceInfList = receInfList;
        Messenger = messenger;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<ReceInfo> ReceInfList { get; private set; }

    public IMessenger Messenger { get; set; }
}
