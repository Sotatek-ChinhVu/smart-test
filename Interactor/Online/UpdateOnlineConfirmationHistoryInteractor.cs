using Domain.Models.Online;
using UseCase.Online.UpdateOnlineConfirmationHistory;

namespace Interactor.Online;

public class UpdateOnlineConfirmationHistoryInteractor : IUpdateOnlineConfirmationHistoryInputPort
{
    private readonly IOnlineRepository _onlineRepository;

    public UpdateOnlineConfirmationHistoryInteractor(IOnlineRepository onlineRepository)
    {
        _onlineRepository = onlineRepository;
    }

    public UpdateOnlineConfirmationHistoryOutputData Handle(UpdateOnlineConfirmationHistoryInputData inputData)
    {
        int uketukeStatus = inputData.IsDeleted ? 9 : 1;
        var updateSuccessed = _onlineRepository.UpdateOnlineConfirmationHistory(uketukeStatus, inputData.Id, inputData.UserId);
        return new UpdateOnlineConfirmationHistoryOutputData(UpdateOnlineConfirmationHistoryStatus.Successed, updateSuccessed);
    }
}
