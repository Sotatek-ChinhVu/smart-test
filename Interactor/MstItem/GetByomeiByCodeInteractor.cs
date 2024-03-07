using Domain.Models.MstItem;
using UseCase.MstItem.GetByomeiByCode;

namespace Interactor.MstItem
{
    public class GetByomeiByCodeInteractor : IGetByomeiByCodeInputPort
    {
        private readonly IMstItemRepository _inputItemRepository;

        public GetByomeiByCodeInteractor(IMstItemRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }

        public GetByomeiByCodeOutputData Handle(GetByomeiByCodeInputData inputData)
        {
            try
            {
                if (string.IsNullOrEmpty(inputData.ByomeiCd))
                {
                    return new GetByomeiByCodeOutputData(new(), GetByomeiByCodeStatus.InvalidItemCd);
                }
                var data = _inputItemRepository.GetByomeiByCode(inputData.HpId, inputData.ByomeiCd);

                var item = new ByomeiMstItem(data?.Byomei ?? string.Empty);

                return new GetByomeiByCodeOutputData(item, GetByomeiByCodeStatus.Successed);
            }
            finally
            {
                _inputItemRepository.ReleaseResource();
            }
        }
    }
}
