using Domain.Models.HokenMst;
using UseCase.HokenMst.GetDetail;

namespace Interactor.HokenMst
{
    public class GetDetailHokenMstInteractor : IGetDetailHokenMstInputPort
    {
        private readonly IHokenMstRepository _hokenMstRepository;

        public GetDetailHokenMstInteractor(IHokenMstRepository hokenMstRepository)
        {
            _hokenMstRepository = hokenMstRepository;
        }

        public GetDetailHokenMstOutputData Handle(GetDetailHokenMstInputData inputData)
        {
            if (inputData.HpId <= 0)
                return new GetDetailHokenMstOutputData(default, GetDetailHokenMstStatus.InvalidHpId);

            if (inputData.SinDate <= 0)
                return new GetDetailHokenMstOutputData(default, GetDetailHokenMstStatus.InvalidSinDate);

            if (inputData.HokenNo < 0)
                return new GetDetailHokenMstOutputData(default, GetDetailHokenMstStatus.InvalidHokenNo);

            if (inputData.HokenEdaNo < 0)
                return new GetDetailHokenMstOutputData(default, GetDetailHokenMstStatus.InvalidHokenEdaNo);

            if (inputData.PrefNo < 0)
                return new GetDetailHokenMstOutputData(default, GetDetailHokenMstStatus.InvalidPrefNo);

            var result = _hokenMstRepository.GetHokenMaster(inputData.HpId, inputData.HokenNo, inputData.HokenEdaNo, inputData.PrefNo, inputData.SinDate);

            return new GetDetailHokenMstOutputData(result, GetDetailHokenMstStatus.Successed);
        }
    }
}
