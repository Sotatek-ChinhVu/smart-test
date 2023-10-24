using Domain.Models.FlowSheet;
using UseCase.FlowSheet.GetTooltip;

namespace Interactor.FlowSheet
{
    public class GetTooltipFlowSheetInteractor : IGetTooltipInputPort
    {
        private readonly IFlowSheetRepository _flowsheetRepository;
        public GetTooltipFlowSheetInteractor(IFlowSheetRepository repository)
        {
            _flowsheetRepository = repository;
        }

        public GetTooltipOutputData Handle(GetTooltipInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetTooltipOutputData(new(), GetTooltipStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new GetTooltipOutputData(new(), GetTooltipStatus.InvalidPtId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetTooltipOutputData(new(), GetTooltipStatus.InvalidSinDate);
                }
                if (inputData.StartDate <= 0)
                {
                    return new GetTooltipOutputData(new(), GetTooltipStatus.InvalidStartDate);
                }
                if (inputData.EndDate <= 0)
                {
                    return new GetTooltipOutputData(new(), GetTooltipStatus.InvalidEndDate);
                }

                var result = _flowsheetRepository.GetTooltip(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.StartDate, inputData.EndDate, inputData.IsAll);

                return new GetTooltipOutputData(result, GetTooltipStatus.Success);
            }
            finally
            {
                _flowsheetRepository.ReleaseResource();
            }
        }
    }
}
