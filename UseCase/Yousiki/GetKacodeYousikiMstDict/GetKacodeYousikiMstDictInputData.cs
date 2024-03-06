using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetKacodeYousikiMstDict
{
    public class GetKacodeYousikiMstDictInputData : IInputData<GetKacodeYousikiMstDictOutputData>
    {
        public GetKacodeYousikiMstDictInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId {  get; private set; }
    }
}
