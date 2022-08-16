using Domain.Models.KaMst;
using Domain.Models.KarteFilterDetail;
using Domain.Models.KarteFilterMst;
using Domain.Models.User;
using Helper.Constants;
using System.Linq;
using UseCase.KarteFilter.GetListKarteFilter;

namespace Interactor.KarteFilter;

public class GetKarteFilterMstsInteractor : IGetKarteFilterInputPort
{
    private readonly IKarteFilterDetailRepository _karteFilterDetailRepository;
    private readonly IKarteFilterMstRepository _karteFilterMstRepository;
    private readonly IKaMstRepository _kaMstRepository;
    private readonly IUserRepository _userRepository;

    private int _hpId = TempIdentity.HpId;
    private int _userId = TempIdentity.UserId;

    public GetKarteFilterMstsInteractor(IKarteFilterDetailRepository karteFilterDetailRepository, IKarteFilterMstRepository karteFilterMstRepository, IKaMstRepository kaMstRepository, IUserRepository userRepository)
    {
        _karteFilterDetailRepository = karteFilterDetailRepository;
        _karteFilterMstRepository = karteFilterMstRepository;
        _kaMstRepository = kaMstRepository;
        _userRepository = userRepository;
    }

    public GetKarteFilterOutputData Handle(GetKarteFilterInputData inputData)
    {
        // Check sindate
        if (inputData.sinDate > 99999999 || inputData.sinDate < 10000000)
        {
            return new GetKarteFilterOutputData(new List<GetKarteFilterMstModelOutputItem>(), GetKarteFilterStatus.InvalidSinDate);
        }

        try
        {
            // check list KarteFilterMsts
            var allKarteFilterMsts = _karteFilterMstRepository.GetList(_hpId, _userId);

            if (allKarteFilterMsts == null || allKarteFilterMsts.Count <= 0)
            {
                return new GetKarteFilterOutputData(new List<GetKarteFilterMstModelOutputItem>(), GetKarteFilterStatus.NoData);
            }
            var result = allKarteFilterMsts.AsEnumerable().Select(data => new GetKarteFilterMstModelOutputItem(
                                                data.karteFilterDetailModels
                                                .Select(item => new GetKarteFilterDetailOutputItem(
                                                    item.HpId,
                                                    item.UserId,
                                                    item.FilterId,
                                                    item.FilterItemCd,
                                                    item.FilterEdaNo,
                                                    item.Val,
                                                    false,
                                                    item.Param,
                                                    0,
                                                    String.Empty,
                                                    String.Empty,
                                                    0,
                                                    "",
                                                    0
                                                )).ToList(),
                                                data.HpId,
                                                data.UserId,
                                                data.FilterId,
                                                data.FilterName,
                                                data.FilterName,
                                                data.AutoApply,
                                                data.IsDeleted
                                            )).ToList();

            // get list KaMstModel and UserMstModel
            List<KaMstModel> kaMsts = _kaMstRepository.GetList();
            List<UserMstModel> userMsts = _userRepository.GetAll(inputData.sinDate, true);

            // update field to KarteFilterMstModel
            foreach (var model in result)
            {
                // update Ka information
                foreach (var kaMst in kaMsts)
                {
                    if (kaMst != null)
                    {
                        var karteFilterDetail = model.KarteFilterDetailModels.FirstOrDefault(k => k.FilterItemCd == 4 && k.FilterEdaNo == kaMst.KaId);

                        if (karteFilterDetail == null)
                        {
                            karteFilterDetail = new GetKarteFilterDetailOutputItem(
                                _hpId,
                                _userId,
                                model.FilterId,
                                4,
                                kaMst.KaId,
                                0,
                                false,
                                null,
                                kaMst.KaId,
                                kaMst.KaName,
                                kaMst.KaSname,
                                0,
                                "",
                                0
                            );
                            model.KarteFilterDetailModels.Add(karteFilterDetail);
                        }
                        else
                        {
                            karteFilterDetail = new GetKarteFilterDetailOutputItem(
                                 karteFilterDetail.HpId,
                                 karteFilterDetail.UserId,
                                 karteFilterDetail.FilterId,
                                 karteFilterDetail.FilterItemCd,
                                 karteFilterDetail.FilterEdaNo,
                                 karteFilterDetail.Val,
                                 karteFilterDetail.IsModifiedData,
                                 karteFilterDetail.Param,
                                 kaMst.KaId,
                                 kaMst.KaName,
                                 kaMst.KaSname,
                                 karteFilterDetail.UserKaId,
                                 karteFilterDetail.Sname,
                                 karteFilterDetail.UserSortNo
                             );
                        }
                    }
                }

                // update user information
                foreach (var userMst in userMsts)
                {
                    var KaMst = kaMsts.FirstOrDefault(k => k.KaId == userMst.KaId);
                    if (KaMst != null && userMst != null)
                    {
                        var KarteFilterDetail = model.KarteFilterDetailModels.FirstOrDefault(k => k.FilterItemCd == 2 && k.FilterEdaNo == userMst.UserId);
                        if (KarteFilterDetail == null)
                        {
                            KarteFilterDetail = new GetKarteFilterDetailOutputItem(
                                _hpId,
                                _userId,
                                model.FilterId,
                                2,
                                userMst.UserId,
                                0,
                                false,
                                null,
                                KaMst.KaId,
                                KaMst.KaName,
                                KaMst.KaSname,
                                userMst.UserId,
                                userMst.Sname,
                                userMst.SortNo
                            );
                            model.KarteFilterDetailModels.Add(KarteFilterDetail);
                        }
                        else
                        {
                            KarteFilterDetail = new GetKarteFilterDetailOutputItem(
                                KarteFilterDetail.HpId,
                                KarteFilterDetail.UserId,
                                KarteFilterDetail.FilterId,
                                KarteFilterDetail.FilterItemCd,
                                KarteFilterDetail.FilterEdaNo,
                                KarteFilterDetail.Val,
                                KarteFilterDetail.IsModifiedData,
                                KarteFilterDetail.Param,
                                KaMst.KaId,
                                KaMst.KaName,
                                KaMst.KaSname,
                                userMst.UserId,
                                userMst.Sname,
                                userMst.SortNo
                            );
                        }
                    }
                }
            }

            return new GetKarteFilterOutputData(result, GetKarteFilterStatus.Successed);
        }
        catch (Exception)
        {
            return new GetKarteFilterOutputData(new List<GetKarteFilterMstModelOutputItem>(), GetKarteFilterStatus.Error);
        }
    }
}
