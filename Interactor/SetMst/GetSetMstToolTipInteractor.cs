
using Domain.Models.SetGenerationMst;
using Domain.Models.SetMst;
using UseCase.SetMst.GetToolTip;

namespace Interactor.SetMst
{
    public class GetSetMstToolTipInteractor : IGetSetMstToolTipInputPort
    {
        private readonly ISetMstRepository _setRepository;
        private readonly ISetGenerationMstRepository _setGenerationRepository;
        public GetSetMstToolTipInteractor(ISetMstRepository setRepository, ISetGenerationMstRepository setGenerationRepository)
        {
            _setRepository = setRepository;
            _setGenerationRepository = setGenerationRepository;
        }

        public GetSetMstToolTipOutputData Handle(GetSetMstToolTipInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new GetSetMstToolTipOutputData(new(), GetSetMstToolTipStatus.InvalidHpId);
                }
                if (inputData.SetCd < 0)
                {
                    return new GetSetMstToolTipOutputData(new(), GetSetMstToolTipStatus.InvalidSetCd);
                }

                var tooltip = _setRepository.GetToolTip(inputData.HpId, inputData.SetCd);

                return !(tooltip.ListOrders.Count > 0 && tooltip.ListByomeis.Count > 0 && tooltip.ListKarteNames.Count > 0) ? new GetSetMstToolTipOutputData(tooltip, GetSetMstToolTipStatus.Successed) : new GetSetMstToolTipOutputData(new(), GetSetMstToolTipStatus.NoData);
            }
            finally
            {
                _setRepository.ReleaseResource();
                _setGenerationRepository.ReleaseResource();
            }
            
        }
    }
}