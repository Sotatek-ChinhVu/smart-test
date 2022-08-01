using Domain.Models.User;
using UseCase.User.GetList;

namespace Interactor.User;

public class GetUserListInteractor : IGetUserListInputPort
{
    private readonly IUserRepository _userRepository;

    public GetUserListInteractor(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public GetUserListOutputData Handle(GetUserListInputData input)
    {
        if (input.SinDate <= 0)
        {
            return new GetUserListOutputData(GetUserListStatus.InvalidSinDate);
        }

        var users = _userRepository.GetAll(input.SinDate, input.IsDoctorOnly);
        return new GetUserListOutputData(GetUserListStatus.Success, users);
    }
}
