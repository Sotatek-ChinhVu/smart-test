using Domain.Models.KarteInfs;
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
    private readonly IKarteInfRepository _setKbnMstRepository;

    public SaveImageInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service, IKarteInfRepository setKbnMstRepository)
    {
        _amazonS3Service = amazonS3Service;
        _options = optionsAccessor.Value;
        _setKbnMstRepository = setKbnMstRepository;
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
                string key = input.OldImage.Replace(_options.BaseAccessUrl + "/", String.Empty);
                if (_amazonS3Service.ObjectExistsAsync(key).Result)
                {
                    var isDelete = _amazonS3Service.DeleteObjectAsync(key).Result;
                    if (!isDelete)
                    {
                        return new SaveImageOutputData(SaveImageStatus.InvalidOldImage);
                    }
                    _setKbnMstRepository.SaveImageKarteImgTemp(new KarteImgInfModel(
                            input.HpId,
                            input.PtId,
                            input.RaiinNo,
                            String.Empty,
                            key
                        ));
                }
                else
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
            var responseUpload = _amazonS3Service.UploadAnObjectAsync(subFolder, fileName, memoryStream);
            var linkImage = responseUpload.Result;
            _setKbnMstRepository.SaveImageKarteImgTemp(new KarteImgInfModel(
                            input.HpId,
                            input.PtId,
                            input.RaiinNo,
                            linkImage.Replace(_options.BaseAccessUrl + "/", String.Empty),
                            input.OldImage.Replace(_options.BaseAccessUrl + "/", String.Empty)
                        ));
            return new SaveImageOutputData(linkImage, SaveImageStatus.Successed);
        }
        catch (Exception)
        {
            return new SaveImageOutputData(SaveImageStatus.Failed);
        }
    }
}
