using Domain.Models.DrugDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.DrugDetail;

namespace Interactor.DrugDetail
{
    public class GetDrugDetailInteractor : IGetDrugDetailInputPort
    {
        private readonly IDrugDetailRepository _drugInforRepository;
        public GetDrugDetailInteractor(IDrugDetailRepository drugInforRepository)
        {
            _drugInforRepository = drugInforRepository;
        }

        public GetDrugDetailOutputData Handle(GetDrugDetailInputData inputData)
        {

            if (inputData.HpId <= 0)
            {
                return new GetDrugDetailOutputData(new List<DrugMenuItemModel>(), GetDrugDetailStatus.InValidHpId);
            }

            if (inputData.SinDate <= 0)
            {
                return new GetDrugDetailOutputData(new List<DrugMenuItemModel>(), GetDrugDetailStatus.InValidSindate);
            }

            if (String.IsNullOrEmpty(inputData.ItemCd))
            {
                return new GetDrugDetailOutputData(new List<DrugMenuItemModel>(), GetDrugDetailStatus.InValidItemCd);
            }

            var data = _drugInforRepository.GetDrugMenu(inputData.HpId, inputData.SinDate, inputData.ItemCd);

            return new GetDrugDetailOutputData(data.ToList(), GetDrugDetailStatus.Successed);
        }
    }
}
