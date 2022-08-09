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
            //Check duplicatedId in the insertList
            var duplicatedIdDic = inputData.InserteddUserList.GroupBy(u => u.Id).Where(g => g.Count() > 1).ToDictionary(u => u.Key, y => y.Count());
            if (duplicatedIdDic.Any())
            {
                return new UpsertUserListOutputData(UpsertUserListStatus.DuplicateId);
            }

            //Check existedId in the insertList
            var idList = inputData.InserteddUserList.Select(u => u.Id).ToList();
            if (_userRepository.CheckExistedId(idList))
            {
                return new UpsertUserListOutputData(UpsertUserListStatus.ExistedId);
            }

            _userRepository.Upsert(inputData.UpdatedUserList, inputData.InserteddUserList);

            throw new NotImplementedException();
        }
    }
}
