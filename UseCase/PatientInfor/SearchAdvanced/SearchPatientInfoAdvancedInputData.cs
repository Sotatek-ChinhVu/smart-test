using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchAdvanced;

public class SearchPatientInfoAdvancedInputData : IInputData<SearchPatientInfoAdvancedOutputData>
{
    public SearchPatientInfoAdvancedInputData(PatientAdvancedSearchInput searchInput, int hpId)
    {
        SearchInput = searchInput;
        HpId = hpId;
    }

    public PatientAdvancedSearchInput SearchInput { get; private set; }

    public int HpId { get; private set; }
}
