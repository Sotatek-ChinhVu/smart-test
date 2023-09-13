﻿using Domain.Models.KensaIrai;
using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.UpdateKensaMst
{
    public class UpdateKensaMstInputData : IInputData<UpdateKensaMstOutputData>
    {
        public UpdateKensaMstInputData(int hpId, int userId, List<KensaMstModel> kensaMsts, List<TenItemModel> tenMsts)
        {
            HpId = hpId;
            UserId = userId;
            KensaMsts = kensaMsts;
            TenMsts = tenMsts;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<KensaMstModel> KensaMsts { get; private set; }

        public List<TenItemModel> TenMsts { get; private set; }
    }
}
