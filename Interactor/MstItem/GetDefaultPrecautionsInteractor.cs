using Domain.Models.MstItem;
using UseCase.MstItem.GetDefaultPrecautions;

namespace Interactor.MstItem
{
    public class GetDefaultPrecautionsInteractor : IGetDefaultPrecautionsInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetDefaultPrecautionsInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetDefaultPrecautionsOutputData Handle(GetDefaultPrecautionsInputData inputData)
        {
            try
            {
                var result = _mstItemRepository.GetPrecautions(inputData.HpId, inputData.YjCd);

                return new GetDefaultPrecautionsOutputData(result, GetDefaultPrecautionsStatus.Successed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
