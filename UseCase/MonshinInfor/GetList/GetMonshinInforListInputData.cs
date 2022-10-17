using UseCase.Core.Sync.Core;

namespace UseCase.MonshinInfor.GetList
{
    public class GetMonshinInforListInputData : IInputData<GetMonshinInforListOutputData>
    {
        public GetMonshinInforListInputData(int hpId, long ptId, int sinDate, bool isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public bool IsDeleted { get; private set; }
    }
}
