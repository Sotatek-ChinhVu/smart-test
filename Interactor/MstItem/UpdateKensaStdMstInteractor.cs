using Domain.Models.MstItem;
using UseCase.MstItem.UpdateKensaStdMst;

namespace Interactor.MstItem
{
    public class UpdateKensaStdMstInteractor : IUpdateKensaStdMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public UpdateKensaStdMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public UpdateKensaStdMstOutputData Handle(UpdateKensaStdMstInputData inputData)
        {
            try
            {
                var result = _mstItemRepository.UpdateKensaStdMst(inputData.HpId, inputData.UserId, inputData.KensaStdMsts);
                if (result)
                {
                    return new UpdateKensaStdMstOutputData(UpdateKensaStdMstStatus.Success);
                }
                else
                {
                    return new UpdateKensaStdMstOutputData(UpdateKensaStdMstStatus.Failed);
                }
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
