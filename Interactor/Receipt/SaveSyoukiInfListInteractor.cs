using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Receipt;
using UseCase.Receipt.SaveListSyoukiInf;

namespace Interactor.Receipt;

public class SaveSyoukiInfListInteractor : ISaveSyoukiInfListInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public SaveSyoukiInfListInteractor(ITenantProvider tenantProvider, IReceiptRepository receiptRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public SaveSyoukiInfListOutputData Handle(SaveSyoukiInfListInputData inputData)
    {
        try
        {
            var responseValidate = ValidateInput(inputData);
            if (responseValidate != SaveSyoukiInfListStatus.ValidateSuccess)
            {
                return new SaveSyoukiInfListOutputData(responseValidate, new());
            }

            var listSyoukiInfDB = _receiptRepository.GetSyoukiInfList(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId, true);
            var listSeqNoNotDeletedDB = listSyoukiInfDB.Where(item => !item.IsDeleted).Select(item => item.SeqNo).Distinct().ToList();
            List<SyoukiInfItem> syoukiInfInvalidList = inputData.SyoukiInfList.Where(item => item.SeqNo > 0 && !listSeqNoNotDeletedDB.Contains(item.SeqNo)).ToList();
            foreach (var syoukiInf in syoukiInfInvalidList)
            {
                var syoukiKbn = listSyoukiInfDB.FirstOrDefault(item => item.SeqNo == syoukiInf.SeqNo)?.SyoukiKbn ?? syoukiInf.SyoukiKbn;
                syoukiInf.ChangeSyoukiKbn(syoukiKbn);
            }

            var listReceCmtModel = inputData.SyoukiInfList.Select(item => ConvertToSyoukiInfModel(inputData.PtId, inputData.SinYm, inputData.HokenId, item))
                                                          .ToList();

            if (_receiptRepository.SaveSyoukiInfList(inputData.HpId, inputData.UserId, listReceCmtModel))
            {
                return new SaveSyoukiInfListOutputData(SaveSyoukiInfListStatus.Successed, syoukiInfInvalidList);
            }
            return new SaveSyoukiInfListOutputData(SaveSyoukiInfListStatus.Failed, syoukiInfInvalidList);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }

    private SaveSyoukiInfListStatus ValidateInput(SaveSyoukiInfListInputData inputData)
    {
        if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistIdList(inputData.HpId, new List<long>() { inputData.PtId }))
        {
            return SaveSyoukiInfListStatus.InvalidPtId;
        }
        else if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveSyoukiInfListStatus.InvalidSinYm;
        }
        else if (inputData.HokenId < 0 || !_insuranceRepository.CheckExistHokenId(inputData.HpId, inputData.HokenId))
        {
            return SaveSyoukiInfListStatus.InvalidHokenId;
        }
        else if (!inputData.SyoukiInfList.Any())
        {
            return SaveSyoukiInfListStatus.Failed;
        }
        var listSyoukiKbn = inputData.SyoukiInfList.Select(item => new SyoukiKbnMstModel(item.SyoukiKbn, item.SyoukiKbnStartYm)).ToList();
        if (listSyoukiKbn.Any() && !_receiptRepository.CheckExistSyoukiKbn(inputData.HpId, inputData.SinYm, listSyoukiKbn))
        {
            return SaveSyoukiInfListStatus.InvalidSyoukiKbn;
        }
        return SaveSyoukiInfListStatus.ValidateSuccess;
    }

    private SyoukiInfModel ConvertToSyoukiInfModel(long ptId, int sinYm, int hokenId, SyoukiInfItem syoukiInf)
    {
        return new SyoukiInfModel(
                ptId,
                sinYm,
                hokenId,
                syoukiInf.SeqNo,
                syoukiInf.SortNo,
                syoukiInf.SyoukiKbn,
                syoukiInf.Syouki,
                syoukiInf.IsDeleted
            );
    }
}
