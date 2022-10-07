using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchEmptyId
{
    public class SearchEmptyIdInputData : IInputData<SearchEmptyIdOutputData>
    {
        public SearchEmptyIdInputData(int hpId, long ptNum, int pageIndex, int pageSize)
        {
            HpId = hpId;
            PtNum = ptNum;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int HpId { get; private set; }
        public long PtNum { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
    }
}
