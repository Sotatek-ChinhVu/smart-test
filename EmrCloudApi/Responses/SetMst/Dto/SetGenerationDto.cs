using Domain.Models.SetGenerationMst;
using Helper.Common;

namespace EmrCloudApi.Responses.SetMst.Dto;

public class SetGenerationDto
{
    public SetGenerationDto(SetGenerationMstModel model)
    {
        GenerationId = model.GenerationId;
        StartDate = model.StartDate;
        IsDeleted = model.IsDeleted == 1;
    }

    public int GenerationId { get; private set; }

    public int StartDate { get; private set; }

    public bool IsDeleted { get; private set; }

    public string DateView => StartDate > 0 ? CIUtil.Copy(CIUtil.SDateToShowSDate(StartDate), 1, 7) + "～" : "0000/00～";
}
