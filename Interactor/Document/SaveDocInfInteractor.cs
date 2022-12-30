using Domain.Models.Document;
using Domain.Models.HpInf;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using System.Text.RegularExpressions;
using UseCase.Document.SaveDocInf;

namespace Interactor.Document;

public class SaveDocInfInteractor : ISaveDocInfInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReceptionRepository _receptionRepository;

    public SaveDocInfInteractor(IDocumentRepository documentRepository, IAmazonS3Service amazonS3Service, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository, IUserRepository userRepository, IReceptionRepository receptionRepository)
    {
        _documentRepository = documentRepository;
        _amazonS3Service = amazonS3Service;
        _hpInfRepository = hpInfRepository;
        _patientInforRepository = patientInforRepository;
        _userRepository = userRepository;
        _receptionRepository = receptionRepository;
    }

    public SaveDocInfOutputData Handle(SaveDocInfInputData inputData)
    {
        try
        {
            var resultValidate = ValidateInputData(inputData);
            if (resultValidate != SaveDocInfStatus.ValidateSuccess)
            {
                return new SaveDocInfOutputData(resultValidate);
            }

            // upload file to S3
            var memoryStream = inputData.StreamImage.ToMemoryStreamAsync().Result;
            if (memoryStream.Length == 0 && inputData.SeqNo <= 0)
            {
                return new SaveDocInfOutputData(SaveDocInfStatus.InvalidFileInput);
            }
            else if (memoryStream.Length > 0 && inputData.SeqNo <= 0)
            {
                var ptNum = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, 0, 0)?.PtNum ?? 0;
                var listFolderPath = new List<string>(){
                                                   CommonConstants.Store,
                                                   CommonConstants.Files
                                                };
                string path = _amazonS3Service.GetFolderUploadToPtNum(listFolderPath, ptNum);
                string fileName = _amazonS3Service.GetUniqueFileNameKey(inputData.FileName.Trim());
                var response = _amazonS3Service.UploadObjectAsync(path, fileName, memoryStream);
                response.Wait();

                if (response.Result.Length > 0)
                {
                    inputData.SetFileName(fileName);
                }
            }
            if (_documentRepository.SaveDocInf(inputData.UserId, ConvertToDocInfModel(inputData)))
            {
                return new SaveDocInfOutputData(SaveDocInfStatus.Successed);
            }
            return new SaveDocInfOutputData(SaveDocInfStatus.Failed);
        }
        finally
        {
            _documentRepository.ReleaseResource();
            _hpInfRepository.ReleaseResource();
            _receptionRepository.ReleaseResource();
            _userRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
        }
    }

    private DocInfModel ConvertToDocInfModel(SaveDocInfInputData inputData)
    {
        return new DocInfModel(
                                inputData.HpId,
                                inputData.PtId,
                                inputData.SinDate,
                                inputData.RaiinNo,
                                inputData.SeqNo,
                                inputData.CategoryCd,
                                string.Empty,
                                inputData.FileName,
                                inputData.DisplayFileName,
                                DateTime.UtcNow
                            );
    }

    private SaveDocInfStatus ValidateInputData(SaveDocInfInputData inputData)
    {
        var regxFile = @"^.*\.(docx|DOCX|xls|XLS|xlsx|XLSX)$";
        var rg = new Regex(regxFile);
        if (inputData.SinDate.ToString().Length != 8)
        {
            return SaveDocInfStatus.InvalidSindate;
        }
        else if (!_userRepository.CheckExistedUserId(inputData.UserId))
        {
            return SaveDocInfStatus.InvalidUserId;
        }
        else if (!_documentRepository.CheckExistDocCategory(inputData.HpId, inputData.CategoryCd))
        {
            return SaveDocInfStatus.InvalidCategoryCd;
        }
        else if (inputData.DisplayFileName.Length == 0)
        {
            return SaveDocInfStatus.InvalidDisplayFileName;
        }
        else if (rg.Matches(inputData.FileName).Count == 0 && inputData.SeqNo <= 0)
        {
            return SaveDocInfStatus.InvalidDocInfFileName;
        }
        if (inputData.SeqNo > 0)
        {
            var docInfDetail = _documentRepository.GetDocInfDetail(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.SeqNo);
            if (docInfDetail != null)
            {
                return SaveDocInfStatus.ValidateSuccess;
            }
        }
        else
        {
            if (!_hpInfRepository.CheckHpId(inputData.HpId))
            {
                return SaveDocInfStatus.InvalidHpId;
            }
            else if (!_patientInforRepository.CheckExistListId(new List<long> { inputData.PtId }))
            {
                return SaveDocInfStatus.InvalidPtId;
            }
            else if (!_receptionRepository.CheckExistRaiinNo(inputData.HpId, inputData.PtId, inputData.RaiinNo))
            {
                return SaveDocInfStatus.InvalidRaiinNo;
            }
        }
        return SaveDocInfStatus.ValidateSuccess;
    }
}
