using Domain.Models.HistoryOrder;
using UseCase.MedicalExamination.GetHistoryIndex;

namespace Interactor.MedicalExamination
{
    public class GetHistoryIndexInteractor : IGetHistoryIndexInputPort
    {
        private readonly IHistoryOrderRepository _historyRepository;
        public GetHistoryIndexInteractor(IHistoryOrderRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public GetHistoryIndexOutputData Handle(GetHistoryIndexInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetHistoryIndexOutputData(GetHistoryIndexStatus.InvalidHpId, new());
                }
                if (inputData.UserId <= 0)
                {
                    return new GetHistoryIndexOutputData(GetHistoryIndexStatus.InvalidUserId, new());
                }
                if (inputData.PtId <= 0)
                {
                    return new GetHistoryIndexOutputData(GetHistoryIndexStatus.InvalidPtId, new());
                }
                if (inputData.FilterId < 0)
                {
                    return new GetHistoryIndexOutputData(GetHistoryIndexStatus.InvalidFilterId, new());
                }
                if (inputData.IsDeleted < 0)
                {
                    return new GetHistoryIndexOutputData(GetHistoryIndexStatus.InvalidIsDeleted, new());
                }

                var historyIndex = _historyRepository.GetHistoryIndex(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.UserId, inputData.FilterId, inputData.IsDeleted);
                return new GetHistoryIndexOutputData(GetHistoryIndexStatus.Successed, historyIndex);
            }
            finally
            {
                _historyRepository.ReleaseResource();
            }
        }
    }
}
