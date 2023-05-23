using Domain.Models.FlowSheet;
using UseCase.Holiday.SaveHoliday;

namespace Interactor.Holiday
{
    public class SaveHolidayMstInteractor : ISaveHolidayMstInputPort
    {
        private readonly IFlowSheetRepository _flowSheetRepository;

        public SaveHolidayMstInteractor(IFlowSheetRepository flowSheetRepository)
        {
            _flowSheetRepository = flowSheetRepository;
        }

        public SaveHolidayMstOutputData Handle(SaveHolidayMstInputData inputData)
        {
            if (inputData.UserId <= 0)
                return new SaveHolidayMstOutputData(SaveHolidayMstStatus.InvalidUserId);
            try
            {
                bool result = _flowSheetRepository.SaveHolidayMst(inputData.Holiday, inputData.UserId);
                if (result)
                    return new SaveHolidayMstOutputData(SaveHolidayMstStatus.Successful);
                else
                    return new SaveHolidayMstOutputData(SaveHolidayMstStatus.Failed);
            }
            finally
            {
                _flowSheetRepository.ReleaseResource();
            }
        }
    }
}
