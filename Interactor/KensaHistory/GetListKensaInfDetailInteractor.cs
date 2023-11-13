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
                return new GetListKensaInfDetailOutputData(new ListKensaInfDetailModel(), SearchPostCodeStatus.InvalidHpId);
            }

            if (inputData.PtId <= 0)
            {
                return new GetListKensaInfDetailOutputData(new ListKensaInfDetailModel(), SearchPostCodeStatus.InvalidPtId);
            }

            try
            {
                var result = _kensaSetRepository.GetListKensaInfDetail(inputData.HpId, inputData.UserId, inputData.PtId, inputData.SetId, inputData.IraiCd, inputData.IraiCdStart, inputData.GetGetPrevious, inputData.ShowAbnormalKbn, inputData.ItemQuantity, inputData.ListSeqNoItems, inputData.StartDate, inputData.EndDate);
                if (result == null)
                {
                    return new GetListKensaInfDetailOutputData(new ListKensaInfDetailModel(), SearchPostCodeStatus.NoData);
                }

                return new GetListKensaInfDetailOutputData(result, SearchPostCodeStatus.Success);
            }
            finally
            {
                _kensaSetRepository.ReleaseResource();
            }
        }
    }
}
