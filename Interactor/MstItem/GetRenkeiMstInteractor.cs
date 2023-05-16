using Domain.Models.MstItem;
using Helper.Extension;
using UseCase.MstItem.GetRenkeiMst;

namespace Interactor.MstItem
{
    public class GetRenkeiMstInteractor : IGetRenkeiMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetRenkeiMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetRenkeiMstOutputData Handle(GetRenkeiMstInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetRenkeiMstOutputData(GetRenkeiMstStatus.InvalidHpId, ObjectExtension.CreateInstance<RenkeiMstModel>());

                if (inputData.RenkeiId <= 0)
                    return new GetRenkeiMstOutputData(GetRenkeiMstStatus.InvalidRenkeiId, ObjectExtension.CreateInstance<RenkeiMstModel>());

                var renkeiMst = _mstItemRepository.GetRenkeiMst(inputData.HpId, inputData.RenkeiId);
                if (renkeiMst is null)
                    return new GetRenkeiMstOutputData(GetRenkeiMstStatus.NoData, ObjectExtension.CreateInstance<RenkeiMstModel>());
                else
                    return new GetRenkeiMstOutputData(GetRenkeiMstStatus.Successful, renkeiMst);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
