using Domain.Models.SpecialNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Domain.Models.User;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.SpecialNote.Save;
using static Helper.Constants.UserConst;

namespace Interactor.SpecialNote
{
    public class SaveSpecialNoteInteractor : ISaveSpecialNoteInputPort
    {
        private readonly ISpecialNoteRepository _specialNoteRepository;
        private readonly ISummaryInfRepository _summaryInfRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveSpecialNoteInteractor(ITenantProvider tenantProvider, ISpecialNoteRepository specialNoteRepository, ISummaryInfRepository summaryInfRepository, IUserRepository userRepository)
        {
            _specialNoteRepository = specialNoteRepository;
            _summaryInfRepository = summaryInfRepository;
            _userRepository = userRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveSpecialNoteOutputData Handle(SaveSpecialNoteInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.InvalidPtId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.InvalidSinDate);
                }
                var sumaryInf = _summaryInfRepository.Get(inputData.HpId, inputData.PtId);
                if ((sumaryInf.Text != inputData.SummaryTab.Text || sumaryInf.Rtext != inputData.SummaryTab.Rtext) && _userRepository.GetPermissionByScreenCode(inputData.HpId, inputData.UserId, FunctionCode.EditSummary) != PermissionType.Unlimited)
                {
                    return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.NoPermissionSaveSummary);
                }

                var result = _specialNoteRepository.SaveSpecialNote(inputData.HpId, inputData.PtId, inputData.SinDate, new SummaryInfModel(inputData.SummaryTab.Id, inputData.SummaryTab.HpId, inputData.SummaryTab.PtId, inputData.SummaryTab.SeqNo, inputData.SummaryTab.Text, inputData.SummaryTab.Rtext, CIUtil.GetJapanDateTimeNow(), CIUtil.GetJapanDateTimeNow()), inputData.ImportantNoteTab, new PatientInfoModel(inputData.PatientInfoTab.PregnancyItems.Select(p => new PtPregnancyModel(
                        p.Id,
                        p.HpId,
                        p.PtId,
                        p.SeqNo,
                        p.StartDate,
                        p.EndDate,
                        p.PeriodDate,
                        p.PeriodDueDate,
                        p.OvulationDate,
                        p.OvulationDueDate,
                        p.IsDeleted,
                        CIUtil.GetJapanDateTimeNow(),
                        inputData.UserId,
                        string.Empty,
                        p.SinDate
                    )
                    ).ToList(), inputData.PatientInfoTab.PtCmtInfItems, inputData.PatientInfoTab.SeikatureInfItems, new List<PhysicalInfoModel> { new PhysicalInfoModel(inputData.PatientInfoTab.KensaInfDetailItems.Select(k => new KensaInfDetailModel(k.HpId, k.PtId, k.IraiCd, k.SeqNo, k.IraiDate, k.RaiinNo, k.KensaItemCd, k.ResultVal, k.ResultType, k.AbnormalKbn, k.IsDeleted, k.CmtCd1, k.CmtCd2, DateTime.MinValue, string.Empty, string.Empty, 0)).ToList()) }), inputData.UserId);

                if (!result) return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.Failed);

                return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.Successed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _userRepository.ReleaseResource();
                _specialNoteRepository.ReleaseResource();
                _summaryInfRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
