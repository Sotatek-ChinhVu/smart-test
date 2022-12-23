using Domain.Models.Document;
using Domain.Models.HpInf;
using Helper.Constants;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using UseCase.Document.UploadTemplateToCategory;

namespace Interactor.Document;

public class UploadTemplateToCategoryInteractor : IUploadTemplateToCategoryInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly AmazonS3Options _options;

    public UploadTemplateToCategoryInteractor(IOptions<AmazonS3Options> optionsAccessor, IDocumentRepository documentRepository, IAmazonS3Service amazonS3Service, IHpInfRepository hpInfRepository)
    {
        _options = optionsAccessor.Value;
        _documentRepository = documentRepository;
        _amazonS3Service = amazonS3Service;
        _hpInfRepository = hpInfRepository;
    }

    public UploadTemplateToCategoryOutputData Handle(UploadTemplateToCategoryInputData inputData)
    {
        var regxFile = @"^.*\.(docx|DOCX|xlsx|XLSX)$";
        var rg = new Regex(regxFile);
        if (rg.Matches(inputData.FileName).Count == 0)
        {
            return new UploadTemplateToCategoryOutputData(UploadTemplateToCategoryStatus.InvalidExtentionFile);
        }
        else if (!_hpInfRepository.CheckHpId(inputData.HpId))
        {
            return new UploadTemplateToCategoryOutputData(UploadTemplateToCategoryStatus.InvalidHpId);
        }
        else if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.CategoryCd))
        {
            return new UploadTemplateToCategoryOutputData(UploadTemplateToCategoryStatus.InvalidCategoryCd);
        }
        var memoryStream = inputData.StreamImage.ToMemoryStreamAsync().Result;
        if (memoryStream.Length == 0)
        {
            return new UploadTemplateToCategoryOutputData(UploadTemplateToCategoryStatus.InvalidFileInput);
        }
        try
        {
            var listFolderPath = new List<string>(){
                                                   CommonConstants.Reference,
                                                   CommonConstants.Files,
                                                   inputData.CategoryCd.ToString()
                                                };
            string filePath = _amazonS3Service.GetFolderUploadOther(listFolderPath);

            if (!inputData.OverWrite && _amazonS3Service.ObjectExistsAsync(filePath + inputData.FileName).Result)
            {
                return new UploadTemplateToCategoryOutputData(UploadTemplateToCategoryStatus.ExistFileTemplateName);
            }

            var response = _amazonS3Service.UploadObjectAsync(filePath, inputData.FileName, memoryStream);
            response.Wait();
            if (response.Result.Length > 0)
            {
                return new UploadTemplateToCategoryOutputData(UploadTemplateToCategoryStatus.Successed);
            }
            return new UploadTemplateToCategoryOutputData(UploadTemplateToCategoryStatus.Failed);
        }
        catch
        {
            return new UploadTemplateToCategoryOutputData(UploadTemplateToCategoryStatus.Failed);
        }
        finally
        {
            _documentRepository.ReleaseResource();
            _hpInfRepository.ReleaseResource();
        }
    }
}
