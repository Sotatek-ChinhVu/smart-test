using Domain.Models.MstItem;
using UseCase.MstItem.GetTenOfItem;

namespace Interactor.MstItem
{
    public class GetTenOfItemInteractor : IGetTenOfItemInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetTenOfItemInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public GetTenOfItemOutputData Handle(GetTenOfItemInputData inputData)
        {
            try
            {
                var data = _mstItemRepository.GetTenOfItem(inputData.HpId);

                return new GetTenOfItemOutputData(data, GetTenOfItemStatus.Success);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
