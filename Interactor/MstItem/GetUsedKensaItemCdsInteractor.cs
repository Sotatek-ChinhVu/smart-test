using Domain.Models.MstItem;
using UseCase.MstItem.GetUsedKensaItemCds;

namespace Interactor.MstItem
{
    public class GetUsedKensaItemCdsInteractor : IGetUsedKensaItemCdsInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetUsedKensaItemCdsInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public GetUsedKensaItemCdsOutputData Handle(GetUsedKensaItemCdsInputData inputData)
        {
            try
            {
                var data = _mstItemRepository.GetUsedKensaItemCds(inputData.HpId);
                if (data.Count == 0)
                {
                    return new GetUsedKensaItemCdsOutputData(new(), GetUsedKensaItemCdsStatus.NoData);
                }
                else
                {
                    return new GetUsedKensaItemCdsOutputData(data, GetUsedKensaItemCdsStatus.Successful);
                }
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
