using Domain.Models.MstItem;
using UseCase.MstItem.CheckJihiSbtExistsInTenMst;

namespace Interactor.MstItem
{
    public class CheckJihiSbtExistsInTenMstInteractor : ICheckJihiSbtExistsInTenMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public CheckJihiSbtExistsInTenMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public CheckJihiSbtExistsInTenMstOutputData Handle(CheckJihiSbtExistsInTenMstInputData inputData)
        {
            try
            {
                bool isExitst = _mstItemRepository.CheckJihiSbtExistsInTenMst(inputData.JihiSbt);

                if (isExitst)
                    return new CheckJihiSbtExistsInTenMstOutputData(true);
                else
                    return new CheckJihiSbtExistsInTenMstOutputData(false);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
