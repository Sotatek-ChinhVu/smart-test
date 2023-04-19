using Domain.Models.UserConf;
using UseCase.User.GetUserConfModelList;

namespace Interactor.UserConf;

public class GetUserConfModelListInteractor : IGetUserConfModelListInputPort
{
    private readonly IUserConfRepository _userConfRepository;

    public GetUserConfModelListInteractor(IUserConfRepository userConfRepository)
    {
        _userConfRepository = userConfRepository;
    }

    public GetUserConfModelListOutputData Handle(GetUserConfModelListInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetUserConfModelListOutputData(GetUserConfModelListStatus.InvalidHpId, new());
            }
            if (inputData.UserId <= 0)
            {
                return new GetUserConfModelListOutputData(GetUserConfModelListStatus.InvalidUserId, new());
            }

            var result = _userConfRepository.GetList(inputData.HpId, inputData.UserId);
            return new GetUserConfModelListOutputData(GetUserConfModelListStatus.Successed, result);
        }
        finally
        {
            _userConfRepository.ReleaseResource();
        }
    }
}
