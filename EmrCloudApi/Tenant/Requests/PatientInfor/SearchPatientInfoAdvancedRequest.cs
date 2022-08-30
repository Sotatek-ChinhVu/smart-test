using Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Requests.PatientInfor;

public class SearchPatientInfoAdvancedRequest
{
    public PatientAdvancedSearchInput SearchInput { get; set; } = null!;
}
