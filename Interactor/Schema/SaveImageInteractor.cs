using Helper.Constants;
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

    public SaveImageOutputData Handle(SaveImageInputData inputData)
    {
        try
        {
            var subFolder = CommonConstants.SubFolderKarte;
            if (!string.IsNullOrEmpty(inputData.OldImage))
            {
                string key = _options.AwsAccessKeyId.
                var response = _amazonS3Service.GetListObjectAsync(SchemaConst.Schema);
                response.Wait();
            }


            return new SaveImageOutputData(string.Empty, SaveImageStatus.Successed);
        }
        catch (Exception)
        {
            return new SaveImageOutputData(string.Empty, SaveImageStatus.Failed);
        }
    }
}
