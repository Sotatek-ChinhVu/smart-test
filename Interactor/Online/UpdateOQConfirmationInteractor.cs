using Domain.Models.Online;
using UseCase.Online.UpdateOQConfirmation;

namespace Interactor.Online;

public class UpdateOQConfirmationInteractor : IUpdateOQConfirmationInputPort
{
    private readonly IOnlineRepository _onlineRepository;

    public UpdateOQConfirmationInteractor(IOnlineRepository onlineRepository)
    {
        _onlineRepository = onlineRepository;
    }

    public UpdateOQConfirmationOutputData Handle(UpdateOQConfirmationInputData inputData)
    {
        try
        {
            if (!_onlineRepository.CheckExistIdList(new List<long>() { inputData.OnlineHistoryId }))
            {
                return new UpdateOQConfirmationOutputData(UpdateOQConfirmationStatus.InvalidId);
            }
            else if (_onlineRepository.UpdateOQConfirmation(inputData.HpId, inputData.UserId, inputData.OnlineHistoryId, inputData.OnlQuaResFileDict, inputData.OnlQuaConfirmationTypeDict))
            {
                return new UpdateOQConfirmationOutputData(UpdateOQConfirmationStatus.Successed);
            }
            return new UpdateOQConfirmationOutputData(UpdateOQConfirmationStatus.Failed);
        }
        finally
        {
            _onlineRepository.ReleaseResource();
        }
    }
}
