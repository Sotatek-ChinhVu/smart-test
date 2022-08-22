using Domain.Models.KarteFilterMst;
using Helper.Constants;
using UseCase.KarteFilter.GetListKarteFilter;

namespace Interactor.KarteFilter;

public class GetKarteFilterMstsInteractor : IGetKarteFilterInputPort
{
    private readonly IKarteFilterMstRepository _karteFilterMstRepository;
    private readonly int _hpId = TempIdentity.HpId;
    private readonly int _userId = TempIdentity.UserId;

    public GetKarteFilterMstsInteractor(IKarteFilterMstRepository karteFilterMstRepository)
    {
        _karteFilterMstRepository = karteFilterMstRepository;
    }

    public GetKarteFilterOutputData Handle(GetKarteFilterInputData inputData)
    {
        try
        {
            // check list KarteFilterMsts
            var allKarteFilterMsts = _karteFilterMstRepository.GetList(_hpId, _userId);

            if (allKarteFilterMsts == null || allKarteFilterMsts.Count <= 0)
            {
                return new GetKarteFilterOutputData(new(), GetKarteFilterStatus.NoData);
            }

            return new GetKarteFilterOutputData(allKarteFilterMsts, GetKarteFilterStatus.Successed);
        }
        catch (Exception)
        {
            return new GetKarteFilterOutputData(new(), GetKarteFilterStatus.Error);
        }
    }
}
