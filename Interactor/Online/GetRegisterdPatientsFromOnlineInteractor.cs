using Domain.Models.Online;
using UseCase.Online.GetRegisterdPatientsFromOnline;

namespace Interactor.Online;

public class GetRegisterdPatientsFromOnlineInteractor : IGetRegisterdPatientsFromOnlineInputPort
{
    private readonly IOnlineRepository _onlineRepository;

    public GetRegisterdPatientsFromOnlineInteractor(IOnlineRepository onlineRepository)
    {
        _onlineRepository = onlineRepository;
    }

    public GetRegisterdPatientsFromOnlineOutputData Handle(GetRegisterdPatientsFromOnlineInputData inputData)
    {
        try
        {
            var onlineList = _onlineRepository.GetRegisterdPatientsFromOnline(inputData.SinDate, inputData.Id, inputData.ConfirmType);
            return new GetRegisterdPatientsFromOnlineOutputData(onlineList, GetRegisterdPatientsFromOnlineStatus.Successed);
        }
        finally
        {
            _onlineRepository.ReleaseResource();
        }
    }
}
