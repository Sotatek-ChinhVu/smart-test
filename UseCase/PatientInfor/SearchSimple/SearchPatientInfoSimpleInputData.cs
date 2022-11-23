using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchSimple
{
    public class SearchPatientInfoSimpleInputData : IInputData<SearchPatientInfoSimpleOutputData>
    {
        public SearchPatientInfoSimpleInputData(string keyword, bool containMode, int hpId, int pageIndex, int pageSize)
        {
            Keyword = keyword;
            ContainMode = containMode;
            HpId = hpId;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string Keyword { get; private set; }

        public bool ContainMode { get; private set; }

        public int HpId { get; private set; }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }
    }
}
