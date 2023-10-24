using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetAllCmtCheckMst
{
    public class GetAllCmtCheckMstInputData : IInputData<GetAllCmtCheckMstOutputData>
    {
        public int HpId { get; private set; }
        public int SinDay { get; private set; }

        public GetAllCmtCheckMstInputData(int hpId, int sinDay)
        {
            SinDay = sinDay;
            HpId = hpId;
        }
    }
}
