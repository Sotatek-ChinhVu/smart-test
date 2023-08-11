using Domain.Models.ReceSeikyu;

namespace EmrCloudApi.Responses.ReceSeikyu;

public class GetReceSeikyModelByPtNumResponse
{
    public GetReceSeikyModelByPtNumResponse(ReceSeikyuModel receSeikyuModel)
    {
        ReceSeikyuModel = receSeikyuModel;
    }

    public ReceSeikyuModel ReceSeikyuModel { get; private set; }
}
