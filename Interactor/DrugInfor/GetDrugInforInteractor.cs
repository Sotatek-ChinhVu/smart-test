using Domain.Models.DrugInfor;
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
        public GetDrugInforInteractor(IDrugInforRepository drugInforRepository)
        {
            _drugInforRepository = drugInforRepository;
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

            return new GetDrugInforOutputData(data, GetDrugInforStatus.Successed);
        }
    }
}
