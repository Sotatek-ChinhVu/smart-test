using Domain.Models.MstItem;
using UseCase.MstItem.GetAdoptedItemList;

namespace Interactor.MstItem
{
    public class GetAdoptedItemListInteractor : IGetAdoptedItemListInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public GetAdoptedItemListInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetAdoptedItemListOutputData Handle(GetAdoptedItemListInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new GetAdoptedItemListOutputData(new(), GetAdoptedItemListStatus.InValidHpId);
            }

            if (inputData.SinDate <= 0)
            {
                return new GetAdoptedItemListOutputData(new(), GetAdoptedItemListStatus.InvalidSindate);
            }

            if (inputData.ItemCds.Count == 0)
            {
                return new GetAdoptedItemListOutputData(new(), GetAdoptedItemListStatus.InvalidItemCds);
            }

            try
            {
                var datas = _mstItemRepository.GetAdoptedItems(inputData.ItemCds, inputData.SinDate, inputData.HpId);

                return new GetAdoptedItemListOutputData(datas.Select(d => new TenMstItem(d)).ToList(), GetAdoptedItemListStatus.Successed);
            }
            catch
            {
                return new GetAdoptedItemListOutputData(new(), GetAdoptedItemListStatus.Failed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
