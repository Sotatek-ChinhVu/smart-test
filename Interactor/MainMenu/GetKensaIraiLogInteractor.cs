using Domain.Models.KensaIrai;
using UseCase.MainMenu.GetKensaIraiLog;

namespace Interactor.MainMenu;

public class GetKensaIraiLogInteractor : IGetKensaIraiLogInputPort
{
    private readonly IKensaIraiRepository _kensaIraiRepository;

    public GetKensaIraiLogInteractor(IKensaIraiRepository kensaIraiRepository)
    {
        _kensaIraiRepository = kensaIraiRepository;
    }

    public GetKensaIraiLogOutputData Handle(GetKensaIraiLogInputData inputData)
    {
        try
        {
            var result = _kensaIraiRepository.GetKensaIraiLogModels(inputData.HpId, inputData.StartDate, inputData.EndDate);
            return new GetKensaIraiLogOutputData(result, GetKensaIraiLogStatus.Successed);
        }
        finally
        {
            _kensaIraiRepository.ReleaseResource();
        }
    }
}
