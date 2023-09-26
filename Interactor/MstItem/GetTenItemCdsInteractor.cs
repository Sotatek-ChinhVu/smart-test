using Domain.Models.MstItem;
using UseCase.MstItem.GetTenItemCds;

namespace Interactor.MstItem
{
    public class GetTenItemCdsInteractor : IGetTenItemCdsInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetTenItemCdsInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public GetTenItemCdsOutputData Handle(GetTenItemCdsInputData inputData)
        {
            try
            {
                var data = _mstItemRepository.GetTenItemCds(inputData.HpId);
                if (data.Count == 0)
                {
                    return new GetTenItemCdsOutputData(new(), GetTenItemCdsStatus.NoData);
                }
                else
                {
                    return new GetTenItemCdsOutputData(data, GetTenItemCdsStatus.Success);
                }
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
