﻿using Domain.Models.Ka;
using Domain.Models.User;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.User.UpsertList;
using static Helper.Constants.UserConst;

namespace Interactor.User
{
    public class UpsertUserListInteractor : IUpsertUserListInputPort
    {
        private readonly IUserRepository _userRepository;
        private readonly IKaRepository _kaRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;
        public UpsertUserListInteractor(ITenantProvider tenantProvider, IUserRepository userRepository, IKaRepository kaRepository)
        {
            _userRepository = userRepository;
            _kaRepository = kaRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
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
                        1,
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

                var checkInputId = datas.Where(u => u.Id > 0).Select(u => u.Id);
                if (checkInputId.Count() != checkInputId.Distinct().Count())
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.UserListExistedInputData);
                }

                var checkInputUserId = datas.Select(u => u.UserId);
                if (checkInputUserId.Count() != checkInputUserId.Distinct().Count())
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.UserListExistedInputData);
                }

                var checkInputLoginId = datas.Select(u => u.LoginId);
                if (checkInputLoginId.Count() != checkInputLoginId.Distinct().Count())
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.UserListExistedInputData);
                }

                foreach (var data in datas)
                {
                    var status = data.Validation();
                    if (status != ValidationStatus.Valid)
                    {
                        return new UpsertUserListOutputData(ConvertStatusUser(status));
                    }
                }

                if (!_userRepository.CheckExistedId(datas.Where(u => u.Id > 0).Select(u => u.Id).ToList()))
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.UserListInvalidNoExistedId);
                }

                if (_userRepository.CheckExistedUserIdCreate(datas.Where(u => u.Id == 0).Select(u => u.UserId).ToList()))
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.UserListInvalidExistedUserId);
                }

                if (_userRepository.CheckExistedUserIdUpdate(datas.Select(u => u.Id).ToList(), datas.Select(u => u.UserId).ToList()))
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.UserListInvalidExistedUserId);
                }

                if (_userRepository.CheckExistedLoginIdCreate(datas.Where(u => u.Id == 0).Select(u => u.LoginId).ToList()))
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.UserListInvalidExistedLoginId);
                }

                if (_userRepository.CheckExistedLoginIdUpdate(datas.Select(u => u.Id).ToList(), datas.Select(u => u.LoginId).ToList()))
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.UserListInvalidExistedLoginId);
                }

                if (!_kaRepository.CheckKaId(datas.Select(u => u.KaId).Distinct().ToList()))
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.UserListKaIdNoExist);
                }

                if (!_userRepository.CheckExistedJobCd(datas.Select(u => u.JobCd).Distinct().ToList()))
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.UserListJobCdNoExist);
                }

                var upsert = _userRepository.Upsert(datas, inputData.UserId);
                if (!upsert)
                {
                    return new UpsertUserListOutputData(UpsertUserListStatus.UserListInvalidExistedUserId);
                }

                return new UpsertUserListOutputData(UpsertUserListStatus.Success);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _kaRepository.ReleaseResource();
                _userRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
        private static UpsertUserListStatus ConvertStatusUser(ValidationStatus status)
        {
            if (status == ValidationStatus.InvalidHpId)
                return UpsertUserListStatus.InvalidHpId;
            if (status == ValidationStatus.InvalidId)
                return UpsertUserListStatus.InvalidId;
            if (status == ValidationStatus.InvalidUserId)
                return UpsertUserListStatus.InvalidUserId;
            if (status == ValidationStatus.InvalidJobCd)
                return UpsertUserListStatus.InvalidJobCd;
            if (status == ValidationStatus.InvalidManagerKbn)
                return UpsertUserListStatus.InvalidManagerKbn;
            if (status == ValidationStatus.InvalidKanaName)
                return UpsertUserListStatus.InvalidKanaName;
            if (status == ValidationStatus.InvalidKaId)
                return UpsertUserListStatus.InvalidKaId;
            if (status == ValidationStatus.InvalidName)
                return UpsertUserListStatus.InvalidName;
            if (status == ValidationStatus.InvalidSortNo)
                return UpsertUserListStatus.InvalidSortNo;
            if (status == ValidationStatus.InvalidSname)
                return UpsertUserListStatus.InvalidSname;
            if (status == ValidationStatus.InvalidLoginId)
                return UpsertUserListStatus.InvalidLoginId;
            if (status == ValidationStatus.InvalidExistedLoginId)
                return UpsertUserListStatus.UserListInvalidExistedLoginId;
            if (status == ValidationStatus.InvalidLoginPass)
                return UpsertUserListStatus.InvalidLoginPass;
            if (status == ValidationStatus.InvalidMayakuLicenseNo)
                return UpsertUserListStatus.InvalidMayakuLicenseNo;
            if (status == ValidationStatus.InvalidStartDate)
                return UpsertUserListStatus.InvalidStartDate;
            if (status == ValidationStatus.InvalidEndDate)
                return UpsertUserListStatus.InvalidEndDate;
            if (status == ValidationStatus.InvalidRenkeiCd1)
                return UpsertUserListStatus.InvalidRenkeiCd1;
            if (status == ValidationStatus.InvalidIsDeleted)
                return UpsertUserListStatus.InvalidIsDeleted;
            if (status == ValidationStatus.UserListIdNoExist)
                return UpsertUserListStatus.UserListIdNoExist;
            if (status == ValidationStatus.UserListKaIdNoExist)
                return UpsertUserListStatus.UserListKaIdNoExist;
            if (status == ValidationStatus.UserListJobCdNoExist)
                return UpsertUserListStatus.UserListJobCdNoExist;
            if (status == ValidationStatus.InvalidExistedUserId)
                return UpsertUserListStatus.UserListInvalidExistedUserId;
            if (status == ValidationStatus.InvalidNoExistedId)
                return UpsertUserListStatus.UserListInvalidNoExistedId;
            if (status == ValidationStatus.UserListInputData)
                return UpsertUserListStatus.UserListExistedInputData;

            return UpsertUserListStatus.Success;
        }
    }
}
