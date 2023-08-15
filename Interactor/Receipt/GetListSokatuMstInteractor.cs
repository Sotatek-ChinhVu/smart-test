using Domain.Models.Receipt;
using UseCase.Receipt.GetListSokatuMst;

namespace Interactor.Receipt
{
    public class GetListSokatuMstInteractor : IGetListSokatuMstInputPort
    {
        private readonly IReceiptRepository _receiptRepository;

        public GetListSokatuMstInteractor(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }
        public GetListSokatuMstOutputData Handle(GetListSokatuMstInputData input)
        {
            try
            {
                if (input.SeikyuYm <= 0)
                {
                    return new GetListSokatuMstOutputData(GetListSokatuMstStatus.InvalidSeikyuYm, new());
                }

                var SokatuMsts = _receiptRepository.GetSokatuMstModels(input.HpId, input.SeikyuYm);
                return new GetListSokatuMstOutputData(GetListSokatuMstStatus.Success, SokatuMsts);
            }
            finally
            {
                _receiptRepository.ReleaseResource();
            }
        }

    }
}
