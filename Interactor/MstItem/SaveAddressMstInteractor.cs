using Domain.Models.MstItem;
using UseCase.MstItem.SaveAddressMst;

namespace Interactor.MstItem
{
    public class SaveAddressMstInteractor : ISaveAddressMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public SaveAddressMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public SaveAddressMstOutputData Handle(SaveAddressMstInputData inputData)
        {
            try
            {
                bool result = _mstItemRepository.SaveAddressMaster(inputData.PostCodeMsts, inputData.HpId, inputData.UserId);

                return new SaveAddressMstOutputData(result ? SaveAddressMstStatus.Success : SaveAddressMstStatus.Failed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
