﻿using Domain.Models.NextOrder;
using Domain.Models.Reception;
using Helper.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.NextOrder.Get;
using UseCase.Reception.GetLastRaiinInfs;
using UseCase.Reception.GetListRaiinInfs;

namespace Interactor.RaiinFilterMst;

public class GetListRaiinInfsInteractor : IGetListRaiinInfsInputPort
{
    private readonly IReceptionRepository _raiinInfRepository;

    public GetListRaiinInfsInteractor(IReceptionRepository raiinInfRepository)
    {
        _raiinInfRepository = raiinInfRepository;
    }

    public GetListRaiinInfsOutputData Handle(GetListRaiinInfsInputData inputData)
    {
        try
        {
            var data = _raiinInfRepository.GetListRaiinInf(inputData.HpId, inputData.PtId);
            if (inputData.HpId < 0)
            {
                return new GetListRaiinInfsOutputData(new(), GetListRaiinInfsStatus.InValidHpId);
            }
            if (inputData.PtId <= 0)
            {
                return new GetListRaiinInfsOutputData(new(), GetListRaiinInfsStatus.InValidPtId);
            }
            var listNextOrderFiles = GetListRaiinInfos(inputData.HpId, inputData.PtId).Select(item => new GetListRaiinInfsInputItem(
                                                            item.HpId, 
                                                            item.PtId, 
                                                            item.SinDate, 
                                                            item.UketukeNo, 
                                                            item.Status,
                                                            item.KaSname,
                                                            item.SName,
                                                            item.Houbetu,
                                                            item.HokensyaNo,
                                                            item.HokenKbn,
                                                            item.HokenId,
                                                            item.HokenPid,
                                                            item.RaiinNo))
                                                            .ToList();
            return new GetListRaiinInfsOutputData(listNextOrderFiles, GetListRaiinInfsStatus.Success);
        }
        finally
        {
            _raiinInfRepository.ReleaseResource();
        }
    }

    private List<ReceptionModel> GetListRaiinInfos(int hpId, long ptId)
    {
        var raiinInfos = _raiinInfRepository.GetListRaiinInf(hpId, ptId);
        List<ReceptionModel> result = new();
        foreach (var raiinInfo in raiinInfos)
        {
            result.Add(new ReceptionModel(
                raiinInfo.HpId, 
                raiinInfo.PtId, 
                raiinInfo.SinDate, 
                raiinInfo.UketukeNo, 
                raiinInfo.Status, 
                raiinInfo.KaSname, 
                raiinInfo.SName, 
                raiinInfo.Houbetu, 
                raiinInfo.HokensyaNo, 
                raiinInfo.HokenKbn, 
                raiinInfo.HokenId,
                raiinInfo.HokenPid,
                raiinInfo.RaiinNo));
        }
        return result;
    }
}