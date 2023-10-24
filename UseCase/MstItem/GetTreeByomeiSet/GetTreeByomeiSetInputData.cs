using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTreeByomeiSet
{
    public sealed class GetTreeByomeiSetInputData : IInputData<GetTreeByomeiSetOutputData>
    {
        public GetTreeByomeiSetInputData(int hpId, int sinDate)
        {
            HpId = hpId;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }
    }
}
