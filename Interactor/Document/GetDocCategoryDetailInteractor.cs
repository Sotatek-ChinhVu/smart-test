using Domain.Models.Document;
using Domain.Models.HpInf;
using Domain.Models.PatientInfor;
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
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly AmazonS3Options _options;

    public GetDocCategoryDetailInteractor(IOptions<AmazonS3Options> optionsAccessor, IDocumentRepository documentRepository, IAmazonS3Service amazonS3Service, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository)
    {
        _options = optionsAccessor.Value;
        _documentRepository = documentRepository;
        _amazonS3Service = amazonS3Service;
        _hpInfRepository = hpInfRepository;
        _patientInforRepository = patientInforRepository;
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
        var ptInf = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, 0, 0);
        try
        {
            var docCategory = _documentRepository.GetDocCategoryDetail(inputData.HpId, inputData.CategoryCd);
            var listDocumentTemplate = GetListDocumentTemplate(inputData.CategoryCd);
            var listDocInf = GetListDocInf(inputData.HpId, inputData.PtId, inputData.CategoryCd, ptInf != null ? ptInf.PtNum : 0);
            var category = new DocCategoryItem(
                                                docCategory.CategoryCd,
                                                docCategory.CategoryName,
                                                docCategory.SortNo
                                            );
            return new GetDocCategoryDetailOutputData(
                                                        category,
                                                        listDocumentTemplate,
                                                        listDocInf,
                                                        GetDocCategoryDetailStatus.Successed
                                                    );
        }
        catch
        {
            return new GetDocCategoryDetailOutputData(GetDocCategoryDetailStatus.Failed);
        }
        finally
        {
            _documentRepository.ReleaseResource();
            _hpInfRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
        }
    }

    private List<FileDocumentModel> GetListDocumentTemplate(int categoryId)
    {
        List<FileDocumentModel> result = new();
        var listFolderPath = new List<string>(){
                                                   CommonConstants.Reference,
                                                   CommonConstants.Files,
                                                   categoryId.ToString()
                                                };
        string path = _amazonS3Service.GetFolderUploadOther(listFolderPath);
        var response = _amazonS3Service.GetListObjectAsync(path);
        response.Wait();
        var listOutputData = response.Result;
        if (listOutputData.Any())
        {
            StringBuilder domainUrl = new StringBuilder();
            domainUrl.Append(_options.BaseAccessUrl + "/");
            var listFileItem = listOutputData
                                        .Select(file => new FileDocumentModel(
                                                file.Replace(path, string.Empty).Replace(categoryId + "/", string.Empty),
                                                domainUrl + file
                                            )).ToList();
            result.AddRange(listFileItem.Where(item => !string.IsNullOrWhiteSpace(item.FileName)).Distinct().ToList());
        }
        return result;
    }

    private List<DocInfModel> GetListDocInf(int hpId, long ptId, int categoryCd, long ptNum)
    {
        var listDocInf = _documentRepository.GetDocInfByCategoryCd(hpId, ptId, categoryCd);
        var listFolderPath = new List<string>(){
                                                   CommonConstants.Store,
                                                   CommonConstants.Files
                                                };
        string path = _amazonS3Service.GetFolderUploadToPtNum(listFolderPath, ptNum);
        var response = _amazonS3Service.GetListObjectAsync(path);
        response.Wait();
        var listOutputFiles = response.Result;
        if (listOutputFiles.Any())
        {
            StringBuilder domainUrl = new StringBuilder();
            domainUrl.Append(_options.BaseAccessUrl + "/");
            foreach (var model in listDocInf)
            {
                var listFiles = listOutputFiles
                                            .Where(file =>
                                                        (path + model.FileName).Equals(file)
                                                        && file.Length > path.Length)
                                            .ToList();
                var fileItem = listFiles.FirstOrDefault();
                if (fileItem != null)
                {
                    model.SetFileLinkForDocInf(domainUrl + fileItem);
                }
            }
        }
        return listDocInf;
    }
}
