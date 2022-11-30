using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchAdvanced;

public class SearchPatientInfoAdvancedInputData : IInputData<SearchPatientInfoAdvancedOutputData>
{
    public SearchPatientInfoAdvancedInputData(PatientAdvancedSearchInput searchInput, int hpId, int pageIndex, int pageSize)
    {
        SearchInput = searchInput;
        HpId = hpId;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public PatientAdvancedSearchInput SearchInput { get; private set; }

    public int HpId { get; private set; }

    public int PageIndex { get; private set; }

    public int PageSize { get; private set; }
}
