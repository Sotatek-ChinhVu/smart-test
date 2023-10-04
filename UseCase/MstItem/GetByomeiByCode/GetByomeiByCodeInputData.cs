using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetByomeiByCode
{
    public sealed class GetByomeiByCodeInputData : IInputData<GetByomeiByCodeOutputData>
    {
        public GetByomeiByCodeInputData(string byomeiCd)
        {
            ByomeiCd = byomeiCd;
        }
        public string ByomeiCd {  get; private set; }
    }
}
