using Domain.Models.MstItem;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.MstItem.SaveAddressMst;

namespace Interactor.MstItem
{
    public class SaveAddressMstInteractor : ISaveAddressMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveAddressMstInteractor(ITenantProvider tenantProvider, IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveAddressMstOutputData Handle(SaveAddressMstInputData inputData)
        {
            try
            {
                if (!inputData.PostCodeMsts.Any(x => x.PostCodeStatus > 0))
                {
                    return new SaveAddressMstOutputData(-1, string.Empty, string.Empty, SaveAddressMstStatus.Success);
                }

                foreach (var item in inputData.PostCodeMsts)
                {
                    if (!item.CheckDefaultValue() && item.Id == 0 && item.IsDeleted == 0)
                    {
                        var existRecords = _mstItemRepository.CheckPostCodeExist(inputData.HpId, item.PostCd);
                        if (existRecords)
                        {
                            var errMsg = "既に登録されているため、'" + item.PostCd + "'は登録できません。";
                            return new SaveAddressMstOutputData(item.Id, item.PostCd, errMsg, SaveAddressMstStatus.Error);
                        }
                        var duplicateItem = inputData.PostCodeMsts.Any(x => x.PostCd == item.PostCd && x.IsDeleted == 0 && x != item);
                        if (duplicateItem)
                        {
                            var errMsg = "既に登録されているため、'" + item.PostCd + "'は登録できません。";
                            return new SaveAddressMstOutputData(item.Id, item.PostCd, errMsg, SaveAddressMstStatus.Error);
                        }
                    }
                }

                bool result = _mstItemRepository.SaveAddressMaster(inputData.PostCodeMsts, inputData.HpId, inputData.UserId);

                return new SaveAddressMstOutputData(-1, string.Empty, string.Empty, result ? SaveAddressMstStatus.Success : SaveAddressMstStatus.Failed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
