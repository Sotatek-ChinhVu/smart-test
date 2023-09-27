using Infrastructure.Logger;
using UseCase.Logger;

namespace Interactor.Logger
{
    public class WriteLogInteractor : IWriteLogInputPort
    {
        private readonly ILoggingHandler _loggingHandler;

        public WriteLogInteractor(ILoggingHandler loggingHandler)
        {
            _loggingHandler = loggingHandler;
        }

        public WriteLogOutputData Handle(WriteLogInputData inputData)
        {
            var status = _loggingHandler.WriteAuditLog(
                 inputData.RequestInfo, inputData.EventCd, inputData.PtId, inputData.RaiinNo, inputData.SinDay, inputData.Description, inputData.LogType);

            return new WriteLogOutputData(status ? WriteLogStatus.Successed : WriteLogStatus.Failed);
        }
    }
}
