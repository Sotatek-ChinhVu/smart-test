using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Responses.Reception;

public class InitDoctorComboResponse
{
    public InitDoctorComboResponse(long tantoId)
    {
        TantoId = tantoId;
    }

    public long TantoId { get; private set; }
}
