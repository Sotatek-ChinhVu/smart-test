using Domain.Models.Online;
using UseCase.Online.GetOnlineConsent;

namespace Interactor.Online;

public class GetOnlineConsentInteractor : IGetOnlineConsentInputPort
{
    private readonly IOnlineRepository _onlineRepository;

    public GetOnlineConsentInteractor(IOnlineRepository onlineRepository)
    {
        _onlineRepository = onlineRepository;
    }

    public GetOnlineConsentOutputData Handle(GetOnlineConsentInputData inputData)
    {
        try
        {
            var result = _onlineRepository.GetOnlineConsentModel(inputData.PtId);
            return new GetOnlineConsentOutputData(GetOnlineConsentStatus.Successed, result);
        }
        finally
        {
            _onlineRepository.ReleaseResource();
        }
    }
}
