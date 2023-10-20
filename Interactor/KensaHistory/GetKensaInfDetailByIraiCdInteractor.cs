using Domain.Models.KensaIrai;
using Domain.Models.KensaSet;
using UseCase.KensaHistory.GetListKensaCmtMst.GetKensaInfDetailByIraiCd;
using UseCase.MstItem.SearchPostCode;

namespace Interactor.KensaHistory
{
    public class GetKensaInfDetailByIraiCdInteractor : IGetKensaInfDetailByIraiCdInputPort
    {
        private readonly IKensaSetRepository _kensaSetRepository;

        public GetKensaInfDetailByIraiCdInteractor(IKensaSetRepository kensaSetRepository)
        {
            _kensaSetRepository = kensaSetRepository;
        }
        public GetKensaInfDetailByIraiCdOutputData Handle(GetKensaInfDetailByIraiCdInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new GetKensaInfDetailByIraiCdOutputData(new List<ListKensaInfDetailItemModel>(), SearchPostCodeStatus.InvalidHpId);
            }

            try
            {
                var result = _kensaSetRepository.GetKensaInfDetailByIraiCd(inputData.HpId, inputData.IraiCd);
                if (result == null)
                {
                    return new GetKensaInfDetailByIraiCdOutputData(new List<ListKensaInfDetailItemModel>(), SearchPostCodeStatus.NoData);
                }

                return new GetKensaInfDetailByIraiCdOutputData(result, SearchPostCodeStatus.Success);
            }
            finally
            {
                _kensaSetRepository.ReleaseResource();
            }
        }
    }
}
