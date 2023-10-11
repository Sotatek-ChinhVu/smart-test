﻿using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Unlock
{
    public class UnlockInputData : IInputData<UnlockOutputData>
    {
        public UnlockInputData(int hpId, int userId, List<LockInfModel> lockModels)
        {
            HpId = hpId;
            UserId = userId;
            LockModels = lockModels;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<LockInfModel> LockModels {  get; private set; }
    }
}
