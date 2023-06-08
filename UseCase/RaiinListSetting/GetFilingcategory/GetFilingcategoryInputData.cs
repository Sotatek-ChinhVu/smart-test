using UseCase.Core.Sync.Core;

namespace UseCase.RaiinListSetting.GetFilingcategory
{
    public class GetFilingcategoryInputData : IInputData<GetFilingcategoryOutputData>
    {
        public GetFilingcategoryInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
