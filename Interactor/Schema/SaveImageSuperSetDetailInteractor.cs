﻿using Domain.Models.KarteInfs;
using Domain.Models.SuperSetDetail;
using Helper.Constants;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.Schema.SaveImageSuperSetDetail;

namespace Interactor.Schema;

public class SaveImageSuperSetDetailInteractor : ISaveImageSuperSetDetailInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly AmazonS3Options _options;
    private readonly ISuperSetDetailRepository _superSetDetailRepository;

    public SaveImageSuperSetDetailInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service, ISuperSetDetailRepository superSetDetailRepository)
    {
        _amazonS3Service = amazonS3Service;
        _options = optionsAccessor.Value;
        _superSetDetailRepository = superSetDetailRepository;
    }
    public SaveImageSuperSetDetailOutputData Handle(SaveImageSuperSetDetailInputData input)
    {
        try
        {
            if (input.SetCd <= 0)
            {
                return new SaveImageSuperSetDetailOutputData(SaveImageSuperSetDetailStatus.InvalidSetCd);
            }

            List<SetKarteImgInfModel> listImageSaveTemps = new();

            // Delete old image
            if (!string.IsNullOrEmpty(input.OldImage))
            {
                string key = input.OldImage.Replace(_options.BaseAccessUrl + "/", String.Empty);
                if (_amazonS3Service.ObjectExistsAsync(key).Result)
                {
                    var isDelete = _amazonS3Service.DeleteObjectAsync(key).Result;
                    if (!isDelete)
                    {
                        return new SaveImageSuperSetDetailOutputData(SaveImageSuperSetDetailStatus.InvalidOldImage);
                    }
                    listImageSaveTemps.Add(new SetKarteImgInfModel(
                            input.HpId,
                            input.SetCd,
                            input.Position,
                            String.Empty,
                            key
                        ));
                }
                else
                {
                    return new SaveImageSuperSetDetailOutputData(SaveImageSuperSetDetailStatus.InvalidOldImage);
                }
            }

            // Insert new image
            var memoryStream = input.StreamImage.ToMemoryStreamAsync().Result;
            var subFolder = CommonConstants.SubFolderSuperSet + "/" + input.SetCd.ToString();

            if (memoryStream.Length <= 0 && string.IsNullOrEmpty(input.OldImage))
            {
                return new SaveImageSuperSetDetailOutputData(SaveImageSuperSetDetailStatus.InvalidFileImage);
            }
            if (memoryStream.Length > 0)
            {
                string fileName = input.SetCd.ToString().PadLeft(10, '0') + ".png";
                var responseUpload = _amazonS3Service.UploadAnObjectAsync(false, subFolder, fileName, memoryStream);
                var linkImage = responseUpload.Result;
                listImageSaveTemps.Add(new SetKarteImgInfModel(
                                    input.HpId,
                                    input.SetCd,
                                    input.Position,
                                    linkImage.Replace(_options.BaseAccessUrl + "/", String.Empty),
                                    String.Empty
                                  ));
                _superSetDetailRepository.SaveListSetKarteImgTemp(listImageSaveTemps);
                return new SaveImageSuperSetDetailOutputData(linkImage, SaveImageSuperSetDetailStatus.Successed);
            }
            _superSetDetailRepository.SaveListSetKarteImgTemp(listImageSaveTemps);
            return new SaveImageSuperSetDetailOutputData(SaveImageSuperSetDetailStatus.DeleteSuccessed);
        }
        catch (Exception)
        {
            return new SaveImageSuperSetDetailOutputData(SaveImageSuperSetDetailStatus.Failed);
        }
    }
}
