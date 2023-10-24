using Domain.Models.KensaIrai;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.CreateDataKensaIraiRenkei;

public class CreateDataKensaIraiRenkeiOutputData : IOutputData
{
    public CreateDataKensaIraiRenkeiOutputData(CreateDataKensaIraiRenkeiStatus status, List<KensaIraiModel> kensaIraiList)
    {
        Status = status;
        KensaIraiList = kensaIraiList;
    }

    public List<KensaIraiModel> KensaIraiList { get; private set; }

    public CreateDataKensaIraiRenkeiStatus Status { get; private set; }
}
