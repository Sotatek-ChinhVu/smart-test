using Domain.Models.User;
using UseCase.User.GetPermissionByScreenCode;

namespace Interactor.User;

public class GetPermissionByScreenInteractor : IGetPermissionByScreenInputPort
{
    private readonly IUserRepository _userRepository;

    public GetPermissionByScreenInteractor(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public GetPermissionByScreenOutputData Handle(GetPermissionByScreenInputData input)
    {
        try
        {
            if (input.HpId <= 0)
            {
                return new GetPermissionByScreenOutputData(GetPermissionByScreenStatus.InvalidHpId);
            }
            if (input.UserId <= 0)
            {
                return new GetPermissionByScreenOutputData(GetPermissionByScreenStatus.InvalidUserId);
            }


            var permisionType = _userRepository.GetPermissionByScreenCode(input.HpId, input.UserId, input.PermissionCode);
            return new GetPermissionByScreenOutputData(GetPermissionByScreenStatus.Successed, permisionType);
        }
        catch
        {
            return new GetPermissionByScreenOutputData(GetPermissionByScreenStatus.Failed);
        }
    }
}
