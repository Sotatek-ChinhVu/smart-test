namespace EmrCloudApi.Tenant.Responses.Document;

public class GetDocCategoryDetailResponse
{
    public GetDocCategoryDetailResponse(DocCategoryDto docCategory, List<FileDocumentDto> templateFile)
    {
        DocCategory = docCategory;
        TemplateFile = templateFile;
    }

    public DocCategoryDto DocCategory { get; private set; }
    public List<FileDocumentDto> TemplateFile { get; private set; }
}
