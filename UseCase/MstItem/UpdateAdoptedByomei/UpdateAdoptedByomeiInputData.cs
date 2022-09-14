using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateAdoptedByomei
{
    public class UpdateAdoptedByomeiInputData : IInputData<UpdateAdoptedByomeiOutputData>
    {
        public UpdateAdoptedByomeiInputData(int hpId, string byomeiCd)
        {
            HpId = hpId;
            ByomeiCd = byomeiCd;
        }

        public int HpId { get; private set; }
        public string ByomeiCd { get; private set; }
    }
}
