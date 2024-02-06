using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetDefaultPrecautions
{
    public class GetDefaultPrecautionsInputData : IInputData<GetDefaultPrecautionsOutputData>
    {
        public GetDefaultPrecautionsInputData(int hpId, string yjCd)
        {
            HpId = hpId;
            YjCd = yjCd;
        }

        public string YjCd { get; private set; }

        public int HpId { get; private set; }
    }
}
