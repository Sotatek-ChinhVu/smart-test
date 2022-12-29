using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.User.MigrateDatabase;

namespace Interactor.User
{
    public class MigrateDatabaseInterator : IMigrateDatabaseInputPort
    {
        private readonly IUserRepository _userRepository;
        public MigrateDatabaseInterator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public MigrateDatabaseOutputData Handle(MigrateDatabaseInputData inputData)
        {
            try
            {
                string username = inputData.Username;
                string password = inputData.Password;

                if (!_userRepository.CheckLoginInfo(username, password))
                {
                    return new MigrateDatabaseOutputData(MigrateDatabaseStatus.LoginFailed);
                }
                if (!_userRepository.MigrateDatabase())
                {
                    return new MigrateDatabaseOutputData(MigrateDatabaseStatus.MigrateDataFailed);
                }

                return new MigrateDatabaseOutputData(MigrateDatabaseStatus.Successed);
            }
            finally
            {
                _userRepository.ReleaseResource();
            }
        }
    }
}
