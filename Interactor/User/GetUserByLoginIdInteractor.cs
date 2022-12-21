using Domain.Models.User;
using UseCase.User.GetByLoginId;

namespace Interactor.User;

public class GetUserByLoginIdInteractor : IGetUserByLoginIdInputPort
{
    private readonly IUserRepository _userRepository;

    public GetUserByLoginIdInteractor(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public GetUserByLoginIdOutputData Handle(GetUserByLoginIdInputData input)
    {
        try
        {
            var user = _userRepository.GetByLoginId(input.LoginId);
            var status = user is null ? GetUserByLoginIdStatus.NotFound : GetUserByLoginIdStatus.Success;
            return new GetUserByLoginIdOutputData(status, user);
        }
        finally
        {
            _userRepository.ReleaseResource();
        }
    }
}
