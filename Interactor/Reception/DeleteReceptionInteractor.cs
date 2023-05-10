﻿using Domain.Models.Reception;
using Interactor.CalculateService;
using UseCase.Accounting.Recaculate;
using UseCase.Reception.Delete;

namespace Interactor.Reception;

public class DeleteReceptionInteractor : IDeleteReceptionInputPort
{
    private readonly IReceptionRepository _raiinInfRepository;
    private readonly ICalculateService _calculateService;

    public DeleteReceptionInteractor(IReceptionRepository raiinInfRepository, ICalculateService calculateService)
    {
        _raiinInfRepository = raiinInfRepository;
        _calculateService = calculateService;
    }

    public DeleteReceptionOutputData Handle(DeleteReceptionInputData inputData)
    {
        try
        {
            var raiinNos = inputData.RaiinNos.Distinct().ToList();
            if (inputData.HpId < 0)
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidHpId, new());
            }
            if (inputData.UserId <= 0)
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidUserId, new());
            }
            if (raiinNos.Count() < 1)
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidRaiinNo, new());
            }
            if (!_raiinInfRepository.CheckExistOfRaiinNos(raiinNos.Select(r => r.RaiinNo).ToList()))
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidRaiinNo, new());
            }

            var result = _raiinInfRepository.Delete(inputData.Flag, inputData.HpId, inputData.PtId, inputData.UserId, inputData.SinDate, raiinNos.Select(r => new Tuple<long, long, int>(r.RaiinNo, r.OyaRaiinNo, r.Status)).ToList());

            if (result.Count > 0)
            {
                Task.Run(() =>
                {
                    _calculateService.RunCalculate(new RecaculationInputDto(inputData.HpId, inputData.PtId, inputData.SinDate, 0, "DR_"));
                });
                //Item1: SinDate, Item2: RaiinNo, Item3: PtId
                return new DeleteReceptionOutputData(DeleteReceptionStatus.Successed, result.Select(r => new DeleteReceptionItem(r.Item1, r.Item2, r.Item3)).ToList());
            }

            return new DeleteReceptionOutputData(DeleteReceptionStatus.Failed, new());
        }
        finally
        {
            _raiinInfRepository.ReleaseResource();
        }
    }
}