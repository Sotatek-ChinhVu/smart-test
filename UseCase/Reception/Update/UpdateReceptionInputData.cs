﻿using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Update;

public class UpdateReceptionInputData : IInputData<UpdateReceptionOutputData>
{
    public UpdateReceptionInputData(ReceptionSaveDto dto, int hpId, int userId)
    {
        Dto = dto;
        HpId = hpId;
        UserId = userId;
    }

    public ReceptionSaveDto Dto { get; private set; }

    public int HpId { get; private set; }

    public int UserId { get; private set; }
}
