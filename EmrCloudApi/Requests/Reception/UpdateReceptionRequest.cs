using Domain.Models.Reception;

namespace EmrCloudApi.Requests.Reception;

public class UpdateReceptionRequest
{
    public ReceptionSaveDto Dto { get; set; } = null!;
}
