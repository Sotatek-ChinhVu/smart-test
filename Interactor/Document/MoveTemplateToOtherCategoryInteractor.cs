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
            // old file path
            var listOldFolderPath = new List<string>(){
                                                   CommonConstants.Reference,
                                                   CommonConstants.Files,
                                                   inputData.OldCategoryCd.ToString()
                                                };
            string oldFilePath = _amazonS3Service.GetFolderUploadOther(listOldFolderPath);

            // new file path
            var listNewFolderPath = new List<string>(){
                                                   CommonConstants.Reference,
                                                   CommonConstants.Files,
                                                   inputData.NewCategoryCd.ToString()
                                                };
            string newFilePath = _amazonS3Service.GetFolderUploadOther(listNewFolderPath);

            var validateReuslt = ValidateInput(inputData, oldFilePath, newFilePath);
            if (validateReuslt != MoveTemplateToOtherCategoryStatus.VaidateSuccess)
            {
                return new MoveTemplateToOtherCategoryOutputData(validateReuslt);
            }

            var response = _amazonS3Service.CopyObjectAsync(oldFilePath, inputData.FileName, newFilePath, inputData.FileName);
            response.Wait();
            if (response.Result)
            {
                return new MoveTemplateToOtherCategoryOutputData(MoveTemplateToOtherCategoryStatus.Successed);
            }
            return new MoveTemplateToOtherCategoryOutputData(MoveTemplateToOtherCategoryStatus.Failed);
        }
        catch (Exception)
        {
            return new MoveTemplateToOtherCategoryOutputData(MoveTemplateToOtherCategoryStatus.Failed);
        }
    }

    private MoveTemplateToOtherCategoryStatus ValidateInput(MoveTemplateToOtherCategoryInputData inputData, string oldFilePath, string newFilePath)
    {
        if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.OldCategoryCd))
        {
            return MoveTemplateToOtherCategoryStatus.InvalidOldCategoryCd;
        }
        else if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.NewCategoryCd))
        {
            return MoveTemplateToOtherCategoryStatus.InvalidNewCategoryCd;
        }
        else if (inputData.NewCategoryCd == inputData.OldCategoryCd)
        {
            return MoveTemplateToOtherCategoryStatus.FileTemplateIsExistInNewFolder;
        }

        // check exist file in source folder
        var checkExistOldFile = _amazonS3Service.ObjectExistsAsync(oldFilePath + inputData.FileName);
        checkExistOldFile.Wait();
        if (!checkExistOldFile.Result)
        {
            return MoveTemplateToOtherCategoryStatus.FileTemplateNotFould;
        }

        // check exist file in new folder
        var checkExistNewFile = _amazonS3Service.ObjectExistsAsync(newFilePath + inputData.FileName);
        checkExistNewFile.Wait();
        if (checkExistNewFile.Result)
        {
            return MoveTemplateToOtherCategoryStatus.FileTemplateIsExistInNewFolder;
        }
        return MoveTemplateToOtherCategoryStatus.VaidateSuccess;
    }
}
