using Domain.Models.DrugInfor;
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
        private readonly IConfiguration _configuration;

        public GetDrugInforInteractor(IDrugInforRepository drugInforRepository, IConfiguration configuration)
        {
            _drugInforRepository = drugInforRepository;
            _configuration = configuration;
        }

        public GetDrugInforOutputData Handle(GetDrugInforInputData inputData)
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

            var emptyImageDrug = _configuration["DefaultImageDrugEmpty"];

            if (!String.IsNullOrEmpty(data.OtherPicZai))
            {
                data.PathPicZai = data.OtherPicZai;
            }
            else
            {
                data.PathPicZai = GetPathImagePic(data.YjCode, data.DefaultPathPicZai, data.CustomPathPicZai);
            }
            
            if(!String.IsNullOrEmpty(data.OtherPicHou))
            {
                data.PathPicHou = data.OtherPicHou;
            }  
            else
            {
                data.PathPicHou = GetPathImagePic(data.YjCode, data.DefaultPathPicHou, data.CustomPathPicHou);
            }    

            return new GetDrugInforOutputData(data, GetDrugInforStatus.Successed);
        }

        private string GetPathImagePic(string yjCode, string defaultPath, string customPath)
        {
            return string.Empty;
        }
    }
}
