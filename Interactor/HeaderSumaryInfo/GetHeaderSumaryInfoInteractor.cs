using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.OrdInfs;
using Domain.Models.PtAlrgyDrug;
using Domain.Models.PtAlrgyElse;
using Domain.Models.PtAlrgyFood;
using Domain.Models.PtCmtInf;
using Domain.Models.PtInfection;
using Domain.Models.PtKioReki;
using Domain.Models.PtOtcDrug;
using Domain.Models.PtOtherDrug;
using Domain.Models.PtPregnancy;
using Domain.Models.PtSupple;
using Helper.Common;
using Helper.Enums;
using Helper.Extendsions;
using UseCase.HeaderSumaryInfo.Get;
using UseCase.OrdInfs.GetListTrees;

namespace Interactor.HeaderSumaryInfo
{
    public class GetHeaderSumaryInfoInteractor : IGetOrdInfListTreeInputPort
    {
        private readonly IPtAlrgyElseRepository _ptAlrgryElseRepository;
        private readonly IPtAlrgyFoodRepository _ptPtAlrgyFoodRepository;
        private readonly IPtAlrgyDrugRepository _ptPtAlrgyDrugRepository;
        private readonly IPtKioRekiRepository _ptKioRekiRepository;
        private readonly IPtInfectionRepository _ptInfectionRepository;
        private readonly IPtOtherDrugRepository _ptOtherDrugRepository;
        private readonly IPtOtcDrugRepository _ptPtOtcDrugRepository;
        private readonly IPtSuppleRepository _ptPtSuppleRepository;
        private readonly IPtPregnancyRepository _ptPregnancyRepository;
        private readonly IPtCmtInfRepository _ptCmtInfRepository;
        private readonly IInsuranceRepository _insuranceRepository;

        public GetHeaderSumaryInfoInteractor(IPtAlrgyElseRepository ptAlrgryElseRepository, IPtAlrgyFoodRepository ptPtAlrgyFoodRepository, IPtAlrgyDrugRepository ptPtAlrgyDrugRepository, IPtKioRekiRepository ptKioRekiRepository, IPtInfectionRepository ptInfectionRepository, IPtOtherDrugRepository ptOtherDrugRepository, IPtOtcDrugRepository ptPtOtcDrugRepository, IPtSuppleRepository ptPtSuppleRepository, IPtPregnancyRepository ptPregnancyRepository, IPtCmtInfRepository ptCmtInfRepository, IInsuranceRepository insuranceRepository)
        {
            _ptAlrgryElseRepository = ptAlrgryElseRepository;
            _ptPtAlrgyFoodRepository = ptPtAlrgyFoodRepository;
            _ptPtAlrgyDrugRepository = ptPtAlrgyDrugRepository;
            _ptKioRekiRepository = ptKioRekiRepository;
            _ptInfectionRepository = ptInfectionRepository;
            _ptOtherDrugRepository = ptOtherDrugRepository;
            _ptPtOtcDrugRepository = ptPtOtcDrugRepository;
            _ptPtSuppleRepository = ptPtSuppleRepository;
            _ptPregnancyRepository = ptPregnancyRepository;
            _ptCmtInfRepository = ptCmtInfRepository;
            _insuranceRepository = insuranceRepository;
        }

