using Domain.Models.InputItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.InputItem.UpdateAdopted;

namespace Interactor.InputItem
{
    public class UpdateAdoptedInputItemInteractor : IUpdateAdoptedInputItemInputPort
    {
        private readonly IInputItemRepository _inputItemRepository;
        public UpdateAdoptedInputItemInteractor(IInputItemRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }

        public UpdateAdoptedInputItemOutputData Handle(UpdateAdoptedInputItemInputData inputData)
        {
            if (inputData.ValueAdopted < 0)
            {
                return new UpdateAdoptedInputItemOutputData(false, UpdateAdoptedInputItemStatus.InvalidValueAdopted);
            }

            if (String.IsNullOrEmpty(inputData.ItemCdInputItem))
            {
                return new UpdateAdoptedInputItemOutputData(false, UpdateAdoptedInputItemStatus.InvalidItemCd);
            }

            if (inputData.SinDateInputItem < 0)
            {
                return new UpdateAdoptedInputItemOutputData(false, UpdateAdoptedInputItemStatus.InvalidSinDate);
            }

            var data = _inputItemRepository.UpdateAdoptedItemAndItemConfig(inputData.ValueAdopted, inputData.ItemCdInputItem, inputData.SinDateInputItem);

            return new UpdateAdoptedInputItemOutputData(data, UpdateAdoptedInputItemStatus.Successed);
        }
    }
}
