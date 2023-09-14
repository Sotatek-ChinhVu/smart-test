using Domain.Models.KensaIrai;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetKensaInf;

public class GetKensaInfOutputData : IOutputData
{
    public GetKensaInfOutputData(GetKensaInfStatus status, List<KensaInfModel> kensaInfModelList)
    {
        Status = status;
        KensaInfModelList = kensaInfModelList;
    }

    public GetKensaInfStatus Status { get; private set; }

    public List<KensaInfModel> KensaInfModelList { get; private set; }
}
