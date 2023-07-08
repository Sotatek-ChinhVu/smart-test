using Domain.Models.SystemGenerationConf;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemGenerationConf.GetList
{
    public class GetSystemGenerationConfListInputData : IInputData<GetSystemGenerationConfListOutputData>
    {
        public GetSystemGenerationConfListInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
