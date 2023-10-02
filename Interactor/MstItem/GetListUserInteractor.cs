using Domain.Models.MstItem;
using Domain.Models.User;
using UseCase.MstItem.GetListUser;

namespace Interactor.MstItem
{
    public class GetListUserInteractor : IGetListUserInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetListUserInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetListUserOutputData Handle(GetListUserInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetListUserOutputData(new List<UserMstModel>(), GetListUserStatus.InvalidHpId);
                var data = _mstItemRepository.GetListUser(inputData.HpId, inputData.UserId, inputData.SinDate);

                if (!data.Any())
                    return new GetListUserOutputData(new List<UserMstModel>(), GetListUserStatus.NoData);
                else
                    return new GetListUserOutputData(data, GetListUserStatus.Successed);
            }
            catch (Exception e)
            {
                var x = e.Message;
                return new GetListUserOutputData(new List<UserMstModel>(), GetListUserStatus.NoData);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
