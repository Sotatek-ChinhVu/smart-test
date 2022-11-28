using Domain.Models.Document;
using Domain.Models.HpMst;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text;
using UseCase.Document;
using UseCase.Document.GetListDocCategory;

namespace Interactor.Document;

public class GetListDocCategoryInteractor : IGetListDocCategoryInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly AmazonS3Options _options;

    public GetListDocCategoryInteractor(IOptions<AmazonS3Options> optionsAccessor, IDocumentRepository documentRepository, IAmazonS3Service amazonS3Service, IHpInfRepository hpInfRepository)
    {
        _options = optionsAccessor.Value;
        _documentRepository = documentRepository;
        _amazonS3Service = amazonS3Service;
        _hpInfRepository = hpInfRepository;
    }

    public GetListDocCategoryOutputData Handle(GetListDocCategoryInputData inputData)
    {
        if (!_hpInfRepository.CheckHpId(inputData.HpId))
        {
            return new GetListDocCategoryOutputData(GetListDocCategoryStatus.InvalidHpId);
        }
        try
        {
            var listDocCategory = _documentRepository.GetAllDocCategory(inputData.HpId);
            var listDocumentTemplate = GetListDocumentTemplate(listDocCategory.Select(item => item.CategoryCd).ToList());
            var result = listDocCategory.Select(model => ConvertToDocCategoryMstOutputItem(model)).ToList();
            return new GetListDocCategoryOutputData(result, listDocumentTemplate, GetListDocCategoryStatus.Successed);
        }
        catch
        {
            return new GetListDocCategoryOutputData(GetListDocCategoryStatus.Failed);
        }
    }

    private DocCategoryOutputItem ConvertToDocCategoryMstOutputItem(DocCategoryModel model)
    {
        return new DocCategoryOutputItem(
                                            model.CategoryCd,
                                            model.CategoryName,
                                            model.SortNo
                                        );
    }

    private List<FileDocumentModel> GetListDocumentTemplate(List<int> listCategoryId)
    {
        List<FileDocumentModel> result = new();
        var response = _amazonS3Service.GetListObjectAsync(CommonConstants.FolderDocument);
        response.Wait();
        var listOutputData = response.Result;
        if (listOutputData.Any())
        {
            StringBuilder domainUrl = new StringBuilder();
            domainUrl.Append(_options.BaseAccessUrl + "/");

            foreach (var catId in listCategoryId)
            {
                var listFileItem = listOutputData
                                            .Where(file => file.Contains("/" + catId + "/"))
                                            .Select(file => new FileDocumentModel(
                                                    file.Replace(CommonConstants.FolderDocument + "/", string.Empty).Replace(catId + "/", ""),
                                                    domainUrl + file
                                                )).ToList();
                result.AddRange(listFileItem.Where(item => !string.IsNullOrWhiteSpace(item.FileName)).Distinct().ToList());
            }
        }
        return result;
    }
}
