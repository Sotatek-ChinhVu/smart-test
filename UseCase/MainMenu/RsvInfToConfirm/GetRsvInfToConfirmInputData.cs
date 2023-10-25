using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.RsvInfToConfirm
{
    public class GetRsvInfToConfirmInputData : IInputData<GetRsvInfToConfirmOutputData>
    {
        public GetRsvInfToConfirmInputData(int hpId, int sinDate)
        {
            HpId = hpId;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }
    }
}
