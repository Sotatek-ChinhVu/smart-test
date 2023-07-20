﻿using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using UseCase.Receipt;
using UseCase.Receipt.SaveReceCheckCmtList;

namespace Interactor.Receipt;

public class SaveReceCheckCmtListInteractor : ISaveReceCheckCmtListInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public SaveReceCheckCmtListInteractor(IReceiptRepository receiptRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveReceCheckCmtListOutputData Handle(SaveReceCheckCmtListInputData inputData)
    {
        try
        {
            var receCheckErrorList = inputData.ReceCheckErrorList.Select(item => ConvertToReceCheckErrorModel(item))
                                                                 .ToList();
            var responseValidate = ValidateInput(inputData, receCheckErrorList);
            if (responseValidate != SaveReceCheckCmtListStatus.ValidateSuccess)
            {
                return new SaveReceCheckCmtListOutputData(responseValidate);
            }
            var receCheckCmtList = inputData.ReceCheckCmtList.Select(item => ConvertToReceCheckCmtModel(item))
                                                             .ToList();

            if (_receiptRepository.SaveReceCheckCmtList(inputData.HpId, inputData.UserId, inputData.HokenId, inputData.SinYm, inputData.PtId, receCheckCmtList)
                && _receiptRepository.SaveReceCheckErrList(inputData.HpId, inputData.UserId, inputData.HokenId, inputData.SinYm, inputData.PtId, receCheckErrorList))
            {
                return new SaveReceCheckCmtListOutputData(SaveReceCheckCmtListStatus.Successed);
            }
            return new SaveReceCheckCmtListOutputData(SaveReceCheckCmtListStatus.Failed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private ReceCheckErrModel ConvertToReceCheckErrorModel(ReceCheckErrorItem item)
    {
        return new ReceCheckErrModel(
                   item.ErrCd,
                   item.SinDate,
                   item.ACd,
                   item.BCd,
                   item.IsChecked
               );
    }

    private ReceCheckCmtModel ConvertToReceCheckCmtModel(ReceCheckCmtItem item)
    {
        return new ReceCheckCmtModel(
                   item.SeqNo,
                   item.StatusColor,
                   item.Cmt,
                   item.IsChecked,
                   item.SortNo,
                   item.IsDeleted
                );
    }

    private SaveReceCheckCmtListStatus ValidateInput(SaveReceCheckCmtListInputData inputData, List<ReceCheckErrModel> receCheckErrorList)
    {
        if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
        {
            return SaveReceCheckCmtListStatus.InvalidPtId;
        }
        else if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveReceCheckCmtListStatus.InvalidSinYm;
        }
        else if (inputData.HokenId < 0 || !_insuranceRepository.CheckExistHokenId(inputData.HokenId))
        {
            return SaveReceCheckCmtListStatus.InvalidHokenId;
        }
        else if (!inputData.ReceCheckCmtList.Any() && !receCheckErrorList.Any())
        {
            return SaveReceCheckCmtListStatus.Failed;
        }
        else if (inputData.ReceCheckCmtList.Any(item => item.StatusColor < 0 || item.StatusColor > 3))
        {
            return SaveReceCheckCmtListStatus.InvalidStatusColor;
        }
        else if (inputData.ReceCheckCmtList.Any(item => item.Cmt == string.Empty))
        {
            return SaveReceCheckCmtListStatus.InvalidCmt;
        }
        var seqNoList = inputData.ReceCheckCmtList.Where(item => item.SeqNo > 0).Select(item => item.SeqNo).ToList();
        if (seqNoList.Any() && !_receiptRepository.CheckExistSeqNoReceCheckCmtList(inputData.HpId, inputData.HokenId, inputData.SinYm, inputData.PtId, seqNoList))
        {
            return SaveReceCheckCmtListStatus.InvalidSeqNo;
        }
        else if (receCheckErrorList.Any() && !_receiptRepository.CheckExistSeqNoReceCheckErrorList(inputData.HpId, inputData.HokenId, inputData.SinYm, inputData.PtId, receCheckErrorList))
        {
            return SaveReceCheckCmtListStatus.InvalidReceCheckErrorItem;
        }
        return SaveReceCheckCmtListStatus.ValidateSuccess;
    }
}
