using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetKohiPriorityList
{
    public class GetKohiPriorityListInputData : IInputData<GetKohiPriorityListOutputData>
    {
        public GetKohiPriorityListInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