        public GetOrdInfListTreeOutputData Handle(GetOrdInfListTreeInputData inputData)
        {
            if (inputData.RaiinNo <= 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.InvalidRaiinNo);
            }
            if (inputData.HpId <= 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.InvalidHpId);
            }
            if (inputData.PtId <= 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.InvalidPtId);
            }
            if (inputData.SinDate <= 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.InvalidSinDate);
            }
            var allOdrInfs = _ordInfRepository
                    .GetList(inputData.PtId, inputData.RaiinNo, inputData.SinDate, inputData.IsDeleted)
                    .Select(o => new OdrInfItem(
                        o.HpId,
                        o.RaiinNo,
                        o.RpNo,
                        o.RpEdaNo,
                        o.PtId,
                        o.SinDate,
                        o.HokenPid,
                        o.OdrKouiKbn,
                        o.RpName,
                        o.InoutKbn,
                        o.SikyuKbn,
                        o.SyohoSbt,
                        o.SanteiKbn,
                        o.TosekiKbn,
                        o.DaysCnt,
                        o.SortNo,
                        o.Id,
                        o.GroupKoui.Value,
                        o.OrdInfDetails.Select(od => new OdrInfDetailItem(
                            od.HpId,
                            od.RaiinNo,
                            od.RpNo,
                            od.RpEdaNo,
                            od.RowNo,
                            od.PtId,
                            od.SinDate,
                            od.SinKouiKbn,
                            od.ItemCd,
                            od.ItemName,
                            od.Suryo,
                            od.UnitName,
                            od.UnitSbt,
                            od.TermVal,
                            od.KohatuKbn,
                            od.SyohoKbn,
                            od.SyohoLimitKbn,
                            od.DrugKbn,
                            od.YohoKbn,
                            od.Kokuji1,
                            od.Kokuji2,
                            od.IsNodspRece,
                            od.IpnCd,
                            od.IpnName,
                            od.JissiKbn,
                            od.JissiDate,
                            od.JissiId,
                            od.JissiMachine,
                            od.ReqCd,
                            od.Bunkatu,
                            od.CmtName,
                            od.CmtName,
                            od.FontColor,
                            od.CommentNewline
                        )).OrderBy(odrDetail => odrDetail.RpNo)
                        .ThenBy(odrDetail => odrDetail.RpEdaNo)
                        .ThenBy(odrDetail => odrDetail.RowNo)
                        .ToList()))
                    .OrderBy(odr => odr.OdrKouiKbn)
                    .ThenBy(odr => odr.RpNo)
                    .ThenBy(odr => odr.RpEdaNo)
                    .ThenBy(odr => odr.SortNo)
                    .ToList();

            var hokenOdrInfs = allOdrInfs
                .GroupBy(odr => odr.HokenPid)
                .Select(grp => grp.FirstOrDefault())
                .ToList();

            if (hokenOdrInfs == null || hokenOdrInfs.Count == 0)
            {
                return new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.NoData);
            }

            var tree = new GetOrdInfListTreeOutputData(new List<GroupHokenItem>(), GetOrdInfListTreeStatus.Successed);
            foreach (var hokenId in hokenOdrInfs.Select(h => h?.HokenPid))
            {
                var groupHoken = new GroupHokenItem(new List<GroupOdrItem>(), hokenId, "Hoken title ");
                // Find By Group
                var groupOdrInfs = allOdrInfs.Where(odr => odr.HokenPid == hokenId)
                    .GroupBy(odr => new
                    {
                        odr.HokenPid,
                        odr.GroupOdrKouiKbn,
                        odr.InoutKbn,
                        odr.SyohoSbt,
                        odr.SikyuKbn,
                        odr.TosekiKbn,
                        odr.SanteiKbn
                    })
                    .Select(grp => grp.FirstOrDefault())
                    .ToList();
                if (!(groupOdrInfs == null || groupOdrInfs.Count == 0))
                {
                    foreach (var groupOdrInf in groupOdrInfs)
                    {
                        var rpOdrInfs = allOdrInfs.Where(odrInf => odrInf.HokenPid == hokenId
                                                && odrInf.GroupOdrKouiKbn == groupOdrInf?.GroupOdrKouiKbn
                                                && odrInf.InoutKbn == groupOdrInf?.InoutKbn
                                                && odrInf.SyohoSbt == groupOdrInf?.SyohoSbt
                                                && odrInf.SikyuKbn == groupOdrInf?.SikyuKbn
                                                && odrInf.TosekiKbn == groupOdrInf?.TosekiKbn
                                                && odrInf.SanteiKbn == groupOdrInf?.SanteiKbn)
                                            .ToList();
                        var group = new GroupOdrItem("Hoken title", new List<OdrInfItem>(), hokenId);

                        foreach (OdrInfItem rpOdrInf in rpOdrInfs)
                        {
                            group.OdrInfs.Add(rpOdrInf);
                        }
                        groupHoken.GroupOdrItems.Add(group);
                    }
                }
                tree.GroupHokens.Add(groupHoken);
            }

            return tree;
        }

        private string GetSettingParam(int groupCd, int grpItemCd = 0, string defaultValue = "", bool fromLastestDb = false)
        {
            //Wait user config
            //UserConf userConf = new UserConf();
            //if (!fromLastestDb)
            //{
            //    userConf = _userConfigs.Where(p => p.GrpCd == groupCd && p.GrpItemCd == grpItemCd).FirstOrDefault();
            //}
            //else
            //{
            //    userConf = _userConfRepositoryAsync.ListAsQueryableAsync(p =>
            //        p.HpId == _HpId && p.GrpCd == groupCd && p.GrpItemCd == grpItemCd && p.UserId == _UserId, CancellationToken.None).Result.FirstOrDefault();
            //}
            //return userConf != null ? userConf.Param : defaultValue;
            return "";
        }

        private PtInfNotificationItem GetSummaryInfo(string propertyCd, int headerType, InfoType infoType = InfoType.PtHeaderInfo)
        {
            PtInfNotificationItem ptHeaderInfoModel = new PtInfNotificationItem()
            {
                PropertyColor = "000000" // default color
            };

            GetData(propertyCd, ref ptHeaderInfoModel);

            if (infoType == InfoType.PtHeaderInfo)
            {
                switch (propertyCd)
                {
                    case "C":
                        GetPhoneNumber(ptHeaderInfoModel);
                        //電話番号
                        break;
                    case "D":
                        GetReceptionComment(ptHeaderInfoModel);
                        //受付コメント
                        break;
                    case "E":
                        GetFamilyList(ptHeaderInfoModel);
                        //家族歴
                        break;
                }
            }
            else if (infoType == InfoType.SumaryInfo)
            {
                switch (propertyCd)
                {
                    case "C":
                        //"サマリー";
                        //ptHeaderInfoModel.GrpItemCd = 12;
                        //ptHeaderInfoModel.HeaderName = "◆サマリー";
                        //if (_IsGetDataFromSpecialNote)
                        //{
                        //    SummaryInfModel = SummaryInfModelSp;
                        //}
                        //else
                        //{
                        //    SummaryInfModel = _specialNoteFinder.GetSummaryInf(_ptId);
                        //    if (SummaryInfModel != null && !string.IsNullOrEmpty(SummaryInfModel.Text))
                        //    {
                        //        ptHeaderInfoModel.HeaderInfo = SummaryInfModel.Text;
                        //    }
                        //}
                        break;
                    case "D":
                        // "電話番号";
                        GetPhoneNumber(ptHeaderInfoModel);
                        ptHeaderInfoModel.GrpItemCd = 13;
                        break;
                    case "E":
                        //"受付コメント";
                        GetReceptionComment(ptHeaderInfoModel);
                        ptHeaderInfoModel.GrpItemCd = 14;
                        break;
                    case "F":
                        GetFamilyList(ptHeaderInfoModel);
                        ptHeaderInfoModel.GrpItemCd = 15;
                        //家族歴
                        break;
                }
            }

            return ptHeaderInfoModel;
        }

        private void GetData(string propertyCd, ref PtInfNotificationItem ptHeaderInfoModel)
        {
            switch (propertyCd)
            {
                case "1":
                    //身体情報
                    //GetPhysicalInfo(ptHeaderInfoModel);
                    break;
                case "2":
                    //アレルギー 
                    GetDrugInfo(ptHeaderInfoModel);
                    break;
                case "3":
                    // 病歴
                    GetPathologicalStatus(ptHeaderInfoModel);
                    break;
                case "4":
                    // 服薬情報
                    GetInteraction(ptHeaderInfoModel);
                    break;
                case "5":
                    //算定情報
                    GetCalculationInfo(ptHeaderInfoModel);
                    break;
                case "6":
                    //出産予定
                    GetReproductionInfo(ptHeaderInfoModel);
                    break;
                case "7":
                    //予約情報
                    GetReservationInf(ptHeaderInfoModel);
                    break;
                case "8":
                    //コメント
                    GetComment(ptHeaderInfoModel);
                    break;
                case "9":
                    //住所
                    GetAddress(ptHeaderInfoModel);
                    break;
                case "A":
                    //保険情報
                    GetInsuranceInfo(ptHeaderInfoModel);
                    break;
                case "B":
                    //生活歴
                    GetLifeHistory(ptHeaderInfoModel);
                    break;
            }
        }

        private void GetDrugInfo(long ptId, int sinDate, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 2;
            ptHeaderInfoModel.HeaderName = "◆アレルギー";

            var listPtAlrgyElseItem = _ptAlrgryElseRepository.GetList(ptId).Select(p => new PtAlrgyElseItem(p)).Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate).OrderBy(p => p.PtAlrgyElse.SortNo).ToList();
            var listPtAlrgyFoodItem = _ptPtAlrgyFoodRepository.GetList(ptId).Select(p => new PtAlrgyFoodItem(p)).Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate).OrderBy(p => p.PtAlrgyFood.SortNo).ToList();
            var listPtAlrgyDrugItem = _ptPtAlrgyDrugRepository.GetList(ptId).Select(p => new PtAlrgyDrugItem(p)).Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate).OrderBy(p => p.PtAlrgyDrug.SortNo).ToList();

            foreach (var ptAlrgyDrugModel in listPtAlrgyDrugItem)
            {
                if (!string.IsNullOrEmpty(ptAlrgyDrugModel?.DrugName))
                {
                    ptHeaderInfoModel.HeaderInfo += ptAlrgyDrugModel.DrugName;
                    if (!string.IsNullOrEmpty(ptAlrgyDrugModel.Cmt))
                    {
                        ptHeaderInfoModel.HeaderInfo += "／" + ptAlrgyDrugModel.Cmt;
                    }
                    ptHeaderInfoModel.HeaderInfo += Environment.NewLine;
                }
            }

            foreach (var ptAlrgyFoodModel in listPtAlrgyFoodItem)
            {
                if (!string.IsNullOrEmpty(ptAlrgyFoodModel.FoodName))
                {
                    ptHeaderInfoModel.HeaderInfo += ptAlrgyFoodModel.FoodName;
                    if (!string.IsNullOrEmpty(ptAlrgyFoodModel.Cmt))
                    {
                        ptHeaderInfoModel.HeaderInfo += "／" + ptAlrgyFoodModel.Cmt;
                    }
                    ptHeaderInfoModel.HeaderInfo += Environment.NewLine;
                }
            }

            foreach (var ptAlrgyElseModel in listPtAlrgyElseItem)
            {
                if (!string.IsNullOrEmpty(ptAlrgyElseModel.AlrgyName))
                {
                    ptHeaderInfoModel.HeaderInfo += ptAlrgyElseModel.AlrgyName;
                    if (!string.IsNullOrEmpty(ptAlrgyElseModel.Cmt))
                    {
                        ptHeaderInfoModel.HeaderInfo += "／" + ptAlrgyElseModel.Cmt;
                    }
                    ptHeaderInfoModel.HeaderInfo += Environment.NewLine;
                }
            }

            ptHeaderInfoModel.HeaderInfo = ptHeaderInfoModel.HeaderInfo?.TrimEnd(Environment.NewLine.ToCharArray());
        }

        private void GetPathologicalStatus(long ptId, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 3;
            ptHeaderInfoModel.HeaderName = "◆病歴";
            var listPtKioRekiItem = _ptKioRekiRepository.GetList(ptId);
            var listPtInfectionItem = _ptInfectionRepository.GetList(ptId);
            foreach (var ptKioRekiModel in listPtKioRekiItem)
            {
                if (!string.IsNullOrEmpty(ptKioRekiModel.Byomei))
                {
                    ptHeaderInfoModel.HeaderInfo += ptKioRekiModel.Byomei;
                    if (!string.IsNullOrEmpty(ptKioRekiModel.Cmt))
                    {
                        ptHeaderInfoModel.HeaderInfo += "／" + ptKioRekiModel.Cmt;
                    }
                    ptHeaderInfoModel.HeaderInfo += Environment.NewLine;
                }
            }
            foreach (var ptInfectionModel in listPtInfectionItem)
            {
                if (!string.IsNullOrEmpty(ptInfectionModel.Byomei))
                {
                    ptHeaderInfoModel.HeaderInfo += ptInfectionModel.Byomei;
                    if (!string.IsNullOrEmpty(ptInfectionModel.Cmt))
                    {
                        ptHeaderInfoModel.HeaderInfo += "／" + ptInfectionModel.Cmt;
                    }
                    ptHeaderInfoModel.HeaderInfo += Environment.NewLine;
                }
            }
            ptHeaderInfoModel.HeaderInfo = ptHeaderInfoModel.HeaderInfo?.TrimEnd(Environment.NewLine.ToCharArray());
        }

        private void GetInteraction(long ptId, int sinDate, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 4;
            ptHeaderInfoModel.HeaderName = "◆服薬情報";

            var listPtOtherDrugItem = _ptOtherDrugRepository.GetList(ptId).Select(p => new PtOtherDrugItem(p)).Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate).OrderBy(p => p.PtOtherDrug.SortNo).ToList();
            var listPtOtcDrugItem = _ptPtOtcDrugRepository.GetList(ptId).Select(p => new PtOtcDrugItem(p)).Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate)
                    .OrderBy(p => p.PtOtcDrug.SortNo);
            var listPtSuppleModel = _ptPtSuppleRepository.GetList(ptId).Select(p => new PtSuppleItem(p)).Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate)
                    .OrderBy(p => p.PtSupple.SortNo); ;

            foreach (var ptOtherDrugModel in listPtOtherDrugItem)
            {
                if (!string.IsNullOrEmpty(ptOtherDrugModel.DrugName))
                {
                    ptHeaderInfoModel.HeaderInfo += ptOtherDrugModel.DrugName;
                    if (!string.IsNullOrEmpty(ptOtherDrugModel.Cmt))
                    {
                        ptHeaderInfoModel.HeaderInfo += "／" + ptOtherDrugModel.Cmt;
                    }
                    ptHeaderInfoModel.HeaderInfo += Environment.NewLine;
                }
            }
            foreach (var ptOtcDrugModel in listPtOtcDrugItem)
            {
                if (!string.IsNullOrEmpty(ptOtcDrugModel.TradeName))
                {
                    ptHeaderInfoModel.HeaderInfo += ptOtcDrugModel.TradeName;
                    if (!string.IsNullOrEmpty(ptOtcDrugModel.Cmt))
                    {
                        ptHeaderInfoModel.HeaderInfo += "／" + ptOtcDrugModel.Cmt;
                    }
                    ptHeaderInfoModel.HeaderInfo += Environment.NewLine;
                }
            }
            foreach (var suppleModel in listPtSuppleModel)
            {
                if (!string.IsNullOrEmpty(suppleModel.IndexWord))
                {
                    ptHeaderInfoModel.HeaderInfo += suppleModel.IndexWord;
                    if (!string.IsNullOrEmpty(suppleModel.Cmt))
                    {
                        ptHeaderInfoModel.HeaderInfo += "／" + suppleModel.Cmt;
                    }
                    ptHeaderInfoModel.HeaderInfo += Environment.NewLine;
                }
            }
            ptHeaderInfoModel.HeaderInfo = ptHeaderInfoModel.HeaderInfo?.TrimEnd(Environment.NewLine.ToCharArray());
        }

        //private void GetCalculationInfo(PtInfNotificationItem ptHeaderInfoModel)
        //{
        //    ptHeaderInfoModel.GrpItemCd = 5;
        //    ptHeaderInfoModel.HeaderName = "◆算定情報";
        //    var listSanteiInfModels = GetCalculationInfo(this._PtId, this._SinDate).Result;
        //    if (listSanteiInfModels.Count > 0)
        //    {
        //        listSanteiInfModels = listSanteiInfModels.Where(u => u.DayCount > u.AlertDays).ToList();
        //        foreach (SanteiInfomationModel santeiInfomationModel in listSanteiInfModels)
        //        {
        //            ptHeaderInfoModel.HeaderInfo += santeiInfomationModel.ItemName?.Trim() + "(" + santeiInfomationModel.KisanType + " " + CIUtil.SDateToShowSDate(santeiInfomationModel.LastOdrDate) + "～　" + santeiInfomationModel.DayCountDisplay + ")" + Environment.NewLine;
        //        }
        //        ptHeaderInfoModel.HeaderInfo = ptHeaderInfoModel.HeaderInfo?.TrimEnd(Environment.NewLine.ToCharArray());
        //    }
        //}

        //public async Task<List<SanteiInfomationModel>> GetCalculationInfo(long ptId, int sinDate)
        //{
        //    List<int> listAletTermIsValid = new List<int>() { 2, 3, 4, 5, 6 };
        //    List<SanteiInfomationModel> result = new List<SanteiInfomationModel>();
        //    try
        //    {
        //        var santeiInfs = await _santeiInfRepositoryAsync.ListAsQueryableAsync(u => u.HpId == this._HpId &&
        //                                                                                     (u.PtId == ptId || u.PtId == 0) &&
        //                                                                                     u.AlertDays > 0 &&
        //                                                                                     listAletTermIsValid.Contains(u.AlertTerm), CancellationToken.None);
        //        var santeiInfDetails = await _santeiInfDetailRepositoryAsync.ListAsQueryableAsync(u => u.HpId == this._HpId &&
        //                                                                                                 u.PtId == ptId &&
        //                                                                                                 u.KisanDate > 0 &&
        //                                                                                                 u.EndDate >= sinDate &&
        //                                                                                                 u.IsDeleted == 0, CancellationToken.None);
        //        var tenMsts = await _tenMstRepositoryAsync.ListAsQueryableAsync(u => u.HpId == this._HpId &&
        //                                                                               u.StartDate <= this._SinDate &&
        //                                                                               u.EndDate >= this._SinDate, CancellationToken.None);

        //        // Query Santei inf code
        //        var kensaTenMst = await _tenMstRepositoryAsync.ListAsQueryableAsync(e => e.HpId == this._HpId
        //                                                                                && e.StartDate <= sinDate
        //                                                                                && e.EndDate >= sinDate, CancellationToken.None);

        //        var tenMstList = from santeiInf in santeiInfs
        //                         join tenMst in kensaTenMst on santeiInf.ItemCd
        //                                                equals tenMst.SanteiItemCd into tenMstLeft
        //                         from tenMst in tenMstLeft.DefaultIfEmpty()
        //                         select new
        //                         {
        //                             SanteiCd = santeiInf.ItemCd,
        //                             ItemCd = tenMst.ItemCd ?? santeiInf.ItemCd
        //                         };


        //        var odrInfs = await _odrInfRepositoryAsync.ListAsQueryableAsync(u => u.HpId == this._HpId &&
        //                                                                              u.PtId == ptId &&
        //                                                                              u.SinDate < sinDate &&
        //                                                                              u.IsDeleted == 0, CancellationToken.None);

        //        var odrInfDetails = await _odrInfDetailRepositoryAsync.ListAsQueryableAsync(u => u.HpId == this._HpId &&
        //                                                                                           u.PtId == ptId, CancellationToken.None);
        //        var listOdrInfs = from odrInfItem in odrInfs
        //                          join odrInfDetailItem in odrInfDetails on new { odrInfItem.RaiinNo, odrInfItem.RpEdaNo, odrInfItem.RpNo } equals
        //                                                                     new { odrInfDetailItem.RaiinNo, odrInfDetailItem.RpEdaNo, odrInfDetailItem.RpNo }
        //                          join tenMstItem in tenMstList on odrInfDetailItem.ItemCd equals tenMstItem.ItemCd
        //                          select new
        //                          {
        //                              tenMstItem.SanteiCd,
        //                              OdrInf = odrInfItem,
        //                              OdrInfDetail = odrInfDetailItem,
        //                          };

        //        //Get last oder day by ItemCd
        //        var listOrdInfomation = listOdrInfs.AsEnumerable().OrderByDescending(u => u.OdrInf.SinDate).GroupBy(o => o.SanteiCd).Select(g => g.First()).ToList(); //select distinct by ItemCd
        //        var listOrdDetailInfomation = listOrdInfomation.Select(o => new { o.OdrInfDetail, o.SanteiCd }).ToList(); // only select OdrDetailInfo 

        //        var santeiQuery = from santeiInfItem in santeiInfs
        //                          join tenMstItem in tenMsts on santeiInfItem.ItemCd equals tenMstItem.ItemCd
        //                          select new
        //                          {
        //                              SanteiInf = santeiInfItem,
        //                              SnteiInfDetail = santeiInfDetails.Where(c => c.ItemCd == santeiInfItem.ItemCd).OrderByDescending(u => u.KisanDate).FirstOrDefault(),
        //                              TenMst = tenMstItem
        //                          };
        //        result = santeiQuery.AsEnumerable().Select(u => new SanteiInfomationModel(u.SanteiInf, u.SnteiInfDetail, u.TenMst, listOrdDetailInfomation.Where(o => o.SanteiCd == u.SanteiInf.ItemCd).FirstOrDefault()?.OdrInfDetail, sinDate)).OrderBy(t => t.ItemCd).ToList();
        //        return result;
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return result;
        //}


        private void GetReproductionInfo(long ptId, int hpId, int sinDate, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 6;
            ptHeaderInfoModel.HeaderName = "■出産予定";

            var listPtPregnancyItems = _ptPregnancyRepository.GetList(ptId, hpId, sinDate).Select(p => new PtPregnancyItem(p)).ToList();
            if (listPtPregnancyItems.Count > 0)
            {
                string GetSDateFromDateTime(DateTime? dateTime)
                {
                    if (dateTime == null)
                    {
                        return string.Empty;
                    }
                    return CIUtil.SDateToShowSDate(CIUtil.DateTimeToInt((DateTime)dateTime));
                };

                var ptPregnancyModel = listPtPregnancyItems.FirstOrDefault();
                if (ptPregnancyModel?.PeriodDate != null)
                {
                    ptHeaderInfoModel.HeaderInfo += "月経日(" + GetSDateFromDateTime(ptPregnancyModel.PeriodDate) + ")" + " " + "/";
                }
                if (!string.IsNullOrEmpty(ptPregnancyModel?.PeriodWeek) && ptPregnancyModel.PeriodWeek != "0W0D")
                {
                    ptHeaderInfoModel.HeaderInfo += "妊娠週(" + ptPregnancyModel.PeriodWeek + ")" + " " + "/";
                }
                if (ptPregnancyModel?.PeriodDueDate != null)
                {
                    ptHeaderInfoModel.HeaderInfo += "予定日(" + GetSDateFromDateTime(ptPregnancyModel.PeriodDueDate) + ")" + " " + "/";
                }
                if (ptPregnancyModel?.OvulationDate != null)
                {
                    ptHeaderInfoModel.HeaderInfo += "排卵日(" + GetSDateFromDateTime(ptPregnancyModel.OvulationDate) + ")" + " " + "/";
                }
                if (!string.IsNullOrEmpty(ptPregnancyModel?.OvulationWeek) && ptPregnancyModel.OvulationWeek != "0W0D")
                {
                    ptHeaderInfoModel.HeaderInfo += "妊娠週(" + ptPregnancyModel.OvulationWeek + ")" + " " + "/";
                }
                if (ptPregnancyModel?.OvulationDueDate != null)
                {
                    ptHeaderInfoModel.HeaderInfo += "予定日(" + GetSDateFromDateTime(ptPregnancyModel.OvulationDueDate) + ")";
                }
                ptHeaderInfoModel.HeaderInfo = ptHeaderInfoModel.HeaderInfo.TrimEnd('/');
            }
        }

        //private void GetReservationInf(PtInfNotificationModel ptHeaderInfoModel)
        //{
        //    int today = DateTime.Now.ToString("yyyyMMdd").AsInteger();
        //    ptHeaderInfoModel.GrpItemCd = 7;
        //    ptHeaderInfoModel.HeaderName = "■予約情報";
        //    List<RsvInfModel> listRsvInfModel = _masterFinderService.GetRsvInfoByRsvInf(_HpId, _PtId, today).Result;
        //    List<RaiinInfModel> listRaiinInfModel = _masterFinderService.GetRsvInfoByRaiinInf(_HpId, _PtId, today).Result; ;
        //    listRaiinInfModel = listRaiinInfModel.Where(u => !listRsvInfModel.Any(r => r.RaiinNo == u.RaiinNo)).ToList();
        //    foreach (RaiinInfModel raiinInf in listRaiinInfModel)
        //    {
        //        listRsvInfModel.Add(new RsvInfModel(null, null, null, raiinInf));
        //    }

        //    if (listRsvInfModel.Count > 0)
        //    {
        //        listRsvInfModel = listRsvInfModel.OrderBy(u => u.SinDate).ToList();
        //        foreach (RsvInfModel rsvInfModel in listRsvInfModel)
        //        {
        //            if (rsvInfModel.RsvInf != null)
        //            {
        //                //formart for RsvInf
        //                string startTime = rsvInfModel.StartTime > 0 ? " " + CIUtil.TimeToShowTime(rsvInfModel.StartTime) + " " : " ";
        //                string rsvFrameName = string.IsNullOrEmpty(rsvInfModel.RsvFrameName) ? string.Empty : "[" + rsvInfModel.RsvFrameName + "]";
        //                ptHeaderInfoModel.HeaderInfo += CIUtil.SDateToShowSDate2(rsvInfModel.SinDate) + startTime + rsvInfModel.RsvGrpName + " " + rsvFrameName + Environment.NewLine;
        //            }
        //            else
        //            {
        //                //formart for raiinInf
        //                string kaName = string.IsNullOrEmpty(rsvInfModel.RaiinInfModel.KaSname) ? " " : " " + "[" + rsvInfModel.RaiinInfModel.KaSname + "]" + " ";
        //                ptHeaderInfoModel.HeaderInfo += CIUtil.SDateToShowSDate2(rsvInfModel.SinDate) + " "
        //                                                + FormatTime(rsvInfModel.RaiinInfModel.YoyakuTime)
        //                                                + kaName
        //                                                + rsvInfModel.RaiinInfModel.TantoName + " "
        //                                                + (!string.IsNullOrEmpty(rsvInfModel.RaiinInfModel.RaiinCmt) ? "(" + rsvInfModel.RaiinInfModel.RaiinCmt + ")" : string.Empty)
        //                                                + Environment.NewLine;
        //            }
        //        }
        //    }
        //    ptHeaderInfoModel.HeaderInfo = ptHeaderInfoModel.HeaderInfo?.TrimEnd(Environment.NewLine.ToCharArray());
        //}

        private void GetComment(long ptId, int hpId,PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 8;
            ptHeaderInfoModel.HeaderName = "■コメント";

            var ptCmtInfModel = this.GetPtCmtInfo(ptId, hpId);

            if (ptCmtInfModel != null && !string.IsNullOrEmpty(ptCmtInfModel.Text))
            {
                ptHeaderInfoModel.HeaderInfo += ptCmtInfModel.Text + Environment.NewLine;
            }
            ptHeaderInfoModel.HeaderInfo = ptHeaderInfoModel.HeaderInfo.TrimEnd(Environment.NewLine.ToCharArray());
        }

        private PtCmtInfModel? GetPtCmtInfo(long ptId, int hpId)
        {
            var result = _ptCmtInfRepository.GetList(ptId, hpId)
                              .FirstOrDefault();
            return result;
        }

        //private void GetAddress(PtInfNotificationItem ptHeaderInfoModel)
        //{
        //    ptHeaderInfoModel.GrpItemCd = 9;
        //    ptHeaderInfoModel.HeaderName = "◆住所";
        //    if (_ptInfModel != null && !string.IsNullOrEmpty(_ptInfModel.HomeAddress1 + _ptInfModel.HomeAddress2))
        //    {
        //        ptHeaderInfoModel.HeaderInfo = _ptInfModel.HomeAddress1 + " " + _ptInfModel.HomeAddress2;
        //    }
        //}

        private void GetInsuranceInfo(long ptId, int sinDate, int hpId, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 10;
            ptHeaderInfoModel.HeaderName = "◆保険情報";
            var listPtHokenInfoItem = _insuranceRepository.GetInsuranceListById(hpId, ptId, sinDate, 28).ToList();
            if (listPtHokenInfoItem?.Count == 0) return;
            string futanInfo = string.Empty;
            string kohiInf = string.Empty;

            if (listPtHokenInfoItem?.Count > 0)
            {
                foreach (var ptHokenInfoModel in listPtHokenInfoItem)
                {
                    kohiInf = string.Empty;
                    if (!ptHokenInfoModel.IsEmptyKohi1)
                    {
                        kohiInf += GetFutanInfo(ptHokenInfoModel.Kohi1Inf);
                    }
                    if (!ptHokenInfoModel.IsEmptyKohi2)
                    {
                        kohiInf += GetFutanInfo(ptHokenInfoModel.Kohi2Inf);
                    }
                    if (!ptHokenInfoModel.IsEmptyKohi3)
                    {
                        kohiInf += GetFutanInfo(ptHokenInfoModel.Kohi3Inf);
                    }
                    if (!ptHokenInfoModel.IsEmptyKohi4)
                    {
                        kohiInf += GetFutanInfo(ptHokenInfoModel.Kohi4Inf);
                    }
                    if (string.IsNullOrEmpty(kohiInf))
                    {
                        continue;
                    }
                    kohiInf = kohiInf?.TrimEnd();
                    kohiInf = kohiInf?.TrimEnd('　');
                    kohiInf = kohiInf?.TrimEnd(',');
                    futanInfo += ptHokenInfoModel.HokenPid.ToString().PadLeft(3, '0') + ". ";
                    futanInfo += kohiInf;
                    futanInfo += Environment.NewLine;
                }
            }
            futanInfo = futanInfo?.TrimEnd();
            if (!string.IsNullOrEmpty(futanInfo?.Trim()))
            {
                ptHeaderInfoModel.HeaderInfo = futanInfo;
            }
        }

        private string GetFutanInfo(KohiInfModel ptKohi)
        {
            HokenMstModel hokenMst = ptKohi.HokenMasterModel;
            int gokenGaku = ptKohi.GendoGaku;
            string futanInfo = string.Empty;

            if (!string.IsNullOrEmpty(ptKohi.FutansyaNo))
            {
                futanInfo += "[" + ptKohi.FutansyaNo + "]";
            }
            else
            {
                if (hokenMst == null)
                {
                    return string.Empty;
                }
                futanInfo += "[" + hokenMst.HoubetsuNumber + "]";
            }

            if (hokenMst == null && !string.IsNullOrEmpty(ptKohi.FutansyaNo))
            {
                return futanInfo + "," + " ";
            }
            if (hokenMst.FutanKbn == 0)
            {
                //負担なし
                futanInfo += "0円";
            }
            else
            {
                if (hokenMst.KaiLimitFutan > 0)
                {
                    if (hokenMst.DayLimitFutan <= 0 && hokenMst.MonthLimitFutan <= 0 && gokenGaku > 0)
                    {
                        futanInfo += gokenGaku.AsString() + "円/回・";
                    }
                    else
                    {
                        futanInfo += hokenMst.KaiLimitFutan.AsString() + "円/回・";
                    }
                }

                if (hokenMst.DayLimitFutan > 0)
                {
                    if (hokenMst.KaiLimitFutan <= 0 && hokenMst.MonthLimitFutan <= 0 && gokenGaku > 0)
                    {
                        futanInfo += gokenGaku.AsString() + "円/日・";
                    }
                    else
                    {
                        futanInfo += hokenMst.DayLimitFutan.AsString() + "円/日・";
                    }
                }

                if (hokenMst.DayLimitCount > 0)
                {
                    futanInfo = hokenMst.DayLimitCount.AsString() + "回/日・";
                }

                if (hokenMst.MonthLimitFutan > 0)
                {
                    if (hokenMst.KaiLimitFutan <= 0 && hokenMst.DayLimitFutan <= 0 && gokenGaku > 0)
                    {
                        futanInfo += gokenGaku.AsString() + "円/月・";
                    }
                    else
                    {
                        futanInfo += hokenMst.MonthLimitFutan.AsString() + "円/月・";
                    }
                }

                if (hokenMst.MonthLimitCount > 0)
                {
                    futanInfo += hokenMst.MonthLimitCount.AsString() + "回/月";
                }
            }
            if (!string.IsNullOrEmpty(futanInfo))
            {
                futanInfo = futanInfo.TrimEnd('・');
                futanInfo = futanInfo + "," + " ";
            }
            return futanInfo;
        }
    }
}
