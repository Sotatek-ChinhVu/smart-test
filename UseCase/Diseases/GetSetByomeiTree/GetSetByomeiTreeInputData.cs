using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.GetSetByomeiTree
{
    public class GetSetByomeiTreeInputData : IInputData<GetSetByomeiTreeOutputData>
    {
        public GetSetByomeiTreeInputData(int hpId, int sinDate)
        {
            HpId = hpId;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

    }
}
