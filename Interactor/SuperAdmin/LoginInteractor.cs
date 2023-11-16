﻿using Domain.SuperAdminModels.Admin;
using UseCase.SuperAdmin.Login;

namespace Interactor.SuperAdmin
{
    public class LoginInteractor : ILoginInputPort
    {
        private readonly IAdminRepository _adminRepository;

        public LoginInteractor(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public LoginOutputData Handle(LoginInputData inputData)
        {
            try
            {
                if (inputData.LoginId <= 0)
                {
                    return new LoginOutputData(LoginStatus.InvalidLoginId);
                }
                if (inputData.Password.Length > 100)
                {
                    return new LoginOutputData(LoginStatus.InvalidPassWord);
                }

                var result = _adminRepository.Get(inputData.LoginId, inputData.Password);

                if (result.Id > 0)
                {
                    return new LoginOutputData(LoginStatus.Successed);
                }

                return new LoginOutputData(LoginStatus.Failed);
            }
            finally
            {
                _adminRepository.ReleaseResource();
            }
        }
    }
}