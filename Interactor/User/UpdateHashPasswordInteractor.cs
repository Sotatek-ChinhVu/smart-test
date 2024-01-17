using Domain.Models.User;
using UseCase.User.UpdateHashPassword;

namespace Interactor.User
{
    public class UpdateHashPasswordInteractor : IUpdateHashPasswordInputPort
    {
        private readonly IUserRepository _userRepository;

        public UpdateHashPasswordInteractor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UpdateHashPasswordOutputData Handle(UpdateHashPasswordInputData inputData)
        {
            try
            {
                _userRepository.UpdateHashPassword();

                return new UpdateHashPasswordOutputData(UpdateHashPasswordStatus.Success);
            }
            finally
            {
                _userRepository.ReleaseResource();
            }
        }
    }
}
