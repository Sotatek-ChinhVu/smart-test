using Domain.Models.KensaIrai;
using UseCase.MainMenu.GetKensaIrai;

namespace Interactor.MainMenu;

public class GetKensaIraiInteractor : IGetKensaIraiInputPort
{
    private readonly IKensaIraiRepository _kensaIraiRepository;

    public GetKensaIraiInteractor(IKensaIraiRepository kensaIraiRepository)
    {
        _kensaIraiRepository = kensaIraiRepository;
    }

    public GetKensaIraiOutputData Handle(GetKensaIraiInputData inputData)
    {
        try
        {
            List<KensaIraiModel> result;
            if (inputData.SearchByList)
            {
                result = _kensaIraiRepository.GetKensaIraiModels(inputData.HpId, inputData.KensaInfModelList);
            }
            else
            {
                result = _kensaIraiRepository.GetKensaIraiModels(inputData.HpId, inputData.PtId, inputData.StartDate, inputData.EndDate, inputData.KensaCenterMstCenterCd, inputData.KensaCenterMstPrimaryKbn);
            }
            return new GetKensaIraiOutputData(GetKensaIraiStatus.Successed, result);
        }
        finally
        {
            _kensaIraiRepository.ReleaseResource();
        }
    }
}
