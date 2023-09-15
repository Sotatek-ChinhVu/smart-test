using Domain.Models.MstItem;
using UseCase.MstItem.GetKensaStdMst;
using UseCase.MstItem.GetParrentKensaMst;

namespace Interactor.MstItem
{
    public class GetKensaStdMstModelsInteractor : IGetKensaStdMstInputport
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetKensaStdMstModelsInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public GetKensaStdMstOutputData Handle(GetKensaStdMstInputData inputData)
        {
            try
            {
                var data = _mstItemRepository.GetKensaStdMstModels(inputData.HpId, inputData.KensaItemCd);
                if (data.Count == 0)
                {
                    return new GetKensaStdMstOutputData(new(), GetKensaStdMstIStatus.NoData);
                }
                else
                {
                    return new GetKensaStdMstOutputData(data, GetKensaStdMstIStatus.Successful);
                }
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
