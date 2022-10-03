﻿using Domain.Models.User;
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

                //Check existedId in the upsertList
                var idList = inputData.UpsertUserList.Select(u => u.Id).ToList();
                if (_userRepository.CheckExistedId(idList))
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.ExistedId);
                }
                return new UpsertUserListOutputData(UpsertUserListStatus.Success);
            }
            catch
            {
                return new UpsertUserListOutputData(UpsertUserListStatus.Fail);
            }
            _userRepository.Upsert(inputData.UpsertUserList);
        }
    }
}
