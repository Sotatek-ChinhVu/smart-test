using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using UseCase.Receipt;
using UseCase.Receipt.SaveListReceCmt;

namespace Interactor.Receipt;

public class SaveListReceCmtInteractor : ISaveListReceCmtInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public SaveListReceCmtInteractor(IReceiptRepository receiptRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveListReceCmtOutputData Handle(SaveListReceCmtInputData inputData)
    {
        try
        {
            var responseValidate = ValidateInput(inputData);
            if (responseValidate != SaveListReceCmtStatus.ValidateSuccess)
            {
                return new SaveListReceCmtOutputData(responseValidate);
            }
            var listReceCmtModel = inputData.ReceCmtList.Select(item => ConvertToReceCmtModel(inputData.PtId, inputData.SinYm, inputData.HokenId, item))
                                                        .ToList();

            if (_receiptRepository.SaveListReceCmt(inputData.HpId, inputData.UserId, listReceCmtModel))
            {
                return new SaveListReceCmtOutputData(SaveListReceCmtStatus.Successed);
            }
            return new SaveListReceCmtOutputData(SaveListReceCmtStatus.Failed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private SaveListReceCmtStatus ValidateInput(SaveListReceCmtInputData inputData)
    {
        if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistListId(new List<long>() { inputData.PtId }))
        {
            return SaveListReceCmtStatus.InvalidPtId;
        }
        else if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveListReceCmtStatus.InvalidSinYm;
        }
        else if (inputData.HokenId < 0 || !_insuranceRepository.CheckExistHokenId(inputData.HokenId))
        {
            return SaveListReceCmtStatus.InvalidSinYm;
        }
        if (!inputData.ReceCmtList.Any())
        {
            return SaveListReceCmtStatus.ValidateSuccess;
        }
        return ValidateReceCmtItem(inputData);
    }

    private SaveListReceCmtStatus ValidateReceCmtItem(SaveListReceCmtInputData inputData)
    {
        var listReceCmtDB = _receiptRepository.GetListReceCmt(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
        var listItemCds = inputData.ReceCmtList.Where(item => item.ItemCd != string.Empty).Select(item => item.ItemCd.Trim()).Distinct().ToList();
        if (listItemCds.Any() && _mstItemRepository.GetCheckItemCds(listItemCds).Count != listItemCds.Count)
        {
            return SaveListReceCmtStatus.InvalidItemCd;
        }
        var listReceCmtIds = inputData.ReceCmtList.Where(item => item.Id > 0).Select(item => item.Id).Distinct().ToList();
        var countReceCmt = listReceCmtDB.Count(item => listReceCmtIds.Contains(item.Id));
        if (listReceCmtIds.Any() && countReceCmt != listReceCmtIds.Count)
        {
            return SaveListReceCmtStatus.InvalidReceCmtId;
        }
        else if (inputData.ReceCmtList.Any(item => item.CmtKbn > 2 || item.CmtKbn < 1))
        {
            return SaveListReceCmtStatus.InvalidCmtKbn;
        }
        else if (inputData.ReceCmtList.Any(item => item.CmtSbt > 1 || item.CmtSbt < 0))
        {
            return SaveListReceCmtStatus.InvalidCmtSbt;
        }
        else if (inputData.ReceCmtList.Any(item => item.Cmt == string.Empty))
        {
            return SaveListReceCmtStatus.InvalidCmt;
        }

        // validate Cmt detail
        foreach (var cmtInput in inputData.ReceCmtList)
        {
            if ((cmtInput.CmtKbn == 2 && cmtInput.CmtSbt == 0) || (cmtInput.CmtKbn == 1 && cmtInput.CmtSbt == 0))
            {
                if (cmtInput.ItemCd == string.Empty)
                {
                    return SaveListReceCmtStatus.InvalidCmt;
                }
                continue;
            }
            if ((cmtInput.CmtKbn == 1 && cmtInput.CmtSbt == 1) || (cmtInput.CmtKbn == 2 && cmtInput.CmtSbt == 1))
            {
                if (cmtInput.ItemCd != string.Empty)
                {
                    return SaveListReceCmtStatus.InvalidItemCd;
                }else if (cmtInput.CmtData != string.Empty)
                {
                    return SaveListReceCmtStatus.InvalidCmtData;
                }
                var cmtDB = listReceCmtDB.FirstOrDefault(item => item.CmtKbn == cmtInput.CmtKbn && item.CmtSbt == cmtInput.CmtSbt);
                if (cmtDB == null)
                {
                    if (cmtInput.Id != 0)
                    {
                        return SaveListReceCmtStatus.InvalidReceCmtId;
                    }
                    continue;
                }
                if (cmtDB.Id != cmtInput.Id)
                {
                    return SaveListReceCmtStatus.InvalidReceCmtId;
                }
                continue;
            }
            return SaveListReceCmtStatus.InvalidCmtSbt;
        }
        return SaveListReceCmtStatus.ValidateSuccess;
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
