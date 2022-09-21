using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Requests.Reception;

public class UpdateReceptionRequest
{
    public ReceptionSaveDto Dto { get; set; } = null!;
}
