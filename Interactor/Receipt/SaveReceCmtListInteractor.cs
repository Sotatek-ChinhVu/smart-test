using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using Helper.Constants;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Receipt;
using UseCase.Receipt.SaveListReceCmt;

namespace Interactor.Receipt;

public class SaveReceCmtListInteractor : ISaveReceCmtListInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public SaveReceCmtListInteractor(ITenantProvider tenantProvider, IReceiptRepository receiptRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public SaveReceCmtListOutputData Handle(SaveReceCmtListInputData inputData)
    {
        try
        {
            var responseValidate = ValidateInput(inputData);
            if (responseValidate != SaveReceCmtListStatus.ValidateSuccess)
            {
                return new SaveReceCmtListOutputData(new(), responseValidate);
            }

            var listReceCmtDB = _receiptRepository.GetReceCmtList(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId, 0, true);
            var receCmtIdNotDeletedDB = listReceCmtDB.Where(item => !item.IsDeleted).Select(item => item.Id).Distinct().ToList();
            List<ReceCmtItem> receCmtInvalidList = inputData.ReceCmtList.Where(item => item.Id > 0 && !receCmtIdNotDeletedDB.Contains(item.Id)).ToList();
            foreach (var receCmt in receCmtInvalidList)
            {
                var cmtData = listReceCmtDB.FirstOrDefault(item => item.Id == receCmt.Id);
                if (cmtData != null)
                {
                    receCmt.ChangeCmtData(cmtData.CmtData, cmtData.Cmt);
                }
            }

            var receCmtDeletedList = listReceCmtDB.Where(item => (item.CmtKbn == ReceCmtKbn.Header || item.CmtKbn == ReceCmtKbn.Footer) && item.CmtSbt == ReceCmtSbt.FreeCmt)
                                                  .Select(item => new ReceCmtItem(item, true))
                                                  .ToList();
            inputData.ReceCmtList.AddRange(receCmtDeletedList);

            var listReceCmtModel = inputData.ReceCmtList.Select(item => ConvertToReceCmtModel(inputData.PtId, inputData.SinYm, inputData.HokenId, item))
                                                        .ToList();

            if (_receiptRepository.SaveReceCmtList(inputData.HpId, inputData.UserId, listReceCmtModel))
            {
                return new SaveReceCmtListOutputData(receCmtInvalidList, SaveReceCmtListStatus.Successed);
            }
            return new SaveReceCmtListOutputData(receCmtInvalidList, SaveReceCmtListStatus.Failed);
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

    private SaveReceCmtListStatus ValidateInput(SaveReceCmtListInputData inputData)
    {
        if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistIdList(inputData.HpId, new List<long>() { inputData.PtId }))
        {
            return SaveReceCmtListStatus.InvalidPtId;
        }
        else if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveReceCmtListStatus.InvalidSinYm;
        }
        else if (inputData.HokenId < 0 || !_insuranceRepository.CheckExistHokenId(inputData.HpId, inputData.HokenId))
        {
            return SaveReceCmtListStatus.InvalidHokenId;
        }
        return ValidateReceCmtItem(inputData);
    }

    private SaveReceCmtListStatus ValidateReceCmtItem(SaveReceCmtListInputData inputData)
    {
        var listItemCds = inputData.ReceCmtList.Where(item => item.ItemCd != string.Empty).Select(item => item.ItemCd.Trim()).Distinct().ToList();
        if (listItemCds.Any() && _mstItemRepository.GetCheckItemCds(inputData.HpId, listItemCds).Count != listItemCds.Count)
        {
            return SaveReceCmtListStatus.InvalidItemCd;
        }
        else if (inputData.ReceCmtList.Any(item => item.CmtKbn > 2 || item.CmtKbn < 1))
        {
            return SaveReceCmtListStatus.InvalidCmtKbn;
        }
        else if (inputData.ReceCmtList.Any(item => item.CmtSbt > 1 || item.CmtSbt < 0))
        {
            return SaveReceCmtListStatus.InvalidCmtSbt;
        }
        else if (inputData.ReceCmtList.Any(item => item.Cmt == string.Empty))
        {
            return SaveReceCmtListStatus.InvalidCmt;
        }

        // validate Cmt detail
        foreach (var cmtInput in inputData.ReceCmtList)
        {
            if ((cmtInput.CmtKbn == 2 && cmtInput.CmtSbt == 0) || (cmtInput.CmtKbn == 1 && cmtInput.CmtSbt == 0))
            {
                if (cmtInput.ItemCd == string.Empty)
                {
                    return SaveReceCmtListStatus.InvalidCmt;
                }
                continue;
            }
            if ((cmtInput.CmtKbn == 1 && cmtInput.CmtSbt == 1) || (cmtInput.CmtKbn == 2 && cmtInput.CmtSbt == 1))
            {
                if (cmtInput.ItemCd != string.Empty)
                {
                    return SaveReceCmtListStatus.InvalidItemCd;
                }
                else if (cmtInput.CmtData != string.Empty)
                {
                    return SaveReceCmtListStatus.InvalidCmtData;
                }
                continue;
            }
            return SaveReceCmtListStatus.InvalidCmtSbt;
        }
        return SaveReceCmtListStatus.ValidateSuccess;
    }

    private ReceCmtModel ConvertToReceCmtModel(long ptId, int sinYm, int hokenId, ReceCmtItem inputItem)
    {
        return new ReceCmtModel(
                                    inputItem.Id,
                                    ptId,
                                    inputItem.SeqNo,
                                    sinYm,
                                    hokenId,
                                    inputItem.CmtKbn,
                                    inputItem.CmtSbt,
                                    inputItem.Cmt,
                                    inputItem.CmtData,
                                    inputItem.ItemCd,
                                    inputItem.IsDeleted
                                );
    }
}
