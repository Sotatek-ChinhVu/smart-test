using Domain.Models.MstItem;
using UseCase.MstItem.GetKensaCenterMsts;

namespace Interactor.MstItem
{
    public class GetKensaCenterMstsInteractor : IGetKensaCenterMstsInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetKensaCenterMstsInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public GetKensaCenterMstsOutputData Handle(GetKensaCenterMstsInputData inputData)
        {
            try
            {
                var data = _mstItemRepository.GetKensaCenterMsts(inputData.HpId);
                if (data.Count == 0)
                {
                    return new GetKensaCenterMstsOutputData(new(), GetKensaCenterMstsStatus.NoData);
                }
                else
                {
                    return new GetKensaCenterMstsOutputData(data, GetKensaCenterMstsStatus.Success);
                }
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
