using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.GetListKensaInfDetail
{
    public class GetListKensaInfDetailInputData : IInputData<GetListKensaInfDetailOutputData>
    {
        public GetListKensaInfDetailInputData(int hpId, int userId, long ptId, int setId, int iraiCd, int iraiCdStart, bool getGetPrevious, bool showAbnormalKbn, int itemQuantity, List<long> listSeqNoItems, int startDate, int endDate)
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
            ListSeqNoItems = listSeqNoItems;
            StartDate = startDate;
            EndDate = endDate;
        }
        public int HpId {get; private set;}

        public long PtId {get; private set;}

        public int UserId { get; private set;}

        public int SetId { get; private set;}

        public int IraiCd { get; private set; }

        public int IraiCdStart { get; private set; }

        public bool GetGetPrevious { get; private set; }

        public bool ShowAbnormalKbn { get; private set; }

        public int ItemQuantity { get; private set; }

        public List<long> ListSeqNoItems { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }
    }
}
