using Domain.Models.UketukeSbtDayInf;
using UseCase.UketukeSbtDayInf.Upsert;

namespace Interactor.UketukeSbtDayInf;

public class UpsertUketukeSbtDayInfInteractor : IUpsertUketukeSbtDayInfInputPort
{
    private readonly IUketukeSbtDayInfRepository _uketukeSbtDayInfRepository;

    public UpsertUketukeSbtDayInfInteractor(IUketukeSbtDayInfRepository uketukeSbtDayInfRepository)
    {
        _uketukeSbtDayInfRepository = uketukeSbtDayInfRepository;
    }

    public UpsertUketukeSbtDayInfOutputData Handle(UpsertUketukeSbtDayInfInputData input)
    {
        if (input.SinDate <= 0)
        {
            return new UpsertUketukeSbtDayInfOutputData(UpsertUketukeSbtDayInfStatus.InvalidSinDate);
        }

        _uketukeSbtDayInfRepository.Upsert(input.SinDate, input.UketukeSbt, input.SeqNo);
        return new UpsertUketukeSbtDayInfOutputData(UpsertUketukeSbtDayInfStatus.Success);
    }
}
