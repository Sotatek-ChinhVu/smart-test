using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SearchAdvanced;

public class SearchPatientInfoAdvancedInputData : IInputData<SearchPatientInfoAdvancedOutputData>
{
    public SearchPatientInfoAdvancedInputData(PatientAdvancedSearchInput searchInput)
    {
        SearchInput = searchInput;
    }

    public PatientAdvancedSearchInput SearchInput { get; private set; }
}
