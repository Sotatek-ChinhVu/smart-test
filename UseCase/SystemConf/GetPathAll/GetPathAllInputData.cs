using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.GetPathAll
{
    public class GetPathAllInputData : IInputData<GetPathAllOutputData>
    {
        public GetPathAllInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
