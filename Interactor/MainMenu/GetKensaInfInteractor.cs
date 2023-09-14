using Domain.Models.KensaIrai;
using UseCase.MainMenu.GetKensaInf;

namespace Interactor.MainMenu;

public class GetKensaInfInteractor : IGetKensaInfInputPort
{
    private readonly IKensaIraiRepository _kensaIraiRepository;

    public GetKensaInfInteractor(IKensaIraiRepository kensaIraiRepository)
    {
        _kensaIraiRepository = kensaIraiRepository;
    }

    public GetKensaInfOutputData Handle(GetKensaInfInputData inputData)
    {
        try
        {
            var result = _kensaIraiRepository.GetKensaInfModels(inputData.HpId, inputData.StartDate, inputData.EndDate, inputData.CenterCd);
            return new GetKensaInfOutputData(GetKensaInfStatus.Successed, result);
        }
        finally
        {
            _kensaIraiRepository.ReleaseResource();
        }
    }
}
