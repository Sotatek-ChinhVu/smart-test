using Domain.Enum;
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
                    return new DeleteOrRecoverTenMstOutputData(DeleteOrRecoverTenMstStatus.InvalidItemCd, string.Empty);

                if (inputData.UserId < 0)
                    return new DeleteOrRecoverTenMstOutputData(DeleteOrRecoverTenMstStatus.InvalidUserId, string.Empty);

                if (inputData.Mode == DeleteOrRecoverTenMstMode.Delete && !inputData.ConfirmDeleteIfModeIsDeleted)
                {
                    if (_mstItemRepository.IsTenMstItemCdUsed(inputData.HpId, inputData.ItemCd))
                    {
                        string msg = string.Format("'{0}'は、既に使用されている項目です。", inputData.SelectedTenMstModelName) + Environment.NewLine + "削除しますか？";
                        return new DeleteOrRecoverTenMstOutputData(DeleteOrRecoverTenMstStatus.RequiredConfirmDelete, msg);
                    }
                }

                bool result = _mstItemRepository.SaveDeleteOrRecoverTenMstOrigin(inputData.HpId, inputData.Mode, inputData.ItemCd, inputData.UserId, inputData.TenMsts);

                if (result)
                    return new DeleteOrRecoverTenMstOutputData(DeleteOrRecoverTenMstStatus.Successful, string.Empty);
                else
                    return new DeleteOrRecoverTenMstOutputData(DeleteOrRecoverTenMstStatus.Failed, string.Empty);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
