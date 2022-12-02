using Domain.Models.Document;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.Document.MoveTemplateToOtherCategory;

namespace Interactor.Document;

public class MoveTemplateToOtherCategoryInteractor : IMoveTemplateToOtherCategoryInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly AmazonS3Options _options;

    public MoveTemplateToOtherCategoryInteractor(IOptions<AmazonS3Options> optionsAccessor, IDocumentRepository documentRepository, IAmazonS3Service amazonS3Service)
    {
        _options = optionsAccessor.Value;
        _documentRepository = documentRepository;
        _amazonS3Service = amazonS3Service;
    }

    public MoveTemplateToOtherCategoryOutputData Handle(MoveTemplateToOtherCategoryInputData inputData)
    {
        try
        {
            if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.OldCategoryCd))
            {
                return new MoveTemplateToOtherCategoryOutputData(MoveTemplateToOtherCategoryStatus.InvalidOldCategoryCd);
            }
            if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.NewCategoryCd))
            {
                return new MoveTemplateToOtherCategoryOutputData(MoveTemplateToOtherCategoryStatus.InvalidNewCategoryCd);
            }
            var listFolderPath = new List<string>(){
                                                   CommonConstants.Reference,
                                                   CommonConstants.Files,
                                                   inputData.OldCategoryCd.ToString()
                                                };
            string filePath = _amazonS3Service.GetFolderUploadOther(listFolderPath);

            var checkExist = _amazonS3Service.ObjectExistsAsync(filePath + inputData.FileName);
            checkExist.Wait();
            if (checkExist.Result)
            {
                return new MoveTemplateToOtherCategoryOutputData(MoveTemplateToOtherCategoryStatus.FileTemplateNotFould);
            }
            return new MoveTemplateToOtherCategoryOutputData(MoveTemplateToOtherCategoryStatus.Successed);
        }
        catch (Exception)
        {
            return new MoveTemplateToOtherCategoryOutputData(MoveTemplateToOtherCategoryStatus.Failed);
        }
    }
}
