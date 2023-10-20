using Domain.Models.Reception;
using Interactor.CalculateService;
using UseCase.Accounting.Recaculate;
using UseCase.Reception.Delete;

namespace Interactor.Reception;

public class DeleteReceptionInteractor : IDeleteReceptionInputPort
{
    private readonly IReceptionRepository _raiinInfRepository;
    private readonly IReceptionRepository _receptionRepository;
    private readonly ICalculateService _calculateService;

    public DeleteReceptionInteractor(IReceptionRepository raiinInfRepository, ICalculateService calculateService, IReceptionRepository receptionRepository)
    {
        _raiinInfRepository = raiinInfRepository;
        _calculateService = calculateService;
        _receptionRepository = receptionRepository;
    }

    public DeleteReceptionOutputData Handle(DeleteReceptionInputData inputData)
    {
        try
        {
            List<ReceptionRowModel> receptionInfos = new();
            List<SameVisitModel> sameVisitList = new();
            var raiinNos = inputData.RaiinNos.Distinct().ToList();
            if (inputData.HpId < 0)
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidHpId, new(), receptionInfos, sameVisitList);
            }
            if (inputData.UserId <= 0)
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidUserId, new(), receptionInfos, sameVisitList);
            }
            if (raiinNos.Count < 1)
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidRaiinNo, new(), receptionInfos, sameVisitList);
            }
            if (!_raiinInfRepository.CheckExistOfRaiinNos(raiinNos.Select(r => r.RaiinNo).ToList()))
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidRaiinNo, new(), receptionInfos, sameVisitList);
            }
            var result = _raiinInfRepository.Delete(inputData.Flag, inputData.HpId, inputData.PtId, inputData.UserId, inputData.SinDate, raiinNos.Select(r => new Tuple<long, long, int>(r.RaiinNo, r.OyaRaiinNo, r.Status)).ToList());
            if (result.Any())
            {
                Task.Run(() =>
                {
                    _calculateService.RunCalculate(new RecaculationInputDto(inputData.HpId, inputData.PtId, inputData.SinDate, 0, "DR_"));
                });
                //Item1: SinDate, Item2: RaiinNo, Item3: PtId
                receptionInfos.Add(new ReceptionRowModel(result.First().Item2, result.First().Item3, result.First().Item1, 1));
                sameVisitList = _receptionRepository.GetListSameVisit(inputData.HpId, inputData.PtId, inputData.SinDate);
                return new DeleteReceptionOutputData(DeleteReceptionStatus.Successed, result.Select(r => new DeleteReceptionItem(r.Item1, r.Item2, r.Item3)).ToList(), receptionInfos, sameVisitList);
            }

            return new DeleteReceptionOutputData(DeleteReceptionStatus.Failed, new(), receptionInfos, sameVisitList);
        }
        finally
        {
            _raiinInfRepository.ReleaseResource();
            _receptionRepository.ReleaseResource();
        }
    }
}