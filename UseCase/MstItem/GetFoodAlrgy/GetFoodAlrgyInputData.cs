﻿using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetFoodAlrgy
{
    public class GetFoodAlrgyInputData : IInputData<GetFoodAlrgyOutputData>
    {
        public GetFoodAlrgyInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
