using Domain.Models.Document;
using EmrCloudApi.Responses.Document.Dto;
using UseCase.Document;

namespace EmrCloudApi.Responses.Document;

public class GetListDocCategoryResponse
{
    public GetListDocCategoryResponse(List<DocCategoryItem> docCategories, List<FileDocumentModel> templateFiles, List<DocInfModel> docInfs)
    {
        DocCategories = docCategories.Select(item => new DocCategoryDto(item)).ToList();
        TemplateFiles = templateFiles.Select(item => new FileDocumentDto(item)).ToList();
        DocInfs = docInfs.Select(item => new DocInfDto(item)).ToList();
    }

    public List<DocCategoryDto> DocCategories { get; private set; }

    public List<FileDocumentDto> TemplateFiles { get; private set; }

    public List<DocInfDto> DocInfs { get; private set; }
}
