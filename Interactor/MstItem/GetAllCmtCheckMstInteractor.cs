using Domain.Models.MstItem;
using UseCase.MstItem.GetAllCmtCheckMst;
using UseCase.MstItem.GetCmtCheckMstList;

namespace Interactor.MstItem
{
    public class GetAllCmtCheckMstInteractor : IGetAllCmtCheckMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public GetAllCmtCheckMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetAllCmtCheckMstOutputData Handle(GetAllCmtCheckMstInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new GetAllCmtCheckMstOutputData(new(), GetCmtCheckMstListStatus.InValidHpId);
            }

            try
            {
                var data = _mstItemRepository.GetAllCmtCheckMst(inputData.HpId, inputData.SinDay);

                return new GetAllCmtCheckMstOutputData(data, GetCmtCheckMstListStatus.Successed);
            }
            catch
            {
                return new GetAllCmtCheckMstOutputData(new(), GetCmtCheckMstListStatus.Failed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
