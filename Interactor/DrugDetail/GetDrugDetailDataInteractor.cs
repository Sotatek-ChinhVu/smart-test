using Domain.Models.DrugDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.DrugDetailData;

namespace Interactor.DrugDetailData
{
    public class GetDrugDetailDataInteractor : IGetDrugDetailDataInputPort
    {
        private readonly IDrugDetailRepository _drugInforRepository;
        public GetDrugDetailDataInteractor(IDrugDetailRepository drugInforRepository)
        {
            _drugInforRepository = drugInforRepository;
        }

        public GetDrugDetailDataOutputData Handle(GetDrugDetailDataInputData inputData)
        {
            if(string.IsNullOrEmpty(inputData.ItemCd))
            {
                return new GetDrugDetailDataOutputData(new DrugDetailModel(), GetDrugDetailDataStatus.InvalidItemCd);
            }    

            if(string.IsNullOrEmpty(inputData.YJCode))
            {
                return new GetDrugDetailDataOutputData(new DrugDetailModel(), GetDrugDetailDataStatus.InvalidYJCode);
            }    

            var data = _drugInforRepository.GetDataDrugSeletedTree(inputData.SelectedIndexOfChildren, inputData.SelectedIndexOfLevel0, inputData.DrugName, inputData.ItemCd, inputData.YJCode);

            return new GetDrugDetailDataOutputData(data, GetDrugDetailDataStatus.Successed);
        }
    }
}
