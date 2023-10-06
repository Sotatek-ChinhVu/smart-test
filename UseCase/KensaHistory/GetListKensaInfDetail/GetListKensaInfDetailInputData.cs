using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.GetListKensaInfDetail
{
    public class GetListKensaInfDetailInputData : IInputData<GetListKensaInfDetailOutputData>
    {
        public GetListKensaInfDetailInputData(int hpId, int userId, int ptId, int setId, int pageIndex, int pageSize)
        {
            HpId = hpId;
            UserId = userId;
            PtId = ptId;
            SetId = setId;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
        public int HpId {get; set;}
        public int PtId {get; set;}
        public int UserId { get; set;}
        public int SetId { get; set;}
        public int PageIndex {get; set;}
        public int PageSize {get; set;}
    }
}
