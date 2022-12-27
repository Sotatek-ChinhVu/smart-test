using Domain.Models.Document;
using Domain.Models.HpInf;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Interfaces;
using UseCase.Document.DeleteDocInf;

namespace Interactor.Document;

public class DeleteDocInfInteractor : IDeleteDocInfInputPort
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IPatientInforRepository _patientInforRepository;

    public DeleteDocInfInteractor(IDocumentRepository documentRepository, IAmazonS3Service amazonS3Service, IPatientInforRepository patientInforRepository)
    {
        _documentRepository = documentRepository;
        _amazonS3Service = amazonS3Service;
        _patientInforRepository = patientInforRepository;
    }
    public DeleteDocInfOutputData Handle(DeleteDocInfInputData inputData)
    {
        try
        {
            var docInfDetail = _documentRepository.GetDocInfDetail(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.SeqNo);
            if (docInfDetail.RaiinNo == 0)
            {
                return new DeleteDocInfOutputData(DeleteDocInfStatus.DocInfNotFound);
            }
            var ptNum = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, 0, 0)?.PtNum ?? 0;
            var listFolderPath = new List<string>(){
                                                   CommonConstants.Store,
                                                   CommonConstants.Files
                                                };
            string path = _amazonS3Service.GetFolderUploadToPtNum(listFolderPath, ptNum);
            var response = _amazonS3Service.DeleteObjectAsync(path + docInfDetail.FileName);
            response.Wait();
            if (response.Result && _documentRepository.DeleteDocInf(inputData.HpId, inputData.UserId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.SeqNo))
            {
                return new DeleteDocInfOutputData(DeleteDocInfStatus.Successed);
            }
            return new DeleteDocInfOutputData(DeleteDocInfStatus.Failed);
        }
        catch (Exception)
        {
            return new DeleteDocInfOutputData(DeleteDocInfStatus.Failed);
        }
        finally
        {
            _documentRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
        }
    }
}
