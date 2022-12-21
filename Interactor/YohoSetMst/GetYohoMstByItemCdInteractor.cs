using Domain.Models.OrdInfDetails;
using Domain.Models.YohoSetMst;
using UseCase.YohoSetMst.GetByItemCd;

namespace Interactor.YohoSetMst
{
    public class GetYohoMstByItemCdInteractor : IGetYohoMstByItemCdInputPort
    {
        private readonly IYohoSetMstRepository _yohoSetMstRepository;

        public GetYohoMstByItemCdInteractor(IYohoSetMstRepository yohoSetMstRepository)
        {
            _yohoSetMstRepository = yohoSetMstRepository;
        }

        public GetYohoMstByItemCdOutputData Handle(GetYohoMstByItemCdInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                    return new GetYohoMstByItemCdOutputData(new List<YohoSetMstModel>(), GetYohoMstByItemCdStatus.InvalidHpId);

                if (inputData.StartDate < 0)
                    return new GetYohoMstByItemCdOutputData(new List<YohoSetMstModel>(), GetYohoMstByItemCdStatus.InvalidStartDate);

                if (string.IsNullOrEmpty(inputData.ItemCd))
                    return new GetYohoMstByItemCdOutputData(new List<YohoSetMstModel>(), GetYohoMstByItemCdStatus.InvalidItemCd);

                var datas = _yohoSetMstRepository.GetByItemCd(inputData.HpId, inputData.ItemCd, inputData.StartDate);

                if (datas.Any())
                    return new GetYohoMstByItemCdOutputData(datas.ToList(), GetYohoMstByItemCdStatus.Successful);
                else
                    return new GetYohoMstByItemCdOutputData(new List<YohoSetMstModel>(), GetYohoMstByItemCdStatus.DataNotFound);
            }
            finally
            {
                _yohoSetMstRepository.ReleaseResource();
            }
        }
    }
}
