using Domain.Models.KaMst;
using Domain.Models.KarteFilterDetail;
using Domain.Models.KarteFilterMst;
using Domain.Models.User;
using Helper.Constants;
using UseCase.KarteFilter;

namespace Interactor.KarteFilter;

public class GetKarteFilterMstsInteractor : IKarteFilterInputPort
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

    public KarteFilterOutputData Handle(KarteFilterInputData inputData)
    {
        // Check sindate
        if (inputData.sinDate > 99999999 || inputData.sinDate < 10000000)
        {
            return new KarteFilterOutputData(new List<KarteFilterMstModelOutputItem>(), KarteFilterStatus.InvalidSinDate);
        }

        try
        {
            // check list KarteFilterMsts
            var AllKarteFilterMsts = _karteFilterMstRepository
                                            .GetList(_hpId, _userId);

            if (AllKarteFilterMsts == null || AllKarteFilterMsts.Count <= 0)
            {
                return new KarteFilterOutputData(new List<KarteFilterMstModelOutputItem>(), KarteFilterStatus.NoData);
            }

            // check list KarteFilterDetails
            var AllKarteFilterDetails = _karteFilterDetailRepository
                                            .GetList(_hpId, _userId)
                                            .Select(detail => new KarteFilterDetailOutputItem(
                                                detail.HpId,
                                                detail.UserId,
                                                detail.FilterId,
                                                detail.FilterItemCd,
                                                detail.FilterEdaNo,
                                                detail.Val,
                                                true,
                                                detail.Param,
                                                0,
                                                "",
                                                "",
                                                0,
                                                "",
                                                0
                                             ))
                                            .ToList();

            // get list KarteFilterMstModel
            var query = from mst in AllKarteFilterMsts
                        join detail in AllKarteFilterDetails on mst.FilterId equals detail.FilterId into details
                        select new
                        {
                            Mst = mst,
                            Details = details.ToList()
                        };

            var result = query.AsEnumerable().Select(data => new KarteFilterMstModelOutputItem(
                                                data.Details.ToList(),
                                                data.Mst.HpId,
                                                data.Mst.UserId,
                                                data.Mst.FilterId,
                                                data.Mst.FilterName,
                                                data.Mst.FilterName,
                                                data.Mst.AutoApply
                                            )).ToList();

            // get list KaMstModel and UserMstModel
            List<KaMstModel> KaMsts = _kaMstRepository.GetList();
            List<UserMstModel> UserMsts = _userRepository.GetAll(inputData.sinDate, true);

            // update field to KarteFilterMstModel
            foreach (var model in result)
            {

                // update Ka information
                foreach (KaMstModel KaMst in KaMsts)
                {
                    if (KaMst != null)
                    {
                        var KarteFilterDetail = model.KarteFilterDetailModels.FirstOrDefault(k => k.FilterItemCd == 4 && k.FilterEdaNo == KaMst.KaId);

                        if (KarteFilterDetail == null)
                        {
                            KarteFilterDetail = new KarteFilterDetailOutputItem(
                                _hpId,
                                _userId,
                                model.FilterId,
                                4,
                                KaMst.KaId,
                                0,
                                true,
                                null,
                                KaMst.KaId,
                                KaMst.KaName,
                                KaMst.KaSname,
                                0,
                                "",
                                0
                            );
                            model.KarteFilterDetailModels.Add(KarteFilterDetail);
                        }
                        else
                        {
                            KarteFilterDetail = new KarteFilterDetailOutputItem(
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
                                 KarteFilterDetail.UserKaId,
                                 KarteFilterDetail.Sname,
                                 KarteFilterDetail.UserSortNo
                             );
                        }
                    }
                }

                // update user information
                foreach (UserMstModel UserMst in UserMsts)
                {
                    var KaMst = KaMsts.FirstOrDefault(k => k.KaId == UserMst.KaId);
                    if (KaMst != null && UserMst != null)
                    {
                        var KarteFilterDetail = model.KarteFilterDetailModels.FirstOrDefault(k => k.FilterItemCd == 2 && k.FilterEdaNo == UserMst.UserId);
                        if (KarteFilterDetail == null)
                        {
                            KarteFilterDetail = new KarteFilterDetailOutputItem(
                                _hpId,
                                _userId,
                                model.FilterId,
                                2,
                                UserMst.UserId,
                                0,
                                true,
                                null,
                                KaMst.KaId,
                                KaMst.KaName,
                                KaMst.KaSname,
                                UserMst.UserId,
                                UserMst.Sname,
                                UserMst.SortNo
                            );
                            model.KarteFilterDetailModels.Add(KarteFilterDetail);
                        }
                        else
                        {
                            KarteFilterDetail = new KarteFilterDetailOutputItem(
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
                                UserMst.UserId,
                                UserMst.Sname,
                                UserMst.SortNo
                            );
                        }
                    }
                }
            }

            return new KarteFilterOutputData(result, KarteFilterStatus.Successed);
        }
        catch (Exception)
        {
            return new KarteFilterOutputData(new List<KarteFilterMstModelOutputItem>(), KarteFilterStatus.Error);
        }
    }
}
