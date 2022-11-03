using Domain.Models.UserConf;
using UseCase.User.GetUserConfList;

namespace Interactor.UserConf;

public class GetUserConfListInteractor : IGetUserConfListInputPort
{
    private readonly IUserConfRepository _userConfRepository;

    public GetUserConfListInteractor(IUserConfRepository userConfRepository)
    {
        _userConfRepository = userConfRepository;
    }

    public GetUserConfListOutputData Handle(GetUserConfListInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetUserConfListOutputData(GetUserConfListStatus.InvalidHpId, new());
            }
            if (inputData.UserId <= 0)
            {
                return new GetUserConfListOutputData(GetUserConfListStatus.InvalidUserId, new());
            }

            var result = _userConfRepository.GetList(inputData.UserId);
            return new GetUserConfListOutputData(GetUserConfListStatus.Successed, result);
        }
        catch
        {
            return new GetUserConfListOutputData(GetUserConfListStatus.Failed, new());
        }
    }
}
