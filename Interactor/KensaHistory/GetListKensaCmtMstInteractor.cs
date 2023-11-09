using Domain.Models.KensaCmtMst.cs;
using Domain.Models.KensaSet;
using UseCase.KensaHistory.GetListKensaCmtMst;
using UseCase.MstItem.SearchTenMstItem;

namespace Interactor.KensaHistory
{
    public class GetListKensaCmtMstInteractor : IGetListKensaCmtMstInputPort
    {
        private readonly IKensaSetRepository _kensaSetRepository;
        public GetListKensaCmtMstInteractor(IKensaSetRepository kensaSetRepository)
        {
            _kensaSetRepository = kensaSetRepository;
        }
        public GetListKensaCmtMstOutputData Handle(GetListKensaCmtMstInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new GetListKensaCmtMstOutputData(new List<KensaCmtMstModel>(), SearchTenMstItemStatus.InValidHpId);
            }
            try
            {
                var result = new List<KensaCmtMstModel>();
                result = _kensaSetRepository.GetListKensaCmtMst(inputData.HpId, inputData.IraiCd, inputData.Keyword);
                return new GetListKensaCmtMstOutputData(result, SearchTenMstItemStatus.Successed);
            }
            finally
            {
                _kensaSetRepository.ReleaseResource();
            }
        }
    }
}
