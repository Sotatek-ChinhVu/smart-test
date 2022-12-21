using Domain.Models.KarteFilterMst;
using Helper.Constants;
using UseCase.KarteFilter.GetListKarteFilter;

namespace Interactor.KarteFilter;

public class GetKarteFilterMstsInteractor : IGetKarteFilterInputPort
{
    private readonly IKarteFilterMstRepository _karteFilterMstRepository;

    public GetKarteFilterMstsInteractor(IKarteFilterMstRepository karteFilterMstRepository)
    {
        _karteFilterMstRepository = karteFilterMstRepository;
    }

    public GetKarteFilterOutputData Handle(GetKarteFilterInputData inputData)
    {
        try
        {
            // check list KarteFilterMsts
            var allKarteFilterMsts = _karteFilterMstRepository.GetList(inputData.HpId, inputData.UserId);

            if (allKarteFilterMsts == null || allKarteFilterMsts.Count <= 0)
            {
                return new GetKarteFilterOutputData(GetKarteFilterStatus.NoData);
            }

            return new GetKarteFilterOutputData(allKarteFilterMsts.Select(item => new KarteFilterMstOutputItem(item)).ToList(), GetKarteFilterStatus.Successed);
        }
        catch (Exception)
        {
            return new GetKarteFilterOutputData(GetKarteFilterStatus.Error);
        }
    }
}
