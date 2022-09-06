using Domain.Models.Reception;
using UseCase.Reception.GetList;

namespace Interactor.Reception;

public class GetReceptionListInteractor : IGetReceptionListInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public GetReceptionListInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public GetReceptionListOutputData Handle(GetReceptionListInputData inputData)
    {
        if (inputData.HpId <= 0)
        {
            return new GetReceptionListOutputData(GetReceptionListStatus.InvalidHpId);
        }
        if (inputData.SinDate <= 0)
        {
            return new GetReceptionListOutputData(GetReceptionListStatus.InvalidSinDate);
        }

        var receptionInfos = _receptionRepository.GetList(inputData.HpId, inputData.SinDate, inputData.RaiinNo, inputData.PtId);
        CheckNameDuplicate(receptionInfos);
        return new GetReceptionListOutputData(GetReceptionListStatus.Success, receptionInfos);
    }

    private void CheckNameDuplicate(List<ReceptionRowModel> receptionInfos)
    {
        foreach (var info in receptionInfos)
        {
            var duplicate = receptionInfos.Find(r => r.KanaName == info.KanaName && r.PtId != info.PtId);
            info.IsNameDuplicate = duplicate is not null;
        }
    }
}
