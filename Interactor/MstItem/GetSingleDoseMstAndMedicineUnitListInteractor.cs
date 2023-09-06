﻿using Domain.Models.MstItem;
using Helper.Common;
using UseCase.MstItem.GetSingleDoseMstAndMedicineUnitList;

namespace Interactor.MstItem
{
    public class GetSingleDoseMstAndMedicineUnitListInteractor : IGetSingleDoseMstAndMedicineUnitListInputPort
    {
        private readonly IMstItemRepository _inputItemRepository;

        public GetSingleDoseMstAndMedicineUnitListInteractor(IMstItemRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }

        public GetSingleDoseMstAndMedicineUnitListOutputData Handle(GetSingleDoseMstAndMedicineUnitListInputData inputData)
        {
            try
            {
                if (inputData.HpId < 1)
                {
                    return new GetSingleDoseMstAndMedicineUnitListOutputData(new List<SingleDoseMstModel>(), new List<MedicineUnitModel>());
                }
                int today = CIUtil.DateTimeToInt(DateTime.Now);
                var listDataSingleDose = _inputItemRepository.GetListSingleDoseModel(inputData.HpId);
                var listDataMedicineUnitModel = _inputItemRepository.GetListMedicineUnitModel(inputData.HpId, today);
                return new GetSingleDoseMstAndMedicineUnitListOutputData(listDataSingleDose, listDataMedicineUnitModel);
            }
            finally
            {
                _inputItemRepository.ReleaseResource();
            }
        }
    }
}
