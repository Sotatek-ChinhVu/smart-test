using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTreeListSet
{
    public sealed class GetTreeListSetInputData : IInputData<GetTreeListSetOutputData>
    {
        public int HpId { get; private set; }
        public int SinDate { get; private set; }
        public int SetKbn { get; private set; }

        public GetTreeListSetInputData(int hpId, int sinDate, int setKbn)
        {
            HpId = hpId;
            SinDate = sinDate;
            SetKbn = setKbn;
        }
    }
}
