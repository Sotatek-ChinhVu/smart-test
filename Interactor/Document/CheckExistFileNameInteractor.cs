using Domain.Models.Document;
using Domain.Models.HpInf;
using Domain.Models.PatientInfor;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.Document.CheckExistFileName;

namespace Interactor.Document;

public class CheckExistFileNameInteractor : ICheckExistFileNameInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly AmazonS3Options _options;

    public CheckExistFileNameInteractor(IOptions<AmazonS3Options> optionsAccessor, IDocumentRepository documentRepository, IAmazonS3Service amazonS3Service, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository)
    {
        _options = optionsAccessor.Value;
        _documentRepository = documentRepository;
        _amazonS3Service = amazonS3Service;
        _hpInfRepository = hpInfRepository;
        _patientInforRepository = patientInforRepository;
    }

    public CheckExistFileNameOutputData Handle(CheckExistFileNameInputData inputData)
    {
        var resultValidate = ValidateInput(inputData);
        if (resultValidate != CheckExistFileNameStatus.ValidateSuccess)
        {
            return new CheckExistFileNameOutputData(resultValidate);
        }
        try
        {
            string filePath;
            if (inputData.IsCheckDocInf)
            {
                var ptInf = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, 0, 0);
                long ptNum = ptInf != null ? ptInf.PtNum : 0;
                var listFolderPath = new List<string>(){
                                                   CommonConstants.Store,
                                                   CommonConstants.Files
                                                };
                filePath = _amazonS3Service.GetFolderUploadToPtNum(listFolderPath, ptNum) + inputData.FileName;
            }
            else
            {
                var listFolderPath = new List<string>(){
                                                   CommonConstants.Reference,
                                                   CommonConstants.Files,
                                                   inputData.CategoryCd.ToString()
                                                };
                filePath = _amazonS3Service.GetFolderUploadOther(listFolderPath) + inputData.FileName;
            }
            var response = _amazonS3Service.ObjectExistsAsync(filePath);
            response.Wait();
            return new CheckExistFileNameOutputData(response.Result, CheckExistFileNameStatus.Successed);
        }
        catch
        {
            return new CheckExistFileNameOutputData(CheckExistFileNameStatus.Failed);
        }
    }

    private CheckExistFileNameStatus ValidateInput(CheckExistFileNameInputData inputData)
    {
        if (!_hpInfRepository.CheckHpId(inputData.HpId))
        {
            return CheckExistFileNameStatus.InvalidHpId;
        }
        else if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.CategoryCd))
        {
            return CheckExistFileNameStatus.InvalidCategoryCd;
        }
        else if (inputData.FileName.Length == 0)
        {
            return CheckExistFileNameStatus.InvalidFileName;
        }
        if (inputData.IsCheckDocInf && !_patientInforRepository.CheckExistListId(new List<long> { inputData.PtId }))
        {
            return CheckExistFileNameStatus.InvalidPtId;
        }
        return CheckExistFileNameStatus.ValidateSuccess;
    }
}
