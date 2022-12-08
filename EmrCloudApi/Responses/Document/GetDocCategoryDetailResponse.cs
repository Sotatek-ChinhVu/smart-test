using Domain.Models.Document;
using UseCase.Document;

namespace EmrCloudApi.Responses.Document;

public class GetDocCategoryDetailResponse
{
    public GetDocCategoryDetailResponse(DocCategoryItem docCategory, List<FileDocumentModel> templateFiles, List<DocInfModel> docInfs)
    {
        DocCategory = new DocCategoryDto(docCategory);
        TemplateFile = templateFiles.Select(item => new FileDocumentDto(item)).ToList();
        DocInfs = docInfs.Select(item => new DocInfDto(item)).ToList();
    }

    public DocCategoryDto DocCategory { get; private set; }

    public List<FileDocumentDto> TemplateFile { get; private set; }

    public List<DocInfDto> DocInfs { get; private set; }
}
