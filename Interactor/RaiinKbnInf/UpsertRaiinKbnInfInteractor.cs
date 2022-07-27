using Domain.Models.RaiinKbnInf;
using UseCase.RaiinKbnInf.Upsert;

namespace Interactor.RaiinKbnInf;

public class UpsertRaiinKbnInfInteractor : IUpsertRaiinKbnInfInputPort
{
    private readonly IRaiinKbnInfRepository _raiinKbnInfRepository;

    public UpsertRaiinKbnInfInteractor(IRaiinKbnInfRepository raiinKbnInfRepository)
    {
        _raiinKbnInfRepository = raiinKbnInfRepository;
    }

    public UpsertRaiinKbnInfOutputData Handle(UpsertRaiinKbnInfInputData input)
    {
        if (input.HpId <= 0)
        {
            return new UpsertRaiinKbnInfOutputData($"{nameof(input.HpId)} must be greater than 0.");
        }
        if (input.SinDate <= 0)
        {
            return new UpsertRaiinKbnInfOutputData($"{nameof(input.SinDate)} must be greater than 0.");
        }
        if (input.RaiinNo <= 0)
        {
            return new UpsertRaiinKbnInfOutputData($"{nameof(input.RaiinNo)} must be greater than 0.");
        }
        if (input.PtId <= 0)
        {
            return new UpsertRaiinKbnInfOutputData($"{nameof(input.PtId)} must be greater than 0.");
        }
        if (input.GrpId < 0)
        {
            return new UpsertRaiinKbnInfOutputData($"{nameof(input.GrpId)} cannot be negative.");
        }
        if (input.KbnCd < 0)
        {
            return new UpsertRaiinKbnInfOutputData($"{nameof(input.KbnCd)} cannot be negative.");
        }

        _raiinKbnInfRepository.Upsert(input.HpId, input.PtId, input.SinDate, input.RaiinNo, input.GrpId, input.KbnCd);
        return new UpsertRaiinKbnInfOutputData(true);
    }
}
