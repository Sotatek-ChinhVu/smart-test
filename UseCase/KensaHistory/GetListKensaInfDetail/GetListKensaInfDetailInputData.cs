﻿using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.GetListKensaInfDetail
{
    public class GetListKensaInfDetailInputData : IInputData<GetListKensaInfDetailOutputData>
    {
        public GetListKensaInfDetailInputData(int hpId, int userId, long ptId, int setId, int iraiCd, int iraiCdStart, bool getGetPrevious, bool showAbnormalKbn, int itemQuantity)
        {
            HpId = hpId;
            UserId = userId;
            PtId = ptId;
            SetId = setId;
            IraiCd = iraiCd;
            IraiCdStart = iraiCdStart;
            GetGetPrevious = getGetPrevious;
            ShowAbnormalKbn = showAbnormalKbn;
            ItemQuantity = itemQuantity;
        }
        public int HpId {get; private set;}

        public long PtId {get; private set;}

        public int UserId { get; private set;}

        public int SetId { get; private set;}

        public int IraiCd { get; private set; }

        public int IraiCdStart { get; set; }
        public bool GetGetPrevious { get; set; }

        public bool ShowAbnormalKbn { get; private set; }

        public int ItemQuantity { get; private set; }
    }
}
