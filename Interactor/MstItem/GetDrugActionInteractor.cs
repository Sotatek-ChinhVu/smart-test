using Domain.Models.MstItem;
using UseCase.MstItem.GetDrugAction;

namespace Interactor.MstItem
{
    public class GetDrugActionInteractor : IGetDrugActionInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetDrugActionInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetDrugActionOutputData Handle(GetDrugActionInputData inputData)
        {
            try
            {
                var result = _mstItemRepository.GetDrugAction(inputData.HpId, inputData.YjCd);

                return new GetDrugActionOutputData(result, GetDrugActionStatus.Successed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
