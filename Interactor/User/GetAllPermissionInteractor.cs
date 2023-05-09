using Domain.Models.User;
using UseCase.User.GetAllPermission;

namespace Interactor.User;

public class GetAllPermissionInteractor : IGetAllPermissionInputPort
{
    private readonly IUserRepository _userRepository;

    public GetAllPermissionInteractor(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public GetAllPermissionOutputData Handle(GetAllPermissionInputData input)
    {
        try
        {
            if (input.HpId <= 0)
            {
                return new GetAllPermissionOutputData(GetAllPermissionStatus.InvalidHpId, new());
            }
            if (input.UserId <= 0)
            {
                return new GetAllPermissionOutputData(GetAllPermissionStatus.InvalidUserId, new());
            }

            var userPermission = _userRepository.GetAllPermission(input.HpId, input.UserId);
            return new GetAllPermissionOutputData(GetAllPermissionStatus.Success, userPermission);
        }
        finally
        {
            _userRepository.ReleaseResource();
        }
    }
}
