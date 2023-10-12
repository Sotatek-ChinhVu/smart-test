using Domain.Models.KensaSet;
using UseCase.KensaHistory.GetListKensaSet;
using UseCase.MstItem.SearchTenMstItem;

namespace Interactor.KensaHistory
{
    public class GetListKensaSetInteractor : IGetListKensaSetInputPort
    {
        private readonly IKensaSetRepository _kensaSetRepository;

        public GetListKensaSetInteractor(IKensaSetRepository kensaSetRepository)
        {
            _kensaSetRepository = kensaSetRepository;
        }

        public GetListKensaSetOutputData Handle(GetListKensaSetInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new GetListKensaSetOutputData(new List<KensaSetModel>(), SearchTenMstItemStatus.InValidHpId);
            }
            try
            {
                var result = new List<KensaSetModel>();
                result = _kensaSetRepository.GetListKensaSet(inputData.HpId);
                return new GetListKensaSetOutputData(result, SearchTenMstItemStatus.Successed);
            }
            finally
            {
                _kensaSetRepository.ReleaseResource();
            }
        }
    }
}
