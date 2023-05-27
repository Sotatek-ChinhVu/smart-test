using UseCase.Core.Sync.Core;

namespace UseCase.RaiinListSetting.GetDocCategory
{
    public class GetDocCategoryRaiinInputData : IInputData<GetDocCategoryRaiinOutputData>
    {
        public GetDocCategoryRaiinInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
