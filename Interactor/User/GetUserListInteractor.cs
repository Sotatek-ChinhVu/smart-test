﻿using Domain.Models.User;
using UseCase.User.GetList;

namespace Interactor.User;

public class GetUserListInteractor : IGetUserListInputPort
{
    private readonly IUserRepository _userRepository;

    public GetUserListInteractor(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public GetUserListOutputData Handle(GetUserListInputData input)
    {
        try
        {
            if (input.SinDate <= 0 && !input.IsAll)
            {
                return new GetUserListOutputData(GetUserListStatus.InvalidSinDate);
            }

            var users = _userRepository.GetAll(input.HpId, input.SinDate, input.IsDoctorOnly, input.IsAll);
            return new GetUserListOutputData(GetUserListStatus.Success, users);
        }
        finally
        {
            _userRepository.ReleaseResource();
        }
    }
}
