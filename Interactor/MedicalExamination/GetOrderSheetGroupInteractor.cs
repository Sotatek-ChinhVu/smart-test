using Domain.Models.OrdInfs;
using Domain.Models.UserConf;
using Helper.Common;
using UseCase.MedicalExamination.GetOrderSheetGroup;

namespace Interactor.MedicalExamination
{
    public class GetOrderSheetGroupInteractor : IGetOrderSheetGroupInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IUserConfRepository _userConfRepository;

        public GetOrderSheetGroupInteractor(IOrdInfRepository ordInfRepository, IUserConfRepository userConfRepository)
        {
            _ordInfRepository = ordInfRepository;
            _userConfRepository = userConfRepository;
        }
        public GetOrderSheetGroupOutputData Handle(GetOrderSheetGroupInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetOrderSheetGroupOutputData(GetOrderSheetGroupStatus.InvalidPtId, new());
                }
                if (inputData.PtId <= 0)
                {
                    return new GetOrderSheetGroupOutputData(GetOrderSheetGroupStatus.InvalidPtId, new());
                }
                if (inputData.UserId <= 0)
                {
                    return new GetOrderSheetGroupOutputData(GetOrderSheetGroupStatus.InvalidUserId, new());
                }

                var orderSheetItems = GetOdrSheet(inputData.HpId, inputData.UserId, inputData.PtId, inputData.SelectDefOnLoad);

