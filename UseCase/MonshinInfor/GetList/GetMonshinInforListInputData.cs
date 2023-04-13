using UseCase.Core.Sync.Core;

namespace UseCase.MonshinInfor.GetList
{
    public class GetMonshinInforListInputData : IInputData<GetMonshinInforListOutputData>
    {
        public GetMonshinInforListInputData(int hpId, long ptId, long raiinNo, bool isDeleted, bool isGetAll)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            IsDeleted = isDeleted;
            IsGetAll = isGetAll;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public bool IsDeleted { get; private set; }
        public bool IsGetAll { get; private set; }
    }
}
