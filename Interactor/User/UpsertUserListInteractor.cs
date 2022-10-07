using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.User.UpsertList;

namespace Interactor.User
{
    public class UpsertUserListInteractor : IUpsertUserListInputPort
    {
        private readonly IUserRepository _userRepository;

        public UpsertUserListInteractor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UpsertUserListOutputData Handle(UpsertUserListInputData inputData)
        {
            try
            {
                //Check duplicatedId in the upsertList
                var duplicatedIdDic = inputData.UpsertUserList.GroupBy(u => u.Id).Where(g => g.Count() > 1).ToDictionary(u => u.Key, y => y.Count());
                if (duplicatedIdDic.Any())
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.DuplicateId);
                }

                _userRepository.Upsert(inputData.UpsertUserList);

                return new UpsertUserListOutputData(UpsertUserListStatus.Success);
            }
            catch
            {
                return new UpsertUserListOutputData(UpsertUserListStatus.Failed);
            }
        }
    }
}
