using Domain.Models.DrugInfor;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.DrugInfor.Get;

namespace Interactor.DrugInfor
{
    public class GetDrugInforInteractor : IGetDrugInforInputPort
    {
        private readonly IDrugInforRepository _drugInforRepository;
        private readonly IAmazonS3Service _amazonS3Service;
        private readonly IConfiguration _configuration;

        public GetDrugInforInteractor(IDrugInforRepository drugInforRepository, IConfiguration configuration, IAmazonS3Service amazonS3Service)
        {
            _drugInforRepository = drugInforRepository;
            _configuration = configuration;
            _amazonS3Service = amazonS3Service;
        }

        public GetDrugInforOutputData Handle(GetDrugInforInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetDrugInforOutputData(new DrugInforModel(), GetDrugInforStatus.InValidHpId);
                }

                if (inputData.SinDate <= 0)
                {
                    return new GetDrugInforOutputData(new DrugInforModel(), GetDrugInforStatus.InValidSindate);
                }

                if (String.IsNullOrEmpty(inputData.ItemCd))
                {
                    return new GetDrugInforOutputData(new DrugInforModel(), GetDrugInforStatus.InValidItemCd);
                }

                var data = _drugInforRepository.GetDrugInfor(inputData.HpId, inputData.SinDate, inputData.ItemCd);
                var listPicHou = new List<string>();
                var listPicZai = new List<string>();

                if (!String.IsNullOrEmpty(data.OtherPicZai))
                {
                    data.PathPicZai = data.OtherPicZai;
                }
                else
                {
                    data.PathPicZai = GetPathImagePic(data.YjCode, data.DefaultPathPicZai, data.CustomPathPicZai, listPicZai);
                }

                if (!String.IsNullOrEmpty(data.OtherPicHou))
                {
                    data.PathPicHou = data.OtherPicHou;
                }
                else
                {
                    data.PathPicHou = GetPathImagePic(data.YjCode, data.DefaultPathPicHou, data.CustomPathPicHou, listPicHou);
                }

                //set list image
                data.ListPicHou = listPicHou;
                data.ListPicZai = listPicZai;

                return new GetDrugInforOutputData(data, GetDrugInforStatus.Successed);
            }
            finally
            {
                _drugInforRepository.ReleaseResource();
            }
        }

        private string GetPathImagePic(string yjCode, string defaultPath, string customPath, List<string> listPic)
        {
            var defaultImgPic = "";

            var _picStr = " ABCDEFGHIJZ";
            var pathServerS3 = _configuration["PathImageDrugServer"];
            for (int i = 0; i < _picStr.Length - 1; i++)
            {
                if (!String.IsNullOrEmpty(yjCode))
                {
                    var keyImage = "";
                    if (defaultPath.Contains(pathServerS3 + "/"))
                    {
                        keyImage = (defaultPath.Replace(pathServerS3 + "/", "") + yjCode + _picStr[i]).Trim() + ".jpg";
                    }
                    else
                    {
                        keyImage = (defaultPath + yjCode + _picStr[i]).Trim() + ".jpg";
                    }

                    // check image
                    var checkExistImage = _amazonS3Service.ObjectExistsAsync(keyImage);
                    if (checkExistImage != null && checkExistImage.Result)
                    {
                        listPic.Add(pathServerS3 + "/" + keyImage);
                    }
                }
            }

            if (!String.IsNullOrEmpty(yjCode))
            {
                var keyImageCus = "";
                if (defaultPath.Contains(pathServerS3 + "/"))
                {
                    keyImageCus = (customPath.Replace(pathServerS3 + "/", "") + yjCode).Trim() + "Z.jpg";
                }
                else
                {
                    keyImageCus = (customPath + yjCode).Trim() + "Z.jpg";
                }
                // check image
                var checkExistImageCus = _amazonS3Service.ObjectExistsAsync(keyImageCus);
                if (checkExistImageCus != null && checkExistImageCus.Result)
                {
                    listPic.Add(keyImageCus);
                }
            }

            if (listPic.Count > 0)
            {
                // Image default 
                defaultImgPic = listPic[0] ?? string.Empty;
            }
            else
            {
                //Image default Empty
                defaultImgPic = _configuration["DefaultImageDrugEmpty"];
            }

            return defaultImgPic;
        }
    }
}
