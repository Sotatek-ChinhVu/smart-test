using Helper.Constants;
using Helper.Enum;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using System.IO;
using System.Text;
using UseCase.Document.UploadTemplateToCategory;
using UseCase.MstItem.GetListDrugImage;
using UseCase.MstItem.UploadImageDrugInf;

namespace Interactor.MstItem;

public class UploadImageDrugInfInteractor : IUploadImageDrugInfInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;

    public UploadImageDrugInfInteractor(IAmazonS3Service amazonS3Service)
    {
        _amazonS3Service = amazonS3Service;
    }

    public UploadImageDrugInfOutputData Handle(UploadImageDrugInfInputData inputData)
    {
        string linkImage = string.Empty;
        List<string> folderPaths = new List<string>() { CommonConstants.Image, CommonConstants.Reference, CommonConstants.DrugPhoto };
        if (inputData.Type == ImageTypeDrug.HouImage)
        {
            folderPaths.Add(CommonConstants.HouSou);
        }
        else if (inputData.Type == ImageTypeDrug.ZaiImage)
        {
            folderPaths.Add(CommonConstants.ZaiKei);
        }
        else
        {
            return new UploadImageDrugInfOutputData(linkImage, UploadImageDrugInfStatus.InvalidTypeImage);
        }

        string path = BuildPathAws(folderPaths);
        if (inputData.IsDeleted)
        {
            var response = _amazonS3Service.DeleteObjectAsync(path + inputData.YjCd + "Z.jpg");
            response.Wait();
            if (response.Result)
            {
                return new UploadImageDrugInfOutputData(linkImage, UploadImageDrugInfStatus.Successed);
            }
        }
        else
        {
            var memoryStream = inputData.StreamImage.ToMemoryStreamAsync().Result;
            if (memoryStream.Length == 0)
            {
                return new UploadImageDrugInfOutputData(linkImage, UploadImageDrugInfStatus.InvalidFileInput);
            }
            var response = _amazonS3Service.UploadObjectAsync(path, inputData.YjCd + "Z.jpg", memoryStream);
            response.Wait();
            linkImage = response.Result;
            if (linkImage.Length > 0)
            {
                return new UploadImageDrugInfOutputData(linkImage, UploadImageDrugInfStatus.Successed);
            }
        }
        return new UploadImageDrugInfOutputData(linkImage, UploadImageDrugInfStatus.Failed);
    }

    private string BuildPathAws(List<string> folders)
    {
        StringBuilder result = new();
        foreach (var item in folders)
        {
            result.Append(item);
            result.Append("/");
        }
        return result.ToString();
    }
}
