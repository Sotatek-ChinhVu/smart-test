using Domain.Models.KarteInfs;
using Helper.Constants;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.Schema.SaveImageTodayOrder;

namespace Interactor.Schema;

public class SaveImageTodayOrderInteractor : ISaveImageTodayOrderInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly AmazonS3Options _options;
    private readonly IKarteInfRepository _setKbnMstRepository;

    public SaveImageTodayOrderInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service, IKarteInfRepository setKbnMstRepository)
    {
        _amazonS3Service = amazonS3Service;
        _options = optionsAccessor.Value;
        _setKbnMstRepository = setKbnMstRepository;
    }

    public SaveImageTodayOrderOutputData Handle(SaveImageTodayOrderInputData input)
    {
        try
        {
            if (input.PtId <= 0)
            {
                return new SaveImageTodayOrderOutputData(SaveImageTodayOrderStatus.InvalidPtId);
            }

            List<KarteImgInfModel> listImageSaveTemps = new();

            // Delete old image
            if (!string.IsNullOrEmpty(input.OldImage))
            {
                string key = input.OldImage.Replace(_options.BaseAccessUrl + "/", String.Empty);
                if (_amazonS3Service.ObjectExistsAsync(key).Result)
                {
                    var isDelete = _amazonS3Service.DeleteObjectAsync(key).Result;
                    if (!isDelete)
                    {
                        return new SaveImageTodayOrderOutputData(SaveImageTodayOrderStatus.InvalidOldImage);
                    }
                    listImageSaveTemps.Add(new KarteImgInfModel(
                            input.HpId,
                            input.PtId,
                            input.RaiinNo,
                            String.Empty,
                            key
                        ));
                }
                else
                {
                    return new SaveImageTodayOrderOutputData(SaveImageTodayOrderStatus.InvalidOldImage);
                }
            }

            // Insert new image
            var memoryStream = input.StreamImage.ToMemoryStreamAsync().Result;
            var subFolder = CommonConstants.SubFolderKarte;

            if (memoryStream.Length <= 0 && string.IsNullOrEmpty(input.OldImage))
            {
                return new SaveImageTodayOrderOutputData(SaveImageTodayOrderStatus.InvalidFileImage);
            }
            if (memoryStream.Length > 0)
            {
                string fileName = input.PtId.ToString().PadLeft(10, '0') + ".png";
                var responseUpload = _amazonS3Service.UploadAnObjectAsync(true, subFolder, fileName, memoryStream);
                var linkImage = responseUpload.Result;
                listImageSaveTemps.Add(new KarteImgInfModel(
                                    input.HpId,
                                    input.PtId,
                                    input.RaiinNo,
                                    linkImage.Replace(_options.BaseAccessUrl + "/", String.Empty),
                                    String.Empty
                                  ));
                _setKbnMstRepository.SaveListImageKarteImgTemp(listImageSaveTemps);
                return new SaveImageTodayOrderOutputData(linkImage, SaveImageTodayOrderStatus.Successed);
            }
            _setKbnMstRepository.SaveListImageKarteImgTemp(listImageSaveTemps);
            return new SaveImageTodayOrderOutputData(SaveImageTodayOrderStatus.DeleteSuccessed);
        }
        catch (Exception)
        {
            return new SaveImageTodayOrderOutputData(SaveImageTodayOrderStatus.Failed);
        }
    }
}
