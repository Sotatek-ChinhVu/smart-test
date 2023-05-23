using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchAdvanced;

public class SearchPatientInfoAdvancedInputData : IInputData<SearchPatientInfoAdvancedOutputData>
{
    public SearchPatientInfoAdvancedInputData(PatientAdvancedSearchInput searchInput, int hpId, int pageIndex, int pageSize, Dictionary<string, string> sortData)
    {
        SearchInput = searchInput;
        HpId = hpId;
        PageIndex = pageIndex;
        PageSize = pageSize;
        SortData = sortData;
    }

    public PatientAdvancedSearchInput SearchInput { get; private set; }

    public int HpId { get; private set; }

    public int PageIndex { get; private set; }

    public int PageSize { get; private set; }

    public Dictionary<string, string> SortData { get; private set; }
}
