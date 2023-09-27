using Domain.Models.MstItem;
using UseCase.MstItem.IsUsingKensa;

namespace Interactor.MstItem
{
    public class IsUsingKensaInteractor : IIsUsingKensaInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public IsUsingKensaInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public IsUsingKensaOutputData Handle(IsUsingKensaInputData inputData)
        {
            try
            {
                var result = _mstItemRepository.IsUsingKensa(inputData.HpId, inputData.KensaItemCd, inputData.ItemCds);
                if (result)
                {
                    return new IsUsingKensaOutputData(IsUsingKensaStatus.Success);
                }
                else
                {
                    return new IsUsingKensaOutputData(IsUsingKensaStatus.Failed);
                }
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
