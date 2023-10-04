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
                return new GetListKensaMstOuputData(new List<KensaMstModel>(), SearchPostCodeStatus.InvalidHpId, 0);
            }
            try
            {
                var result = _mstItemRepository.GetListKensaMst(inputData.HpId, inputData.Keyword, inputData.PageIndex, inputData.PageSize);
                if (result.Item2 == 0)
                {
                    return new GetListKensaMstOuputData(new List<KensaMstModel>(), SearchPostCodeStatus.NoData, 0);
                }

                return new GetListKensaMstOuputData(result.Item1, SearchPostCodeStatus.Success, result.Item2);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
