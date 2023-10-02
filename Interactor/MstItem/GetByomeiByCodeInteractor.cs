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
                    return new GetByomeiByCodeOutputData(null, GetByomeiByCodeStatus.InvalidItemCd);
                }
                var data = _inputItemRepository.GetByomeiByCode(inputData.ByomeiCd);
                if(data == null)
                {
                    return new GetByomeiByCodeOutputData(null, GetByomeiByCodeStatus.NoData);
                }
                var item = new ByomeiMstItem(data?.Byomei);
                return new GetByomeiByCodeOutputData(item, GetByomeiByCodeStatus.Successed);
            }
            finally
            {
                _inputItemRepository.ReleaseResource();
            }
        }
    }
}
