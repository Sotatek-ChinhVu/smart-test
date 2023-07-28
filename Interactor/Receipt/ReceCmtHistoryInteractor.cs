﻿using Domain.Models.Insurance;
using Domain.Models.Receipt;
using Helper.Common;
using UseCase.Receipt.ReceCmtHistory;

namespace Interactor.Receipt;

public class ReceCmtHistoryInteractor : IReceCmtHistoryInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IInsuranceRepository _insuranceRepository;

    public ReceCmtHistoryInteractor(IReceiptRepository receiptRepository, IInsuranceRepository insuranceRepository)
    {
        _receiptRepository = receiptRepository;
        _insuranceRepository = insuranceRepository;
    }

    public ReceCmtHistoryOutputData Handle(ReceCmtHistoryInputData inputData)
    {
        try
        {
            var insuranceData = _insuranceRepository.GetInsuranceListById(inputData.HpId, inputData.PtId, 0, false, true);
            var hokenInfList = insuranceData.ListHokenInf;
            var receCmtList = _receiptRepository.GetReceCmtList(inputData.HpId, 0, inputData.PtId, 0, 0);

            foreach (var cmt in receCmtList)
            {
                var ptHokenInfModel = hokenInfList.FirstOrDefault(p => p.HokenId == cmt.HokenId);
                if (ptHokenInfModel != null)
                {
                    ptHokenInfModel.ChangeSinDate(cmt.SinYm * 100 + 1);
                }
            }

            var result = ConvertToResult(hokenInfList, receCmtList);
            return new ReceCmtHistoryOutputData(result, ReceCmtHistoryStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
        }
    }

    private List<ReceCmtHistoryOutputItem> ConvertToResult(List<HokenInfModel> hokenInfList, List<ReceCmtModel> receCmtList)
    {
        List<ReceCmtHistoryOutputItem> result = new();
        var sinYmList = receCmtList.Select(item => item.SinYm).Distinct().OrderByDescending(item => item).ToList();
        foreach (var sinYm in sinYmList)
        {
            var hokenIdList = receCmtList.Where(item => item.SinYm == sinYm).Select(item => item.HokenId).OrderByDescending(item => item).Distinct().ToList();
            foreach (var hokenId in hokenIdList)
            {
                var outputItem = new ReceCmtHistoryOutputItem(
                        sinYm,
                        CIUtil.SMonthToShowSWMonth(sinYm, 1),
                        hokenId,
                        GetHokenName(hokenId, hokenInfList),
                        receCmtList.Where(item => item.SinYm == sinYm && item.HokenId == hokenId).ToList()
                    );
                result.Add(outputItem);
            }
        }
        return result;
    }

    private string GetHokenName(int hokenId, List<HokenInfModel> hokenInfList)
    {
        return hokenInfList.FirstOrDefault(p => p.HokenId == hokenId)?.HokenSentaku ?? string.Empty;
    }
}
