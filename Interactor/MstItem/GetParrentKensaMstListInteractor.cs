using Domain.Models.KensaIrai;
using Domain.Models.MstItem;
using UseCase.MstItem.GetParrentKensaMst;

namespace Interactor.MstItem
{
    public class GetParrentKensaMstListInteractor : IGetParrentKensaMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetParrentKensaMstListInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public GetParrentKensaMstOutputData Handle(GetParrentKensaMstInputData inputData)
        {
            try
            {
                var data = _mstItemRepository.GetParrentKensaMstModels(inputData.HpId, inputData.KeyWord, inputData.ItemCd);
                if (data.Count == 0)
                {
                    return new GetParrentKensaMstOutputData(new(), GetParrentKensaMstStatus.NoData);
                }
                else
                {
                    return new GetParrentKensaMstOutputData(data, GetParrentKensaMstStatus.Successful);
                }
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
