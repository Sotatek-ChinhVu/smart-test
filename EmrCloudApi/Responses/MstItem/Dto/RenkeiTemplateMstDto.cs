using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem.Dto;

public class RenkeiTemplateMstDto
{
    public RenkeiTemplateMstDto(RenkeiTemplateMstModel model)
    {
        TemplateId = model.TemplateId;
        TemplateName = model.TemplateName;
        Param = model.Param;
        File = model.File;
        SortNo = model.SortNo;
    }

    public int TemplateId { get; private set; }

    public string TemplateName { get; private set; }

    public string Param { get; private set; }

    public string File { get; private set; }

    public int SortNo { get; private set; }
}