                return new GetOrderSheetGroupOutputData(GetOrderSheetGroupStatus.Successed, orderSheetItems);
            }
            finally
            {
                _ordInfRepository.ReleaseResource();
            }
        }

        private List<OrderSheetItem> GetOdrSheet(int hpId, int userId, long ptId, bool selectDefOnLoad)
        {
            var odrInfs = _ordInfRepository.GetList(ptId, hpId)
                                  .OrderBy(p => p.OdrKouiKbn)
                                  .OrderByDescending(p => p.SinDate).ToList();
            var parrentGrps = new List<OrderSheetItem>();

            var settingValues = OdrSheetKouiKbnInVisible(hpId, userId);

            //Filter odr by settting
            foreach (var val in settingValues)
            {
                //(処方) その他 
                if (val == 7)
                {
                    odrInfs.RemoveAll(p => p.OdrKouiKbn == 100 || p.OdrKouiKbn == 101 ||
                    (p.OdrKouiKbn / 10 == 2 && p.OdrKouiKbn != 21 && p.OdrKouiKbn != 22 && p.OdrKouiKbn != 23));
                }
                //(検査) その他
                else if (val == 18)
                {
                    odrInfs.RemoveAll(p => p.OdrKouiKbn / 10 == 6 && p.OdrKouiKbn != 61 && p.OdrKouiKbn != 62);
                }
                //(その他)
                else if (val == 20)
                {
                    odrInfs.RemoveAll(p => p.OdrKouiKbn / 10 == 8);
                }
                else
                {
                    //child group
                    if (new List<int> { 4, 5, 6, 7, 9, 10, 11, 12, 16, 17, 18 }.Contains(val))
                    {
                        odrInfs.RemoveAll(p => OdrUtil.GetKouiKbnNmBySetting(val) == OdrUtil.GetChildOdrGrpKouiName(p.OdrKouiKbn));
                    }
                    //parrent group
                    else
                    {
                        odrInfs.RemoveAll(p => OdrUtil.GetKouiKbnNmBySetting(val) == OdrUtil.GetOdrGroupName(p.OdrKouiKbn));
                    }
                }
            }
            var odrKouiKbns = odrInfs.OrderBy(p => p.OdrKouiKbn).Select(p => p.OdrKouiKbn).Distinct();
            var odrSheetKouiKbnSelect = _userConfRepository.GetSettingValue(hpId, userId, 207);
            //Get group odrKouiKbn
            foreach (var odrKouiKbn in odrKouiKbns)
            {
                string parrentGrpName = OdrUtil.GetOdrGroupName(odrKouiKbn);
                //item comment 処方箋コメント、処方箋備考コメント
                if (odrKouiKbn == 100 || odrKouiKbn == 101)
                {
                    parrentGrpName = "処方";
                }
                if (parrentGrps.Any(p => p.NodeText == parrentGrpName))
                {
                    var grpOdr = parrentGrps.First(p => p.NodeText == parrentGrpName);
                    if (grpOdr.HasChild && !grpOdr.Childrens.Any(p => p.NodeText == OdrUtil.GetChildOdrGrpKouiName(odrKouiKbn)))
                    {
                        grpOdr = grpOdr.ChangeIsExpanded(true);
                        var childGrp = new OrderSheetItem(2, odrKouiKbn, OdrUtil.GetChildOdrGrpKouiName(odrKouiKbn));
                        grpOdr.Childrens.Add(childGrp);
                        if (selectDefOnLoad && IsDefSelectOdrKouiKbnBySetting(hpId, userId, childGrp))
                        {
                            childGrp = childGrp.ChangeIsSelected(true);
                            grpOdr = grpOdr.ChangeIsExpanded(true);
                        }
                    }
                    continue;
                }
                var parrentGrp = new OrderSheetItem(1, CIUtil.GetGroupKoui(odrKouiKbn), parrentGrpName);
                if (selectDefOnLoad && (odrSheetKouiKbnSelect == 0 || IsDefSelectOdrKouiKbnBySetting(hpId, userId, parrentGrp)))
                {
                    selectDefOnLoad = false;
                    parrentGrp = parrentGrp.ChangeSomeProperties(true, true);
                }
                string childGrpName = OdrUtil.GetChildOdrGrpKouiName(odrKouiKbn);
                if (!string.IsNullOrEmpty(childGrpName))
                {
                    parrentGrp = parrentGrp.ChangeIsExpanded(true);
                    var childGrp = new OrderSheetItem(2, odrKouiKbn, childGrpName);
                    parrentGrp.Childrens.Add(childGrp);
                    if (selectDefOnLoad && IsDefSelectOdrKouiKbnBySetting(hpId, userId, childGrp))
                    {
                        childGrp = childGrp.ChangeIsSelected(true);
                        parrentGrp = parrentGrp.ChangeIsExpanded(true);
                    }
                }
                parrentGrps.Add(parrentGrp);
            }

            //Get sindate
            foreach (var parrentGrp in parrentGrps)
            {
                if (parrentGrp.HasChild)
                {
                    //move 他注 or その他 group to final 30,34 and 60,64
                    var otherItem = parrentGrp.Childrens.FirstOrDefault(p => p.NodeText == "その他" || p.NodeText == "他注");
                    if (otherItem != null)
                    {
                        parrentGrp.Childrens.Remove(otherItem);
                        parrentGrp.Childrens.Add(otherItem);
                    }

                    foreach (var childGrp in parrentGrp.Childrens)
                    {
                        if (childGrp.IsSelected) childGrp.ChangeIsExpanded(true);
                        //自己注射 odrKouiKbn = 28 but groupKouiKbn = 20 -> その他 of (処方)
                        var sinDates = odrInfs.Where(p => childGrp.OdrKouiKbn == 60 ? (p.OdrKouiKbn == 60 || p.OdrKouiKbn == 64) :
                                                      ((childGrp.OdrKouiKbn == 20 || childGrp.OdrKouiKbn == 28) ? (p.OdrKouiKbn == 20 || p.OdrKouiKbn == 28 ||
                                                      p.OdrKouiKbn == 100 || p.OdrKouiKbn == 101) : p.OdrKouiKbn == childGrp.OdrKouiKbn))
                                                       .Select(p => p.SinDate).Distinct();
                        foreach (var sinDate in sinDates)
                        {
                            childGrp.Childrens.Add(new OrderSheetItem(3, sinDate, childGrp.OdrKouiKbn, CIUtil.SDateToShowSDate(sinDate)));
                        }
                    }
                }
                else
                {
                    if (odrSheetKouiKbnSelect == 0)
                    {
                        parrentGrp.ChangeIsExpanded(false);
                    }
                    IEnumerable<OrdInfModel> listOdrInf = Enumerable.Empty<OrdInfModel>();
                    if (parrentGrp.GroupKouiKbn == 14 ||
                        (parrentGrp.GroupKouiKbn >= 68 && parrentGrp.GroupKouiKbn < 70) ||
                        (parrentGrp.GroupKouiKbn >= 95 && parrentGrp.GroupKouiKbn < 99))
                    {
                        listOdrInf = odrInfs.Where(p => p.OdrKouiKbn == parrentGrp.GroupKouiKbn);
                    }
                    else
                    {
                        listOdrInf = odrInfs.Where(p => (parrentGrp.GroupKouiKbn == 20 ? (p.OdrKouiKbn / 10 == parrentGrp.GroupKouiKbn / 10 || p.OdrKouiKbn == 100 || p.OdrKouiKbn == 101) :
                                                      p.OdrKouiKbn / 10 == parrentGrp.GroupKouiKbn / 10) &&
                                                      p.OdrKouiKbn != 14 && !(p.OdrKouiKbn >= 68 && p.OdrKouiKbn < 70) && !(p.OdrKouiKbn >= 95 && p.OdrKouiKbn < 99));
                    }
                    foreach (var sinDate in listOdrInf.OrderByDescending(p => p.SinDate).Select(p => p.SinDate).Distinct())
                    {
                        parrentGrp.Childrens.Add(new OrderSheetItem(2, sinDate, parrentGrp.GroupKouiKbn, CIUtil.SDateToShowSDate(sinDate)));
                    }
                }
            }
            return parrentGrps;
        }

        private List<int> OdrSheetKouiKbnInVisible(int hpId, int userId)
        {
            var values = _userConfRepository.GetSettingValues(hpId, userId, 23, 0, 22);
            var result = values.FindAll(v => v.value == 0).Select(v => v.groupItemCd).ToList();
            return result;
        }

        private bool IsDefSelectOdrKouiKbnBySetting(int hpId, int userId, OrderSheetItem odrModel)
        {
            var settingVal = _userConfRepository.GetSettingValue(hpId, userId, 207);
            if (settingVal == 0) return false;

            string defSelectKbnNm = OdrUtil.GetKouiKbnNmBySetting(settingVal);
            if (defSelectKbnNm == "その他")
            {
                switch (settingVal)
                {
                    case 7:
                        return odrModel.Level == 2 && (odrModel.OdrKouiKbn == 100 ||
                            (odrModel.OdrKouiKbn / 10 == 2 && odrModel.OdrKouiKbn != 21 && odrModel.OdrKouiKbn != 22 && odrModel.OdrKouiKbn != 23));
                    case 18:
                        return odrModel.Level == 2 && odrModel.OdrKouiKbn / 10 == 6 &&
                               odrModel.OdrKouiKbn != 61 && odrModel.OdrKouiKbn != 62;
                    case 20:
                        return odrModel.GroupKouiKbn / 10 == 8;
                }
            }
            return defSelectKbnNm == odrModel.NodeText;
        }
    }
}
