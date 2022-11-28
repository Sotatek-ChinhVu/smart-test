using Domain.Models.PatientInfor;

namespace EmrCloudApi.Requests.PatientInfor;

public class SearchPatientInfoAdvancedRequest
{
    public PatientAdvancedSearchInput SearchInput { get; set; } = null!;
}
