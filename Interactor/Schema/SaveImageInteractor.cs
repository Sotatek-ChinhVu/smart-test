using Helper.Constants;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.Schema.SaveImage;

namespace Interactor.Schema;

public class SaveImageInteractor : ISaveImageInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly AmazonS3Options _options;

    public SaveImageInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service)
    {
        _amazonS3Service = amazonS3Service;
        _options = optionsAccessor.Value;
    }

    public SaveImageOutputData Handle(SaveImageInputData input)
    {
        try
        {
            if (input.PtId <= 0)
            {
                return new SaveImageOutputData(SaveImageStatus.InvalidPtId);
            }

            // Delete old image
            if (!string.IsNullOrEmpty(input.OldImage))
            {
                string key = input.OldImage.Replace(_options.BaseAccessUrl + "/", "");
                var isDelete = _amazonS3Service.DeleteObjectAsync(key).Result;
                if (!isDelete)
                {
                    return new SaveImageOutputData(SaveImageStatus.InvalidOldImage);
                }
            }

            // Insert new image
            var memoryStream = input.StreamImage.ToMemoryStreamAsync().Result;
            var subFolder = CommonConstants.SubFolderKarte;

            if (memoryStream.Length <= 0)
            {
                return new SaveImageOutputData(SaveImageStatus.InvalidFileImage);
            }
            
            string fileName = input.PtId.ToString().PadLeft(10, '0') + ".png";
            var resUpload = _amazonS3Service.UploadAnObjectAsync(subFolder, fileName, memoryStream);
            return new SaveImageOutputData(resUpload.Result, SaveImageStatus.Successed);
        }
        catch (Exception)
        {
            return new SaveImageOutputData(SaveImageStatus.Failed);
        }
    }
}
