using Domain.Models.DrugInfor;
using Helper.Common;
using Helper.Extension;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

                if (!string.IsNullOrEmpty(data.OtherPicZai))
                {
                    data.PathPicZai = _amazonS3Service.GetAccessBaseS3() + data.OtherPicZai;
                }
                else
                {
                    data.PathPicZai = GetPathImagePic(data.YjCode, data.DefaultPathPicZai, data.CustomPathPicZai, listPicZai);
                }

                if (!string.IsNullOrEmpty(data.OtherPicHou))
                {
                    data.PathPicHou = _amazonS3Service.GetAccessBaseS3() + data.OtherPicHou;
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

        private string GetPathImagePic(string yjCode, string filePath, string customPath, List<string> listPic)
        {
            string _picStr = " ABCDEFGHIJZ";
            for (int i = 0; i < _picStr.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(yjCode))
                {
                    string imgFile = (filePath + yjCode + _picStr[i].AsString()).Trim() + ".jpg";

                    // check image
                    var checkExistImage = _amazonS3Service.S3FilePathIsExists(imgFile);
                    if (checkExistImage != null && checkExistImage.Result)
                    {
                        listPic.Add(_amazonS3Service.GetAccessBaseS3() + imgFile);
                    }
                }
            }

            string customImage = customPath + yjCode + "Z.jpg";
            if (_amazonS3Service.S3FilePathIsExists(customImage).Result)
            {
                listPic.Add(_amazonS3Service.GetAccessBaseS3() + customImage);
            }

            if (listPic.Count > 0)
            {
                // Image default 
                return listPic[0] ?? string.Empty;
            }
            else
            {
                //Image default Empty
                return _configuration["DefaultImageDrugEmpty"] ?? string.Empty;
            }
        }
    }
}
