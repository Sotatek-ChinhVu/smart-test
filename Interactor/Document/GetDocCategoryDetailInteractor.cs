using Domain.Models.Document;
using Domain.Models.HpMst;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text;
using UseCase.Document;
using UseCase.Document.GetDocCategoryDetail;

namespace Interactor.Document;

public class GetDocCategoryDetailInteractor : IGetDocCategoryDetailInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly AmazonS3Options _options;

    public GetDocCategoryDetailInteractor(IOptions<AmazonS3Options> optionsAccessor, IDocumentRepository documentRepository, IAmazonS3Service amazonS3Service, IHpInfRepository hpInfRepository)
    {
        _options = optionsAccessor.Value;
        _documentRepository = documentRepository;
        _amazonS3Service = amazonS3Service;
        _hpInfRepository = hpInfRepository;
    }

    public GetDocCategoryDetailOutputData Handle(GetDocCategoryDetailInputData inputData)
    {
        if (!_hpInfRepository.CheckHpId(inputData.HpId))
        {
            return new GetDocCategoryDetailOutputData(GetDocCategoryDetailStatus.InvalidHpId);
        }
        else if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.CategoryCd))
        {
            return new GetDocCategoryDetailOutputData(GetDocCategoryDetailStatus.InvalidCategoryCd);
        }
        try
        {
            var docCategory = _documentRepository.GetDocCategoryDetail(inputData.HpId, inputData.CategoryCd);
            var listDocumentTemplate = GetListDocumentTemplate(inputData.CategoryCd);
            var result = new DocCategoryOutputItem(
                                                inputData.HpId,
                                                docCategory.CategoryCd,
                                                docCategory.CategoryName,
                                                docCategory.SortNo
                                            );
            return new GetDocCategoryDetailOutputData(result, listDocumentTemplate, GetDocCategoryDetailStatus.Successed);
        }
        catch
        {
            return new GetDocCategoryDetailOutputData(GetDocCategoryDetailStatus.Failed);
        }
    }

    private List<FileDocumentModel> GetListDocumentTemplate(int categoryId)
    {
        List<FileDocumentModel> result = new();
        var response = _amazonS3Service.GetListObjectAsync(CommonConstants.FolderDocument);
        response.Wait();
        var listOutputData = response.Result;
        if (listOutputData.Any())
        {
            StringBuilder domainUrl = new StringBuilder();
            domainUrl.Append(_options.BaseAccessUrl + "/");
            var listFileItem = listOutputData
                                        .Where(file => file.Contains("/" + categoryId + "/"))
                                        .Select(file => new FileDocumentModel(
                                                file.Replace(CommonConstants.FolderDocument + "/", string.Empty).Replace(categoryId + "/", ""),
                                                domainUrl + file
                                            )).ToList();
            result.AddRange(listFileItem.Where(item => !string.IsNullOrWhiteSpace(item.FileName)).Distinct().ToList());
        }
        return result;
    }
}
