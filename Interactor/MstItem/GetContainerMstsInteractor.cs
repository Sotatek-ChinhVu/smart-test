using Domain.Models.MstItem;
using UseCase.MstItem.GetContainerMsts;

namespace Interactor.MstItem
{
    public class GetContainerMstsInteractor : IGetContainerMstsInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetContainerMstsInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public GetContainerMstsOutputData Handle(GetContainerMstsInputData inputData)
        {
            try
            {
                var data = _mstItemRepository.GetContainerMsts(inputData.HpId);
                if (data.Count == 0)
                {
                    return new GetContainerMstsOutputData(new(), GetContainerMstsStatus.NoData);
                }
                else
                {
                    return new GetContainerMstsOutputData(data, GetContainerMstsStatus.Success);
                }
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
