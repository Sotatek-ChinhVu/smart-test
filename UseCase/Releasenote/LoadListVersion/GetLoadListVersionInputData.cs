using UseCase.Core.Sync.Core;

namespace UseCase.Releasenote.LoadListVersion
{
    public class GetLoadListVersionInputData : IInputData<GetLoadListVersionOutputData>
    {
        public GetLoadListVersionInputData(int hpId, int userId) 
        {
            HpId = hpId;
            UserId = userId;
        }

        public int HpId {  get; private set; }

        public int UserId {  get; private set; }
    }
}
