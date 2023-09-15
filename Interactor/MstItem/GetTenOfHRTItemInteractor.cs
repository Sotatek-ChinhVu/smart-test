using Domain.Models.MstItem;
using UseCase.MstItem.GetTenOfHRTItem;

namespace Interactor.MstItem
{
    public class GetTenOfHRTItemInteractor : IGetTenOfHRTItemInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetTenOfHRTItemInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public GetTenOfHRTItemOutputData Handle(GetTenOfHRTItemInputData inputData)
        {
            try
            {
                var data = _mstItemRepository.GetTenOfHRTItem(inputData.HpId);

                return new GetTenOfHRTItemOutputData(data, GetTenOfHRTItemStatus.Success);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
