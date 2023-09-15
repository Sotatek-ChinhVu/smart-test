using Domain.Models.MstItem;
using UseCase.MstItem.GetTenOfIGEItem;

namespace Interactor.MstItem
{
    public class GetTenOfIGEItemInteractor : IGetTenOfIGEItemInputData
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetTenOfIGEItemInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public GetTenOfIGEItemOutputData Handle(GetTenOfIGEItemInputData inputData)
        {
            try
            {
                var data = _mstItemRepository.GetTenOfIGEItem(inputData.HpId);

                return new GetTenOfIGEItemOutputData(data, GetTenOfIGEItemStatus.Success);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
