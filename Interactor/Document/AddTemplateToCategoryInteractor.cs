using Domain.Models.Document;
using Domain.Models.HpMst;
using Helper.Constants;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.Document.AddTemplateToCategory;

namespace Interactor.Document;

public class AddTemplateToCategoryInteractor : IAddTemplateToCategoryInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly AmazonS3Options _options;

    public AddTemplateToCategoryInteractor(IOptions<AmazonS3Options> optionsAccessor, IDocumentRepository documentRepository, IAmazonS3Service amazonS3Service, IHpInfRepository hpInfRepository)
    {
        _options = optionsAccessor.Value;
        _documentRepository = documentRepository;
        _amazonS3Service = amazonS3Service;
        _hpInfRepository = hpInfRepository;
    }

    public AddTemplateToCategoryOutputData Handle(AddTemplateToCategoryInputData inputData)
    {
        if (!_hpInfRepository.CheckHpId(inputData.HpId))
        {
            return new AddTemplateToCategoryOutputData(AddTemplateToCategoryStatus.InvalidHpId);
        }
        else if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.CategoryCd))
        {
            return new AddTemplateToCategoryOutputData(AddTemplateToCategoryStatus.InvalidCategoryCd);
        }
        var memoryStream = inputData.StreamImage.ToMemoryStreamAsync().Result;
        if (memoryStream.Length == 0)
        {
            return new AddTemplateToCategoryOutputData(AddTemplateToCategoryStatus.InvalidFileInput);
        }
        try
        {
            var listFolderPath = new List<string>(){
                                                   CommonConstants.Reference,
                                                   CommonConstants.Files,
                                                   inputData.CategoryCd.ToString()
                                                };
            string filePath = _amazonS3Service.GetFolderUploadOther(listFolderPath);

            var checkExist = _amazonS3Service.ObjectExistsAsync(filePath + inputData.FileName);
            checkExist.Wait();
            if (checkExist.Result)
            {
                return new AddTemplateToCategoryOutputData(AddTemplateToCategoryStatus.ExistFileTemplateName);
            }

            var response = _amazonS3Service.UploadObjectAsync(filePath, inputData.FileName, memoryStream);
            response.Wait();
            if (response.Result.Length > 0)
            {
                return new AddTemplateToCategoryOutputData(AddTemplateToCategoryStatus.Successed);
            }
            return new AddTemplateToCategoryOutputData(AddTemplateToCategoryStatus.Failed);
        }
        catch
        {
            return new AddTemplateToCategoryOutputData(AddTemplateToCategoryStatus.Failed);
        }
    }
}
