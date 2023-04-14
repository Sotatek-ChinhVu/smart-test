using Domain.Models.MstItem;
using UseCase.MstItem.DeleteOrRecoverTenMst;

namespace Interactor.MstItem
{
    public class DeleteOrRecoverTenMstInteractor : DeleteOrRecoverTenMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public DeleteOrRecoverTenMstInteractor(IMstItemRepository tenMstMaintenanceRepository)
        {
            _mstItemRepository = tenMstMaintenanceRepository;
        }

        public DeleteOrRecoverTenMstOutputData Handle(DeleteOrRecoverTenMstInputData inputData)
        {
            try
            {
                if (string.IsNullOrEmpty(inputData.ItemCd))
                    return new DeleteOrRecoverTenMstOutputData(DeleteOrRecoverTenMstStatus.InvalidItemCd);

                if (inputData.UserId < 0)
                    return new DeleteOrRecoverTenMstOutputData(DeleteOrRecoverTenMstStatus.InvalidUserId);

                bool result = _mstItemRepository.SaveDeleteOrRecoverTenMstOrigin(inputData.Mode ,inputData.ItemCd, inputData.UserId, inputData.TenMsts);

                if (result)
                    return new DeleteOrRecoverTenMstOutputData(DeleteOrRecoverTenMstStatus.Successful);
                else
                    return new DeleteOrRecoverTenMstOutputData(DeleteOrRecoverTenMstStatus.Failed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
