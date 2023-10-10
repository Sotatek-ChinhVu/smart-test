using Domain.Models.KensaIrai;
using Domain.Models.MstItem;
using UseCase.MstItem.GetListResultKensaMst;
using UseCase.MstItem.SearchPostCode;
using UseCase.MstItem.SearchTenMstItem;

namespace Interactor.MstItem
{
    public class GetListKensaMstInteractor : IGetListKensaMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetListKensaMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public GetListKensaMstOuputData Handle(GetListKensaMstInputData inputData)
        {

            if (inputData.HpId < 0)
            {
                return new GetListKensaMstOuputData(new List<KensaMstModel>(), SearchTenMstItemStatus.InValidHpId);
            }
            try
            {
                var result = new List<KensaMstModel>();
                result = _mstItemRepository.GetListKensaMst(inputData.HpId, inputData.Keyword);
                return new GetListKensaMstOuputData(result, SearchTenMstItemStatus.Successed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}