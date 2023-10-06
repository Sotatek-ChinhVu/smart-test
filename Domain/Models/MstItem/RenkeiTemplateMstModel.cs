namespace Domain.Models.MstItem;

public class RenkeiTemplateMstModel
{
    public RenkeiTemplateMstModel(int templateId, string templateName, string param, string file, int sortNo)
    {
        TemplateId = templateId;
        TemplateName = templateName;
        Param = param;
        File = file;
        SortNo = sortNo;
    }

    public int TemplateId { get; private set; }

    public string TemplateName { get; private set; }

    public string Param { get; private set; }

    public string File { get; private set; }

    public int SortNo { get; private set; }
}
