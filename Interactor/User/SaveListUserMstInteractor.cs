﻿using Domain.Constant;
using Domain.Models.User;
using Helper.Constants;
using UseCase.User.SaveListUserMst;

namespace Interactor.User
{
    public class SaveListUserMstInteractor : ISaveListUserMstInputPort
    {
        private readonly IUserRepository _userRepository;
        public SaveListUserMstInteractor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public SaveListUserMstOutputData Handle(SaveListUserMstInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new SaveListUserMstOutputData(SaveListUserMstStatus.InvalidHpId, string.Empty);

                if (inputData.UserId <= 0)
                    return new SaveListUserMstOutputData(SaveListUserMstStatus.InvalidUserId, string.Empty);

                if (inputData.Users.Count == 0)
                    return new SaveListUserMstOutputData(SaveListUserMstStatus.NoData, string.Empty);

                string msgValidate = this.ValidateMsg(inputData.HpId, inputData.Users, inputData.UserId);
                if(!string.IsNullOrEmpty(msgValidate))
                {
                    return new SaveListUserMstOutputData(SaveListUserMstStatus.InvalidValiDate, msgValidate);
                }

                bool result = _userRepository.SaveListUserMst(inputData.HpId, inputData.Users, inputData.UserId);
                if (result)
                    return new SaveListUserMstOutputData(SaveListUserMstStatus.Successful, string.Empty);
                else
                {
                    string message = string.Format(ErrorMessage.MessageType_mUpd01030, "患者番号");
                    return new SaveListUserMstOutputData(SaveListUserMstStatus.Failed, message);
                }
            }
            finally
            {
                _userRepository.ReleaseResource();
            }
        }

        private string ValidateMsg(int hpId, List<UserMstModel> users, int currentUser)
        {
            var listKaId = _userRepository.ListDepartmentValid(hpId);
            var listJobValid = _userRepository.ListJobCdValid(hpId);
            var currentInfo = _userRepository.GetByUserId(currentUser);
            string msg = string.Empty;
            string msgResult = string.Empty;
            foreach(var user in users)
            {
                if (user.UserId < 1)
                {
                    msg = "ユーザーID ";
                    break;
                }

                if ((!string.IsNullOrEmpty(user.Name) ||
                    !string.IsNullOrEmpty(user.Sname) ||
                    !string.IsNullOrEmpty(user.LoginId) ||
                    !string.IsNullOrEmpty(user.KanaName))
                    && user.UserId < 0)
                {
                    msg = "ユーザーID,'1' ";
                    break;
                }
                else if ((user.Id == 0 && _userRepository.UserIdIsExistInDb(user.UserId)) 
                            || users.Count(x=>x.UserId == user.UserId) > 1)
                {
                    msg = "ログインID'" + user.LoginId + "' +" + "・ ログインIDを変更してください。";
                    break;
                }
                else if (string.IsNullOrEmpty(user.LoginPass))
                {
                    msg = "パスワード";
                    break;
                }
                else if (listJobValid.Count(job => job == user.JobCd) == 0)
                {
                    msg = "職種区分";
                    break;
                }
                else if (listKaId.Count(per => per == user.KaId) == 0)
                {
                    msg = "診療科コード";
                    break;
                }
                else if (user.StartDate > user.EndDate)
                {
                    msg = "DateTime";
                    break;
                }
                else if (currentInfo.ManagerKbn != UserPermissionConst.AdminSystem)
                {
                    if (user.ManagerKbn == UserPermissionConst.AdminSystem)
                    {
                        msg = "権限がないのため、権限を'システム管理者'に更新できません。";
                        break;
                    }
                }
            }

            if (msg.Contains(","))
            {
                msgResult = string.Format(ErrorMessage.MessageType_mInp00060, msg.Substring(0, msg.IndexOf(",")));
            }
            else if (msg.Contains("DateTime"))
            {
                msgResult = string.Format(ErrorMessage.MessageType_mInp00041, "患者番号", new string[] { "終了日", "開始日以降" });
            }
            else if (msg.Contains("を変更してください。"))
            {
                msgResult = string.Format(ErrorMessage.MessageType_mEnt01020, msg.Substring(0, msg.IndexOf("+") - 1));
            }
            else if (msg.Contains("権限がないのため、"))
            {
                msgResult = string.Format(ErrorMessage.MessageType_mFree00030, msg);
            }
            else
            {
                msgResult = string.Format(ErrorMessage.MessageType_mInp00010, msg);
            }

            return msgResult;
        }
    }
}
