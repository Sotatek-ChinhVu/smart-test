using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.CheckedItemName;
using UseCase.MedicalExamination.ConvertFromHistoryTodayOrder;

namespace Interactor.MedicalExamination
{
    public class ConvertFromHistoryToTodayOdrInteractor : ICheckedItemNameInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;

        public ConvertFromHistoryToTodayOdrInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public ConvertFromHistoryTodayOrderOutputData Handle(CheckedItemNameInputData inputData)
        {
            try
            {
                if (inputData.)
                {

                }

                return new CheckedItemNameOutputData(CheckedItemNameStatus.Successed, checkedName);
            }
            catch
            {
                return new CheckedItemNameOutputData(CheckedItemNameStatus.Failed, new());
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
