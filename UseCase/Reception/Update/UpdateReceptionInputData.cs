using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Update;

public class UpdateReceptionInputData : IInputData<UpdateReceptionOutputData>
{
    public UpdateReceptionInputData(ReceptionSaveDto dto)
    {
        Dto = dto;
    }

    public ReceptionSaveDto Dto { get; private set; }
}
