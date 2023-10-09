using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.GetListKensaInfDetail
{
    public class GetListKensaInfDetailInputData : IInputData<GetListKensaInfDetailOutputData>
    {
        public GetListKensaInfDetailInputData(int hpId, int userId, int ptId, int setId, int iraiCd, int startDate, bool showAbnormalKbn, int itemQuantity)
        {
            HpId = hpId;
            UserId = userId;
            PtId = ptId;
            SetId = setId;
            IraiCd = iraiCd;
            StartDate = startDate;
            ShowAbnormalKbn = showAbnormalKbn;
            ItemQuantity = itemQuantity;
        }
        public int HpId {get; set;}
        public int PtId {get; set;}
        public int UserId { get; set;}
        public int SetId { get; set;}
        public int IraiCd { get; set; }
        public int StartDate { get; set; }
        public bool ShowAbnormalKbn { get; set; }
        public int ItemQuantity { get; set; }
    }
}
