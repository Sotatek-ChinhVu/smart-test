using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Requests.Reception;

public class InsertReceptionRequest
{
    public ReceptionSaveDto Dto { get; set; } = null!;
}
