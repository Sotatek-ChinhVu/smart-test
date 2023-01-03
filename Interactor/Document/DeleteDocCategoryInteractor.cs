using Domain.Models.Document;
using Domain.Models.PatientInfor;
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.Document.DeleteDocCategory;

namespace Interactor.Document;

public class DeleteDocCategoryInteractor : IDeleteDocCategoryInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IUserRepository _userRepository;
    private readonly AmazonS3Options _options;

    public DeleteDocCategoryInteractor(IOptions<AmazonS3Options> optionsAccessor, IDocumentRepository documentRepository, IAmazonS3Service amazonS3Service, IPatientInforRepository patientInforRepository, IUserRepository userRepository)
    {
        _options = optionsAccessor.Value;
        _documentRepository = documentRepository;
        _amazonS3Service = amazonS3Service;
        _patientInforRepository = patientInforRepository;
        _userRepository = userRepository;
    }
    public DeleteDocCategoryOutputData Handle(DeleteDocCategoryInputData inputData)
    {
        try
        {
            var ptInf = inputData.MoveToCategoryCd > 0 ? _patientInforRepository.GetById(inputData.HpId, inputData.PtId, 0, 0) : null;
            var responseValidate = ValidateInput(inputData, ptInf);
            if (responseValidate != DeleteDocCategoryStatus.ValidateSuccess)
            {
                return new DeleteDocCategoryOutputData(responseValidate);
            }

            var listFolderPath = new List<string>(){
                                                   CommonConstants.Reference,
                                                   CommonConstants.Files,
                                                   inputData.CategoryCd.ToString()
                                                };
            string pathTemplateCategory = _amazonS3Service.GetFolderUploadOther(listFolderPath);

            var responseCategoryTemplate = _amazonS3Service.GetListObjectAsync(pathTemplateCategory);
            var listCategoryTemplates = responseCategoryTemplate.Result;

            var ptNum = ptInf?.PtNum ?? 0;
            if (MoveAction(inputData.HpId, inputData.UserId, inputData.PtId, ptNum, inputData.CategoryCd, inputData.MoveToCategoryCd, pathTemplateCategory, listCategoryTemplates)
                && _documentRepository.DeleteDocCategory(inputData.HpId, inputData.UserId, inputData.CategoryCd))
            {
                return new DeleteDocCategoryOutputData(DeleteDocCategoryStatus.Successed);
            }
            return new DeleteDocCategoryOutputData(DeleteDocCategoryStatus.Failed);
        }
        finally
        {
            _documentRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _userRepository.ReleaseResource();
        }
    }

    private DeleteDocCategoryStatus ValidateInput(DeleteDocCategoryInputData inputData, PatientInforModel? ptInf)
    {
        if (inputData.MoveToCategoryCd > 0 && ptInf == null)
        {
            return DeleteDocCategoryStatus.InvalidPtId;
        }
        else if (inputData.CategoryCd > 0 && !_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.CategoryCd))
        {
            return DeleteDocCategoryStatus.DocCategoryNotFound;
        }
        else if (inputData.MoveToCategoryCd > 0 && !_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.MoveToCategoryCd))
        {
            return DeleteDocCategoryStatus.MoveDocCategoryNotFound;
        }
        else if (inputData.UserId > 0 && !_userRepository.CheckExistedUserId(inputData.UserId))
        {
            return DeleteDocCategoryStatus.InvalidUserId;
        }
        return DeleteDocCategoryStatus.ValidateSuccess;
    }

    private bool MoveAction(int hpId, int userId, long ptId, long ptNum, int categoryCd, int moveToCategoryCd, string pathTemplateCategory, List<string> listCategoryTemplates)
    {
        var host = _options.BaseAccessUrl;
        if (moveToCategoryCd > 0)
        {
            var listFolderPath = new List<string>(){
                                                   CommonConstants.Reference,
                                                   CommonConstants.Files,
                                                   moveToCategoryCd.ToString()
                                                };
            string pathDestinationTemplateCategory = _amazonS3Service.GetFolderUploadOther(listFolderPath);
            foreach (var item in listCategoryTemplates)
            {
                var sourceFile = item.Replace(host, string.Empty);
                var destinationFile = item.Replace(host, string.Empty).Replace(pathTemplateCategory, pathDestinationTemplateCategory);
                if (_amazonS3Service.ObjectExistsAsync(destinationFile).Result)
                {
                    int indexExtentFile = destinationFile.LastIndexOf(".");
                    if (indexExtentFile > 0)
                    {
                        string extentFile = destinationFile.Substring(indexExtentFile);
                        destinationFile = destinationFile.Replace(extentFile, "_" + DateTime.UtcNow.ToString("yyyyMMdd_HHMMss")) + extentFile;
                    }
                }
                _amazonS3Service.MoveObjectAsync(sourceFile, destinationFile);
            }
            _documentRepository.MoveDocInf(hpId, userId, categoryCd, moveToCategoryCd);
        }
        else
        {

            var listFolderPath = new List<string>(){
                                                   CommonConstants.Store,
                                                   CommonConstants.Files
                                                };
            string pathDocInf = _amazonS3Service.GetFolderUploadToPtNum(listFolderPath, ptNum);

            _amazonS3Service.DeleteObjectAsync(pathTemplateCategory);
            _amazonS3Service.DeleteObjectAsync(pathDocInf);
            _documentRepository.DeleteDocInfs(hpId, userId, ptId, categoryCd);
        }
        return true;
    }
}
