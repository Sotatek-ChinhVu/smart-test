using Domain.Models.MstItem;
using UseCase.MstItem.UpdateAdoptedItemList;

namespace Interactor.MstItem
{
    public class UpdateAdoptedItemListInteractor : IUpdateAdoptedItemListInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public UpdateAdoptedItemListInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public UpdateAdoptedItemListOutputData Handle(UpdateAdoptedItemListInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new UpdateAdoptedItemListOutputData(UpdateAdoptedItemListStatus.InValidHpId);
            }

            if (inputData.SinDate <= 0)
            {
                return new UpdateAdoptedItemListOutputData(UpdateAdoptedItemListStatus.InvalidSindate);
            }

            if (inputData.ItemCds.Count == 0)
            {
                return new UpdateAdoptedItemListOutputData(UpdateAdoptedItemListStatus.InvalidItemCds);
            }

            if (inputData.UserId <= 0)
            {
                return new UpdateAdoptedItemListOutputData(UpdateAdoptedItemListStatus.InvalidUserId);
            }

            if (inputData.ValueAdopted > 1 || inputData.ValueAdopted < 0)
            {
                return new UpdateAdoptedItemListOutputData(UpdateAdoptedItemListStatus.InvalidValueAdopted);
            }


            try
            {
                var data = _mstItemRepository.UpdateAdoptedItems(inputData.ValueAdopted, inputData.ItemCds, inputData.SinDate, inputData.HpId, inputData.UserId);

                return new UpdateAdoptedItemListOutputData(UpdateAdoptedItemListStatus.Successed);
            }
            catch
            {
                return new UpdateAdoptedItemListOutputData(UpdateAdoptedItemListStatus.Failed);
            }
        }
    }
}
