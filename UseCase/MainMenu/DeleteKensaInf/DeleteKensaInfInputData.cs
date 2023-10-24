using Domain.Models.KensaIrai;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.DeleteKensaInf;

public class DeleteKensaInfInputData : IInputData<DeleteKensaInfOutputData>
{
    public DeleteKensaInfInputData(int hpId, int userId, List<KensaInfModel> kensaInfList)
    {
        HpId = hpId;
        UserId = userId;
        KensaInfList = kensaInfList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<KensaInfModel> KensaInfList { get; private set; }
}
