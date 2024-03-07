using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetByomeiByCode
{
    public sealed class GetByomeiByCodeInputData : IInputData<GetByomeiByCodeOutputData>
    {
        public GetByomeiByCodeInputData(int hpId, string byomeiCd)
        {
            HpId = hpId;
            ByomeiCd = byomeiCd;
        }
        public int HpId { get; private set; }
        public string ByomeiCd { get; private set; }
    }
}
