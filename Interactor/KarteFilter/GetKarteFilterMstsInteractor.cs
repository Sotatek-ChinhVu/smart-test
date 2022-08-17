using Domain.Models.KaMst;
using Domain.Models.KarteFilterMst;
using Domain.Models.User;
using Helper.Constants;
using System.Linq;
using UseCase.KarteFilter.GetListKarteFilter;

namespace Interactor.KarteFilter;

public class GetKarteFilterMstsInteractor : IGetKarteFilterInputPort
{
    private readonly IKarteFilterMstRepository _karteFilterMstRepository;

    private int _hpId = TempIdentity.HpId;
    private int _userId = TempIdentity.UserId;

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
                return new GetKarteFilterOutputData(new List<GetKarteFilterMstModelOutputItem>(), GetKarteFilterStatus.NoData);
            }

            var result = new List<GetKarteFilterMstModelOutputItem>();
            foreach (var item in allKarteFilterMsts)
            {
                result.Add(new GetKarteFilterMstModelOutputItem(
                       new GetKarteFilterDetailOutputItem(
                            item.KarteFilterDetailModel.HpId,
                            item.KarteFilterDetailModel.UserId,
                            item.KarteFilterDetailModel.FilterId,
                            item.KarteFilterDetailModel.BookMarkChecked,
                            item.KarteFilterDetailModel.ListHokenId,
                            item.KarteFilterDetailModel.ListKaId,
                            item.KarteFilterDetailModel.ListUserId
                        ),
                        item.HpId,
                        item.UserId,
                        item.FilterId,
                        item.FilterName,
                        item.AutoApply,
                        item.IsDeleted
                    ));
            }
            return new GetKarteFilterOutputData(result, GetKarteFilterStatus.Successed);
        }
        catch (Exception)
        {
            return new GetKarteFilterOutputData(new List<GetKarteFilterMstModelOutputItem>(), GetKarteFilterStatus.Error);
        }
    }
}
