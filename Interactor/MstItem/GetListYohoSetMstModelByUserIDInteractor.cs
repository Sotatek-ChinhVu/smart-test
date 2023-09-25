using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using UseCase.MstItem.GetListYohoSetMstModelByUserID;

namespace Interactor.MstItem
{
    public class GetListYohoSetMstModelByUserIDInteractor : IGetListYohoSetMstModelByUserIDInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetListYohoSetMstModelByUserIDInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetListYohoSetMstModelByUserIDOutputData Handle(GetListYohoSetMstModelByUserIDInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetListYohoSetMstModelByUserIDOutputData(new List<YohoSetMstModel>(), GetListYohoSetMstModelByUserIDStatus.InvalidHpId);

                var data = _mstItemRepository.GetListYohoSetMstModelByUserID(inputData.HpId, inputData.UserIdLogin, inputData.SinDate, inputData.UserId);

                if (!data.Any())
                    return new GetListYohoSetMstModelByUserIDOutputData(new List<YohoSetMstModel>(), GetListYohoSetMstModelByUserIDStatus.NoData);
                else
                    return new GetListYohoSetMstModelByUserIDOutputData(data, GetListYohoSetMstModelByUserIDStatus.Successful);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
