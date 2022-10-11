﻿using Domain.Models.MstItem;
using UseCase.MstItem.UpdateAdopted;

namespace Interactor.MstItem
{
    public class UpdateAdoptedTenItemInteractor : IUpdateAdoptedTenItemInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public UpdateAdoptedTenItemInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public UpdateAdoptedTenItemOutputData Handle(UpdateAdoptedTenItemInputData inputData)
        {
            if (inputData.ValueAdopted < 0)
            {
                return new UpdateAdoptedTenItemOutputData(false, UpdateAdoptedTenItemStatus.InvalidValueAdopted);
            }

            if (String.IsNullOrEmpty(inputData.ItemCdInputItem))
            {
                return new UpdateAdoptedTenItemOutputData(false, UpdateAdoptedTenItemStatus.InvalidItemCd);
            }

            if (inputData.StartDateInputItem < 0)
            {
                return new UpdateAdoptedTenItemOutputData(false, UpdateAdoptedTenItemStatus.InvalidStartDate);
            }

            try
            {
                var data = _mstItemRepository.UpdateAdoptedItemAndItemConfig(inputData.ValueAdopted, inputData.ItemCdInputItem, inputData.StartDateInputItem);
                return new UpdateAdoptedTenItemOutputData(data, UpdateAdoptedTenItemStatus.Successed);
            }
            catch (Exception)
            {
                return new UpdateAdoptedTenItemOutputData(false, UpdateAdoptedTenItemStatus.Failed);
            }
        }
    }
}
