using Domain.Models.MstItem;
using UseCase.MstItem.UpdateCmtCheckMst;
using static UseCase.MstItem.UpdateCmtCheckMst.UpdateCmtCheckMstOutputData;

namespace Interactor.MstItem
{
    public class UpdateCmtCheckMstInteractor : IUpdateCmtCheckMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public UpdateCmtCheckMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public UpdateCmtCheckMstOutputData Handle(UpdateCmtCheckMstInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new UpdateCmtCheckMstOutputData(false, UpdateCmtCheckMstStatus.InValidHpId);
            }

            if (inputData.UserId < 0)
            {
                return new UpdateCmtCheckMstOutputData(false, UpdateCmtCheckMstStatus.InValidUserId);
            }

            if (inputData.ListItemCmt.Count <= 0)
            {
                return new UpdateCmtCheckMstOutputData(false, UpdateCmtCheckMstStatus.InvalidDataUpdate);
            }
            try
            {
                var data = _mstItemRepository.UpdateCmtCheckMst(inputData.UserId, inputData.HpId, inputData.ListItemCmt);
                return new UpdateCmtCheckMstOutputData(data, UpdateCmtCheckMstStatus.Successed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }

        }
    }
}
