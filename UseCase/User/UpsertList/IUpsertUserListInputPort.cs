﻿using UseCase.Core.Sync.Core;

namespace UseCase.User.UpsertList
{
    public interface IUpsertUserListInputPort : IInputPort<UpsertUserListInputData, UpsertUserListOutputData>
    {
        UpsertUserListOutputData Handle(UpsertUserListInputData inputData);
    }
}
