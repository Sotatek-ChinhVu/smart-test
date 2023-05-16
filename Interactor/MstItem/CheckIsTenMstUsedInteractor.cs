using Domain.Enum;
using Domain.Models.MstItem;
using UseCase.MstItem.CheckIsTenMstUsed;

namespace Interactor.MstItem
{
    public class CheckIsTenMstUsedInteractor : ICheckIsTenMstUsedInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public CheckIsTenMstUsedInteractor(IMstItemRepository tenMstMaintenanceRepository)
        {
            _mstItemRepository = tenMstMaintenanceRepository;
        }

        public CheckIsTenMstUsedOutputData Handle(CheckIsTenMstUsedInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new CheckIsTenMstUsedOutputData(CheckIsTenMstUsedStatus.InvalidHpId);
                
                if (string.IsNullOrEmpty(inputData.ItemCd))
                    return new CheckIsTenMstUsedOutputData(CheckIsTenMstUsedStatus.InvalidItemCd);

                bool isUsed = _mstItemRepository.IsTenMstUsed(inputData.HpId, inputData.ItemCd, inputData.StartDate, inputData.EndDate);

                if (isUsed)
                    return new CheckIsTenMstUsedOutputData(CheckIsTenMstUsedStatus.IsUsed);
                else
                    return new CheckIsTenMstUsedOutputData(CheckIsTenMstUsedStatus.IsNotUsed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
