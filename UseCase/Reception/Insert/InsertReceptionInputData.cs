using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Insert;

public class InsertReceptionInputData : IInputData<InsertReceptionOutputData>
{
    public InsertReceptionInputData(ReceptionSaveDto dto)
    {
        Dto = dto;
    }

    public ReceptionSaveDto Dto { get; private set; }
}
