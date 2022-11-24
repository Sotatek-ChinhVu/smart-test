using Domain.Models.Insurance;
using Domain.Models.KarteInfs;
using Helper.Constants;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using PostgreDataContext.Migrations;
using Schema.Insurance.SaveInsuranceScan;
using UseCase.Schema.SaveImageTodayOrder;

namespace Interactor.Schema
{
    public class SaveInsuranceScanInteractor : ISaveInsuranceScanInputPort
    {
        private readonly IAmazonS3Service _amazonS3Service;
        private readonly AmazonS3Options _options;
        private readonly IInsuranceRepository _insuranceRepository;

        public SaveInsuranceScanInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service, IInsuranceRepository insuranceRepository)
        {
            _amazonS3Service = amazonS3Service;
            _options = optionsAccessor.Value;
            _insuranceRepository = insuranceRepository;
        }

        /// <summary>
        /// if have stream || oldImageHaveValue then call this
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public SaveInsuranceScanOutputData Handle(SaveInsuranceScanInputData inputData)
        {
            if (inputData.PtId <= 0)
                return new SaveInsuranceScanOutputData(string.Empty, SaveInsuranceScanStatus.InvalidPtId);

            if (inputData.HpId <= 0)
                return new SaveInsuranceScanOutputData(string.Empty, SaveInsuranceScanStatus.InvalidHpId);

            MemoryStream memoryStream = inputData.StreamImage.ToMemoryStreamAsync().Result;

            // Delete old image
            if (!string.IsNullOrEmpty(inputData.OldImage))
            {
                string key = inputData.OldImage.Replace(_options.BaseAccessUrl + "/", string.Empty);
                if (_amazonS3Service.ObjectExistsAsync(key).Result)
                {
                    var isDelete = _amazonS3Service.DeleteObjectAsync(key).Result;
                    bool removeDb = _insuranceRepository.DeleteInsuranceScan(new InsuranceScanModel(inputData.HpId,
                                                                                                    inputData.PtId,
                                                                                                    inputData.HokenGrp,
                                                                                                    inputData.HokenId,
                                                                                                    string.Empty,
                                                                                                    0), inputData.UserId);
                    if (!isDelete || !removeDb)
                        return new SaveInsuranceScanOutputData(string.Empty, SaveInsuranceScanStatus.RemoveOldImageFailed);

                }
                else
                    return new SaveInsuranceScanOutputData(string.Empty , SaveInsuranceScanStatus.OldImageNotFound);
            }

            string subFolder = CommonConstants.SubFolderInsuranceScan;

            if (memoryStream.Length <= 0 && string.IsNullOrEmpty(inputData.OldImage))
            {
                if(!string.IsNullOrEmpty(inputData.OldImage)) //OldImage is not null and has removed above. 
                {
                    return new SaveInsuranceScanOutputData(string.Empty, SaveInsuranceScanStatus.RemoveOldImageSuccessful);
                }    

                return new SaveInsuranceScanOutputData(string.Empty, SaveInsuranceScanStatus.InvalidImageScan);
            }
                

            if (memoryStream.Length > 0)
            {
                string fileName = $"{inputData.PtId.ToString().PadLeft(10, '0')}{inputData.HokenId}.png";

                Task<string> responseUpload = _amazonS3Service.UploadAnObjectAsync(true, subFolder, fileName, memoryStream);
                string linkImage = responseUpload.Result;


                bool saveToDb = _insuranceRepository.SaveInsuraneScan(new InsuranceScanModel(inputData.HpId,
                                                                            inputData.PtId,
                                                                            inputData.HokenGrp,
                                                                            inputData.HokenId,
                                                                            linkImage,
                                                                           0), inputData.UserId);
                if(!saveToDb)
                    return new SaveInsuranceScanOutputData(string.Empty, SaveInsuranceScanStatus.FailedSaveToDb);

                return new SaveInsuranceScanOutputData(linkImage, SaveInsuranceScanStatus.Successful);
            }

            return new SaveInsuranceScanOutputData(string.Empty ,SaveInsuranceScanStatus.Failed);
        }
    }
}
