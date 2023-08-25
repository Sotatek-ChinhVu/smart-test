using Domain.Models.Online;
using UseCase.Online.UpdateRefNo;

namespace Interactor.Online;

public class UpdateRefNoInteractor : IUpdateRefNoInputPort
{
    private readonly IOnlineRepository _onlineRepository;

    public UpdateRefNoInteractor(IOnlineRepository onlineRepository)
    {
        _onlineRepository = onlineRepository;
    }

    public UpdateRefNoOutputData Handle(UpdateRefNoInputData inputData)
    {
        try
        {
            var nextRefNo = _onlineRepository.UpdateRefNo(inputData.HpId, inputData.PtId);
            if (nextRefNo == 0)
            {
                return new UpdateRefNoOutputData(0, UpdateRefNoStatus.Failed);
            }
            return new UpdateRefNoOutputData(nextRefNo, UpdateRefNoStatus.Successed);
        }
        finally
        {
            _onlineRepository.ReleaseResource();
        }
    }
}
