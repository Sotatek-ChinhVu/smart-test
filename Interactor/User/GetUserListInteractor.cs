using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.User.GetList;

namespace Interactor.User
{
    public class GetUserListInteractor : IGetUserListInputPort
    {
        private readonly IUserRepository _userRepository;
        public GetUserListInteractor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public GetUserListOutputData Handle(GetUserListInputData inputData)
        {
            return new GetUserListOutputData(_userRepository.GetAll().ToList());
        }
    }
}
