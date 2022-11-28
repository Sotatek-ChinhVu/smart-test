using Domain.Models.Reception;

namespace EmrCloudApi.Requests.Reception;

public class InsertReceptionRequest
{
    public ReceptionSaveDto Dto { get; set; } = null!;
}
