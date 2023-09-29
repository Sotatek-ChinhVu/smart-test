using Domain.Models.MstItem;
using UseCase.MstItem.ExistUsedKensaItemCd;

namespace Interactor.MstItem
{
    public class ExistUsedKensaItemCdInteractor : IExistUsedKensaItemCdInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public ExistUsedKensaItemCdInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public ExistUsedKensaItemCdOutputData Handle(ExistUsedKensaItemCdInputData inputData)
        {
            try
            {
                var result = _mstItemRepository.ExistUsedKensaItemCd(inputData.HpId, inputData.KensaItemCd, inputData.KensaSeqNo);
                if (result)
                {
                    return new ExistUsedKensaItemCdOutputData(ExistUsedKensaItemCdStatus.Success);
                }
                else
                {
                    return new ExistUsedKensaItemCdOutputData(ExistUsedKensaItemCdStatus.Failed);
                }
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
