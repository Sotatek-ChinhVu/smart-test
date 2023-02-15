using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.GetTreeSetByomei
{
    public class GetTreeSetByomeiInputData : IInputData<GetTreeSetByomeiOutputData>
    {
        public GetTreeSetByomeiInputData(int hpId, int sinDate)
        {
            HpId = hpId;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

    }
}
