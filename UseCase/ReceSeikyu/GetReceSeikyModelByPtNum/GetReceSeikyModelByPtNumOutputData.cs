using Domain.Models.ReceSeikyu;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.GetReceSeikyModelByPtNum;

public class GetReceSeikyModelByPtNumOutputData : IOutputData
{
    public GetReceSeikyModelByPtNumOutputData(GetReceSeikyModelByPtNumStatus status, ReceSeikyuModel receSeikyuModel)
    {
        Status = status;
        ReceSeikyuModel = receSeikyuModel;
    }

    public GetReceSeikyModelByPtNumStatus Status { get; private set; }

    public ReceSeikyuModel ReceSeikyuModel { get; private set; }
}
