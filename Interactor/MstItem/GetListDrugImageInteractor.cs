using Domain.Models.Insurance;
using Helper.Constants;
using Helper.Enum;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using System;
using System.Text;
using UseCase.MstItem.GetListDrugImage;

namespace Interactor.MstItem
{
    public class GetListDrugImageInteractor : IGetListDrugImageInputPort
    {
        private readonly IAmazonS3Service _amazonS3Service;

        public GetListDrugImageInteractor(IAmazonS3Service amazonS3Service, IInsuranceRepository insuranceRepository)
        {
            _amazonS3Service = amazonS3Service;
        }

        public GetListDrugImageOutputData Handle(GetListDrugImageInputData inputData)
        {
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
                return new GetListDrugImageOutputData(GetListDrugImageStatus.InvalidTypeImage, new());
            }

            string path = BuildPathAws(folderPaths);
            List<string> images = _amazonS3Service.GetListObjectAsync(path).Result;
            if (!images.Any())
                return new GetListDrugImageOutputData(GetListDrugImageStatus.NoData, new());
            else
            {
                string prefix = _amazonS3Service.GetAccessBaseS3();
                images = images.Where(u => (u.Contains(".png") || u.Contains(".PNG") ||
                                u.Contains(".jpg") || u.Contains(".JPG") ||
                                u.Contains(".jpeg") || u.Contains(".JPEG") ||
                                u.Contains(".ico") || u.Contains(".ICO")) && u.Contains(inputData.YjCd))
                               .OrderBy(u => u)
                               .Select(x => prefix + x)
                               .ToList();

                List<DrugImageOutputItem> result = new()
                {
                    SetItemData(string.Empty, inputData.YjCd, inputData.SelectedImage, images)
                };

                foreach (var extention in "ABCDEFGHIJZ")
                {
                    var drugImageItem = SetItemData(extention.ToString(), inputData.YjCd, inputData.SelectedImage, images);
                    if (extention.ToString().Equals("A") && string.IsNullOrEmpty(result.First().FileLink))
                    {
                        result = new();
                    }
                    if (extention.ToString().Equals("Z") && result.Count == 10)
                    {
                        result.Add(new());
                    }
                    result.Add(drugImageItem);
                }

                return new GetListDrugImageOutputData(GetListDrugImageStatus.Successful, result);
            }
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

        private DrugImageOutputItem SetItemData(string extentionKey, string yjCd, string selectedImage, List<string> images)
        {
            string keyCheck = yjCd + extentionKey.ToString();
            var imageLink = images.FirstOrDefault(link => Path.GetFileName(link).Equals(keyCheck + Path.GetExtension(link)));
            DrugImageOutputItem drugImageItem = new(imageLink ?? string.Empty,
                                                    !string.IsNullOrEmpty(imageLink),
                                                    selectedImage.Equals(Path.GetFileName(imageLink)));
            return drugImageItem;
        }
    }
}
