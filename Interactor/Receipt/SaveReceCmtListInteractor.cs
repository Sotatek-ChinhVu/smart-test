using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using Helper.Constants;
using UseCase.Receipt;
using UseCase.Receipt.SaveListReceCmt;

namespace Interactor.Receipt;

public class SaveReceCmtListInteractor : ISaveReceCmtListInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public SaveReceCmtListInteractor(IReceiptRepository receiptRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveReceCmtListOutputData Handle(SaveReceCmtListInputData inputData)
    {
        try
        {
            var listReceCmtDB = _receiptRepository.GetReceCmtList(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
            var responseValidate = ValidateInput(inputData, listReceCmtDB);
            if (responseValidate != SaveReceCmtListStatus.ValidateSuccess)
            {
                return new SaveReceCmtListOutputData(responseValidate);
            }

            var isHaveHeaderFreeCmt = inputData.ReceCmtList.Any(item => item.CmtKbn == ReceCmtKbn.Header && item.CmtSbt == ReceCmtSbt.FreeCmt);
            var isHaveFooterFreeCmt = inputData.ReceCmtList.Any(item => item.CmtKbn == ReceCmtKbn.Footer && item.CmtSbt == ReceCmtSbt.FreeCmt);
            if (isHaveHeaderFreeCmt)
            {
                var receCmtDeletedList = listReceCmtDB.Where(item => item.CmtKbn == ReceCmtKbn.Header && item.CmtSbt == ReceCmtSbt.FreeCmt)
                                                      .Select(item => new ReceCmtItem(item, true))
                                                      .ToList();
                inputData.ReceCmtList.AddRange(receCmtDeletedList);
            }

            if (isHaveFooterFreeCmt)
            {
                var receCmtDeletedList = listReceCmtDB.Where(item => item.CmtKbn == ReceCmtKbn.Footer && item.CmtSbt == ReceCmtSbt.FreeCmt)
                                                      .Select(item => new ReceCmtItem(item, true))
                                                      .ToList();
                inputData.ReceCmtList.AddRange(receCmtDeletedList);
            }
            
            var listReceCmtModel = inputData.ReceCmtList.Select(item => ConvertToReceCmtModel(inputData.PtId, inputData.SinYm, inputData.HokenId, item))
                                                        .ToList();

            if (_receiptRepository.SaveReceCmtList(inputData.HpId, inputData.UserId, listReceCmtModel))
            {
                return new SaveReceCmtListOutputData(SaveReceCmtListStatus.Successed);
            }
            return new SaveReceCmtListOutputData(SaveReceCmtListStatus.Failed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private SaveReceCmtListStatus ValidateInput(SaveReceCmtListInputData inputData, List<ReceCmtModel> listReceCmtDB)
    {
        if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
        {
            return SaveReceCmtListStatus.InvalidPtId;
        }
        else if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveReceCmtListStatus.InvalidSinYm;
        }
        else if (inputData.HokenId < 0 || !_insuranceRepository.CheckExistHokenId(inputData.HokenId))
        {
            return SaveReceCmtListStatus.InvalidHokenId;
        }
        else if (!inputData.ReceCmtList.Any())
        {
            return SaveReceCmtListStatus.Failed;
        }
        return ValidateReceCmtItem(inputData, listReceCmtDB);
    }

    private SaveReceCmtListStatus ValidateReceCmtItem(SaveReceCmtListInputData inputData, List<ReceCmtModel> listReceCmtDB)
    {
        var listItemCds = inputData.ReceCmtList.Where(item => item.ItemCd != string.Empty).Select(item => item.ItemCd.Trim()).Distinct().ToList();
        if (listItemCds.Any() && _mstItemRepository.GetCheckItemCds(listItemCds).Count != listItemCds.Count)
        {
            return SaveReceCmtListStatus.InvalidItemCd;
        }
        var listReceCmtIds = inputData.ReceCmtList.Where(item => item.Id > 0).Select(item => item.Id).Distinct().ToList();
        var countReceCmt = listReceCmtDB.Count(item => listReceCmtIds.Contains(item.Id));
        if (listReceCmtIds.Any() && countReceCmt != listReceCmtIds.Count)
        {
            return SaveReceCmtListStatus.InvalidReceCmtId;
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
