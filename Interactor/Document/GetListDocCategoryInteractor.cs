using Domain.Models.Document;
using Domain.Models.HpInf;
using Domain.Models.PatientInfor;
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
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly AmazonS3Options _options;

    public GetListDocCategoryInteractor(IOptions<AmazonS3Options> optionsAccessor, IDocumentRepository documentRepository, IAmazonS3Service amazonS3Service, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository)
    {
        _options = optionsAccessor.Value;
        _documentRepository = documentRepository;
        _amazonS3Service = amazonS3Service;
        _hpInfRepository = hpInfRepository;
        _patientInforRepository = patientInforRepository;
    }

    public GetListDocCategoryOutputData Handle(GetListDocCategoryInputData inputData)
    {
        if (!_hpInfRepository.CheckHpId(inputData.HpId))
        {
            return new GetListDocCategoryOutputData(GetListDocCategoryStatus.InvalidHpId);
        }
        var ptInf = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, 0, 0);
        try
        {
            var listDocCategory = _documentRepository.GetAllDocCategory(inputData.HpId);
            var listCategory = listDocCategory.Select(model => ConvertToDocCategoryMstOutputItem(model)).ToList();
            var listDocumentTemplate = GetListDocumentTemplate(listDocCategory.Select(item => item.CategoryCd).ToList());
            var listDocInf = GetListDocInf(inputData.HpId, inputData.PtId, ptInf != null ? ptInf.PtNum : 0);
            return new GetListDocCategoryOutputData(
                                                        listCategory,
                                                        listDocumentTemplate,
                                                        listDocInf,
                                                        GetListDocCategoryStatus.Successed
                                                    );
        }
        finally
        {
            _documentRepository.ReleaseResource();
            _hpInfRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
        }
    }

    private DocCategoryItem ConvertToDocCategoryMstOutputItem(DocCategoryModel model)
    {
        return new DocCategoryItem(
                                        model.CategoryCd,
                                        model.CategoryName,
                                        model.SortNo
                                  );
    }

    private List<FileDocumentModel> GetListDocumentTemplate(List<int> listCategoryId)
    {
        List<FileDocumentModel> result = new();
        var listFolderPath = new List<string>(){
                                                   CommonConstants.Reference,
                                                   CommonConstants.Files
                                                };
        string path = _amazonS3Service.GetFolderUploadOther(listFolderPath);
        var response = _amazonS3Service.GetListObjectAsync(path);
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
                                                    catId,
                                                    file.Replace(path, string.Empty).Replace(catId + "/", string.Empty),
                                                    domainUrl + file
                                             )).ToList();
                result.AddRange(listFileItem.Where(item => !string.IsNullOrWhiteSpace(item.FileName)).Distinct().ToList());
            }
        }
        return result;
    }

    private List<DocInfModel> GetListDocInf(int hpId, long ptId, long ptNum)
    {
        var listDocInf = _documentRepository.GetAllDocInf(hpId, ptId);
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
