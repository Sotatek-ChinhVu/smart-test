using Domain.Models.MstItem;
using UseCase.MstItem.GetTenOfKNItem;

namespace Interactor.MstItem
{
    public class GetTenOfKNItemInteractor : IGetTenOfKNItemInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public GetTenOfKNItemInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public GetTenOfKNItemOutputData Handle(GetTenOfKNItemInputData inputData)
        {
            try
            {
                var result = _mstItemRepository.GetTenOfKNItem(inputData.HpId, inputData.ItemCd);

                return new GetTenOfKNItemOutputData(result, GetTenOfKNItemStatus.Success);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
