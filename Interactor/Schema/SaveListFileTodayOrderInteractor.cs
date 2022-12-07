using Domain.Models.PatientInfor;
using Domain.Models.SuperSetDetail;
using Helper.Constants;
using Infrastructure.Interfaces;
using UseCase.Schema.SaveListFileTodayOrder;

namespace Interactor.Schema;

public class SaveListFileTodayOrderInteractor : ISaveListFileTodayOrderInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly ISuperSetDetailRepository _superSetDetailRepository;

    public SaveListFileTodayOrderInteractor(IAmazonS3Service amazonS3Service, IPatientInforRepository patientInforRepository, ISuperSetDetailRepository superSetDetailRepository)
    {
        _amazonS3Service = amazonS3Service;
        _patientInforRepository = patientInforRepository;
        _superSetDetailRepository = superSetDetailRepository;
    }

    public SaveListFileTodayOrderOutputData Handle(SaveListFileTodayOrderInputData input)
    {
        try
        {
            var ptInf = _patientInforRepository.GetById(input.HpId, input.PtId, 0, 0);
            var validateResponse = ValidateInput(input, ptInf);
            if (validateResponse.Item1 != SaveListFileTodayOrderStatus.ValidateSuccess)
            {
                return new SaveListFileTodayOrderOutputData(validateResponse.Item1);
            }
            var listFileItems = validateResponse.Item2;

            List<string> result = new();
            if (listFileItems.Any())
            {
                var pathResponse = GetPath(input.TypeUpload, ptInf != null ? ptInf.PtNum : 0, input.SetCd);
                if (pathResponse.Item1 != SaveListFileTodayOrderStatus.ValidateSuccess)
                {
                    return new SaveListFileTodayOrderOutputData(pathResponse.Item1);
                }
                string path = pathResponse.Item2;
                foreach (var item in listFileItems)
                {
                    var responseUpload = _amazonS3Service.UploadObjectAsync(path, item.FileName, item.StreamImage);
                    var linkImage = responseUpload.Result;
                    if (linkImage.Length > 0)
                    {
                        result.Add(linkImage);
                    }
                }
            }
            if (result.Any())
            {
                return new SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus.Successed, result);
            }
            return new SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus.Failed);
        }
        catch (Exception)
        {
            return new SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus.Failed);
        }
    }

    private Tuple<SaveListFileTodayOrderStatus, string> GetPath(int typeUpload, long ptNum, int setCd)
    {
        List<string> listFolders = new();
        string path = string.Empty;
        switch (typeUpload)
        {
            case TypeUploadConstant.UploadKarteFile:
                listFolders.Add(CommonConstants.Store);
                listFolders.Add(CommonConstants.Karte);
                path = _amazonS3Service.GetFolderUploadToPtNum(listFolders, ptNum);
                break;
            case TypeUploadConstant.UploadSupperSetDetailFile:
                listFolders.Add(CommonConstants.Store);
                listFolders.Add(CommonConstants.Karte);
                listFolders.Add(CommonConstants.SetPic);
                listFolders.Add(setCd.ToString());
                path = _amazonS3Service.GetFolderUploadOther(listFolders);
                break;
            case TypeUploadConstant.UploadNextOrderFile:
                listFolders.Add(CommonConstants.Store);
                listFolders.Add(CommonConstants.Karte);
                listFolders.Add(CommonConstants.NextPic);
                path = _amazonS3Service.GetFolderUploadToPtNum(listFolders, ptNum);
                break;
            default:
                return Tuple.Create(SaveListFileTodayOrderStatus.InvalidTypeUpload, string.Empty);
        }
        return Tuple.Create(SaveListFileTodayOrderStatus.ValidateSuccess, path);
    }

    private Tuple<SaveListFileTodayOrderStatus, List<FileItem>> ValidateInput(SaveListFileTodayOrderInputData input, PatientInforModel? ptInf)
    {
        List<FileItem> listFileItems = new();
        if (input.TypeUpload == TypeUploadConstant.UploadSupperSetDetailFile && !_superSetDetailRepository.CheckExistSupperSetDetail(input.HpId, input.SetCd))
        {
            return Tuple.Create(SaveListFileTodayOrderStatus.InvalidSetCd, listFileItems);
        }
        else
        {
            if (ptInf == null)
            {
                return Tuple.Create(SaveListFileTodayOrderStatus.InvalidPtId, listFileItems);
            }
        }
        if (input.ListImages.Any())
        {
            foreach (var image in input.ListImages)
            {
                string fileName = _amazonS3Service.GetUniqueFileNameKey(image.FileName);
                if (image.StreamImage.Length > 0)
                {
                    if (fileName.Length > 100)
                    {
                        int lastIndex = fileName.IndexOf(".");
                        string extentFile = fileName.Substring(lastIndex);
                        fileName = fileName.Substring(0, 100 - extentFile.Length - 1) + extentFile;
                    }
                    listFileItems.Add(new FileItem(fileName, image.StreamImage));
                }
                else
                {
                    return Tuple.Create(SaveListFileTodayOrderStatus.InvalidFileImage, listFileItems);
                }
            }
        }
        return Tuple.Create(SaveListFileTodayOrderStatus.ValidateSuccess, listFileItems);
    }
}
