namespace EmrCloudApi.Tenant.Responses.Document;

public class GetListDocCategoryResponse
{
    public GetListDocCategoryResponse(List<DocCategoryDto> docCategories, List<FileDocumentDto> templateFile)
    {
        DocCategories = docCategories;
        TemplateFile = templateFile;
    }

    public List<DocCategoryDto> DocCategories { get; private set; }
    public List<FileDocumentDto> TemplateFile { get; private set; }

}
