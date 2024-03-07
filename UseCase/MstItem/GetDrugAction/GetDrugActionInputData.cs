using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetDrugAction
{
    public class GetDrugActionInputData : IInputData<GetDrugActionOutputData>
    {
        public GetDrugActionInputData(int hpId, string yjCd)
        {
            YjCd = yjCd;
            HpId = hpId;
        }

        public string YjCd { get; private set; }

        public int HpId { get; private set; }
    }
}
