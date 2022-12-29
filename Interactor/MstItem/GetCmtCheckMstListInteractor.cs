using Domain.Models.MstItem;
using UseCase.MstItem.GetCmtCheckMstList;

namespace Interactor.MstItem
{
    public class GetCmtCheckMstListInteractor : IGetCmtCheckMstListInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public GetCmtCheckMstListInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetCmtCheckMstListOutputData Handle(GetCmtCheckMstListInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new GetCmtCheckMstListOutputData(new(), GetCmtCheckMstListStatus.InValidHpId);
            }

            if (inputData.UserId <= 0)
            {
                return new GetCmtCheckMstListOutputData(new(), GetCmtCheckMstListStatus.InValidUserId);
            }

            if (inputData.ItemCds.Count == 0)
            {
                return new GetCmtCheckMstListOutputData(new(), GetCmtCheckMstListStatus.InvalidItemCd);
            }

            try
            {
                var data = _mstItemRepository.GetCmtCheckMsts(inputData.HpId, inputData.UserId, inputData.ItemCds);

                return new GetCmtCheckMstListOutputData(data, GetCmtCheckMstListStatus.Successed);
            }
            catch
            {
                return new GetCmtCheckMstListOutputData(new(), GetCmtCheckMstListStatus.Failed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
