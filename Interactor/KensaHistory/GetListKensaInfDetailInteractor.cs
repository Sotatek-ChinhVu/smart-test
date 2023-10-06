using Domain.Models.KensaIrai;
using Domain.Models.KensaSet;
using UseCase.KensaHistory.GetListKensaInfDetail;
using UseCase.MstItem.SearchPostCode;

namespace Interactor.KensaHistory
{
    public class GetListKensaInfDetailInteractor : IGetListKensaInfDetailInputPort
    {
        private readonly IKensaSetRepository _kensaSetRepository;

        public GetListKensaInfDetailInteractor(IKensaSetRepository kensaSetRepository)
        {
            _kensaSetRepository = kensaSetRepository;
        }
        public GetListKensaInfDetailOutputData Handle(GetListKensaInfDetailInputData inputData)
        {

            if (inputData.HpId < 0)
            {
                return new GetListKensaInfDetailOutputData(new List<ListKensaInfDetailModel>(), SearchPostCodeStatus.InvalidHpId, 0);
            }
            try
            {
                var result = _kensaSetRepository.GetListKensaInfDetail(inputData.HpId, inputData.UserId, inputData.PtId, inputData.SetId, inputData.PageIndex, inputData.PageSize);
                if (result.Item2 == 0)
                {
                    return new GetListKensaInfDetailOutputData(new List<ListKensaInfDetailModel>(), SearchPostCodeStatus.NoData, 0);
                }

                return new GetListKensaInfDetailOutputData(result.Item1, SearchPostCodeStatus.Success, result.Item2);
            }
            finally
            {
                _kensaSetRepository.ReleaseResource();
            }
        }
    }
}
