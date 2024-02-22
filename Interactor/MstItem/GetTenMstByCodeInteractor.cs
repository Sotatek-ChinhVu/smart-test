using Domain.Models.MstItem;
using UseCase.MstItem.GetTenMstByCode;

namespace Interactor.MstItem
{
    public class GetTenMstByCodeInteractor : IGetTenMstByCodeInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetTenMstByCodeInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetTenMstByCodeOutputData Handle(GetTenMstByCodeInputData inputData)
        {
            try
            {
                if (string.IsNullOrEmpty(inputData.ItemCd))
                    return new GetTenMstByCodeOutputData(null, GetTenMstByCodeStatus.InvalidItemCd);

                var data = _mstItemRepository.GetTenMstByCode(inputData.HpId, inputData.ItemCd, inputData.SetKbn, inputData.SinDate);

                if (data == null)
                    return new GetTenMstByCodeOutputData(null, GetTenMstByCodeStatus.NoData);
                else
                    return new GetTenMstByCodeOutputData(data, GetTenMstByCodeStatus.Successed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
