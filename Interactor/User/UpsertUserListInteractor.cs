using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Diseases.Upsert;
using UseCase.User.UpsertList;
using static Helper.Constants.UserConst;

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
                if (inputData.ToList() == null || inputData.ToList().Count == 0)
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.UserListInputNoData);
                }
                var datas = inputData.UpsertUserList.Select(u => new UserMstModel(
                        u.HpId,
                        u.Id,
                        u.UserId,
                        u.JobCd,
                        u.ManagerKbn,
                        u.KaId,
                        u.KanaName,
                        u.Name,
                        u.Sname,
                        u.DrName,
                        u.LoginId,
                        u.LoginPass,
                        u.MayakuLicenseNo,
                        u.StartDate,
                        u.EndDate,
                        u.SortNo,
                        u.RenkeiCd1,
                        u.IsDeleted
                    )).ToList();
                foreach (var data in datas)
                {
                    var status = data.Validation();
                    if (status != ValidationStatus.Valid)
                    {
                        return new UpsertUserListOutputData(ConvertStatusUser(status));
                    }
                }

                _userRepository.Upsert(datas);

                return new UpsertUserListOutputData(UpsertUserListStatus.Success);
            }
            catch
            {
                return new UpsertUserListOutputData(UpsertUserListStatus.Failed);
            }
        }
        private UpsertUserListStatus ConvertStatusUser(ValidationStatus status)
        {
            if (status == ValidationStatus.InvalidHpId)
                return UpsertUserListStatus.UserListInvalidHpId;
            if (status == ValidationStatus.InvalidId)
                return UpsertUserListStatus.UserListInvalidId;
            if (status == ValidationStatus.InvalidUserId)
                return UpsertUserListStatus.UserListInvalidUserId;
            if (status == ValidationStatus.InvalidJobCd)
                return UpsertUserListStatus.UserListInvalidJobCd;
            if (status == ValidationStatus.InvalidManagerKbn)
                return UpsertUserListStatus.UserListInvalidManagerKbn;
            if (status == ValidationStatus.InvalidKanaName)
                return UpsertUserListStatus.UserListInvalidKanaName;
            if (status == ValidationStatus.InvalidKaId)
                return UpsertUserListStatus.UserListInvalidKaId;
            if (status == ValidationStatus.InvalidName)
                return UpsertUserListStatus.UserListInvalidName;
            if (status == ValidationStatus.InvalidSortNo)
                return UpsertUserListStatus.UserListInvalidSortNo;
            if (status == ValidationStatus.InvalidSname)
                return UpsertUserListStatus.UserListInvalidSname;
            if (status == ValidationStatus.InvalidLoginId)
                return UpsertUserListStatus.UserListInvalidLoginId;
            if (status == ValidationStatus.InvalidLoginPass)
                return UpsertUserListStatus.UserListInvalidLoginPass;
            if (status == ValidationStatus.InvalidMayakuLicenseNo)
                return UpsertUserListStatus.UserListInvalidMayakuLicenseNo;
            if (status == ValidationStatus.InvalidStartDate)
                return UpsertUserListStatus.UserListInvalidStartDate;
            if (status == ValidationStatus.InvalidEndDate)
                return UpsertUserListStatus.UserListInvalidEndDate;
            if (status == ValidationStatus.InvalidRenkeiCd1)
                return UpsertUserListStatus.UserListInvalidRenkeiCd1;
            if (status == ValidationStatus.InvalidIsDeleted)
                return UpsertUserListStatus.UserListInvalidIsDeleted;

            return UpsertUserListStatus.Success;
        }
    }
}
