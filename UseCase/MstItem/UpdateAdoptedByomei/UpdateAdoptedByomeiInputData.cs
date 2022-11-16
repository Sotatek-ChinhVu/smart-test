using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateAdoptedByomei
{
    public class UpdateAdoptedByomeiInputData : IInputData<UpdateAdoptedByomeiOutputData>
    {
        public UpdateAdoptedByomeiInputData(int hpId, string byomeiCd, int userId)
        {
            HpId = hpId;
            ByomeiCd = byomeiCd;
            UserId = userId;
        }

        public int HpId { get; private set; }
        public string ByomeiCd { get; private set; }
        public int UserId { get; private set; }
    }
}
