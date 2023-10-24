using Domain.Models.MstItem;
using UseCase.MstItem.UpdateYohoSetMst;

namespace Interactor.MstItem
{
    public class UpdateYohoSetMstInteractor : IUpdateYohoSetMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public UpdateYohoSetMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public UpdateYohoSetMstOutputData Handle(UpdateYohoSetMstInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new UpdateYohoSetMstOutputData(false, UpdateYohoSetMstStatus.InvalidHpId);
                }
                var result = _mstItemRepository.UpdateYohoSetMst(inputData.HpId, inputData.UserId, inputData.YohoSetMsts);
                return new UpdateYohoSetMstOutputData(true, UpdateYohoSetMstStatus.Successed);
            }
            catch
            {
                return new UpdateYohoSetMstOutputData(false, UpdateYohoSetMstStatus.Failed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
