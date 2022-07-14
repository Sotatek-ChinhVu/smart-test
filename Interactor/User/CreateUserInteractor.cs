using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.User.Create;

namespace Interactor.User
{
    public class CreateUserInteractor : ICreateUserInputPort
    {
        private readonly IUserRepository _userRepository;
        
        public CreateUserInteractor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public CreateUserOutputData Handle(CreateUserInputData inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Name.Value))
            {
                return new CreateUserOutputData(0, CreateUserStatus.InvalidName);
            }

            int userId = _userRepository.MaxUserId();
            var user = inputData.GenerateUserModel(userId);

            _userRepository.Create(user);

            return new CreateUserOutputData(userId, CreateUserStatus.Success);
        }
    }
}
