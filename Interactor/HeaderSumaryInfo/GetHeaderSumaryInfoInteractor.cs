using Domain.Models.Insurance;
using Domain.Models.KensaInfDetail;
using Domain.Models.KensaMst;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.PtAlrgyDrug;
using Domain.Models.PtAlrgyElse;
using Domain.Models.PtAlrgyFood;
using Domain.Models.PtCmtInf;
using Domain.Models.PtFamily;
using Domain.Models.PtFamilyReki;
using Domain.Models.PtInfection;
using Domain.Models.PtKioReki;
using Domain.Models.PtOtcDrug;
using Domain.Models.PtOtherDrug;
using Domain.Models.PtPregnancy;
using Domain.Models.PtSupple;
using Domain.Models.RaiinCmtInf;
using Domain.Models.Reception;
using Domain.Models.RsvFrameMst;
using Domain.Models.RsvGrpMst;
using Domain.Models.RsvInfo;
using Domain.Models.SanteiInfo;
using Domain.Models.SeikaturekiInf;
using Domain.Models.SummaryInf;
using Domain.Models.TenMst;
using Domain.Models.UserConfig;
using Helper.Common;
using Helper.Constants;
using Helper.Enums;
using Helper.Extendsions;
using System.Text;
using UseCase.HeaderSumaryInfo.Get;

namespace Interactor.HeaderSumaryInfo
{
    public class GetHeaderSumaryInfoInteractor : IGetHeaderSummaryInfoInputPort
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
        private readonly ISeikaturekiInfRepository _seikaturekiInRepository;
        private readonly IRaiinCmtInfRepository _raiinCmtInfRepository;
        private readonly IPatientInforRepository _patientInfRepository;
        private readonly IKensaInfDetailRepository _kensaInfDetailRepository;
        private readonly IKensaMstRepository _kensaMstRepository;
        private readonly ISummaryInfRepository _summaryInfRepository;
        private readonly IUserConfigRepository _userConfigRepository;
        private readonly IPtFamilyRepository _ptFamilyRepository;
        private readonly IPtFamilyRekiRepository _ptFamilyRekiRepository;
        private readonly ISanteiInfoRepository _santeiInfoRepository;
        private readonly ITenMstRepository _tenMstRepository;
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IRsvInfoRepository _rsvInfoRepository;
        private readonly IRsvFrameMstRepository _rsvFrameMstRepository;
        private readonly IRsvGrpMstRepository _rsvGrpMstRepository;
        private readonly IReceptionRepository _receptionRepository;
        private Dictionary<string, string> _relationship = new Dictionary<string, string>();

        public GetHeaderSumaryInfoInteractor(IPtAlrgyElseRepository ptAlrgryElseRepository, IPtAlrgyFoodRepository ptPtAlrgyFoodRepository, IPtAlrgyDrugRepository ptPtAlrgyDrugRepository, IPtKioRekiRepository ptKioRekiRepository, IPtInfectionRepository ptInfectionRepository, IPtOtherDrugRepository ptOtherDrugRepository, IPtOtcDrugRepository ptPtOtcDrugRepository, IPtSuppleRepository ptPtSuppleRepository, IPtPregnancyRepository ptPregnancyRepository, IPtCmtInfRepository ptCmtInfRepository, IInsuranceRepository insuranceRepository, ISeikaturekiInfRepository seikaturekiInRepository, IRaiinCmtInfRepository raiinCmtInfRepository, IPatientInforRepository patientInfRepository, IKensaInfDetailRepository kensaInfDetailRepository, IKensaMstRepository kensaMstRepository, ISummaryInfRepository summaryInfRepository, IUserConfigRepository userConfigRepository, IPtFamilyRepository ptFamilyRepository, IPtFamilyRekiRepository ptFamilyRekiRepository, ISanteiInfoRepository santeiInfoRepository, ITenMstRepository tenMstRepository, IOrdInfRepository ordInfRepository, IRsvInfoRepository rsvInfoRepository, IRsvFrameMstRepository rsvFrameMstRepository, IRsvGrpMstRepository rsvGrpMstRepository, IReceptionRepository receptionRepository)
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
            _seikaturekiInRepository = seikaturekiInRepository;
            _raiinCmtInfRepository = raiinCmtInfRepository;
            _patientInfRepository = patientInfRepository;
            _kensaInfDetailRepository = kensaInfDetailRepository;
            _kensaMstRepository = kensaMstRepository;
            _summaryInfRepository = summaryInfRepository;
            _userConfigRepository = userConfigRepository;
            _ptFamilyRekiRepository = ptFamilyRekiRepository;
            _ptFamilyRepository = ptFamilyRepository;
            _santeiInfoRepository = santeiInfoRepository;
            _tenMstRepository = tenMstRepository;
            _ordInfRepository = ordInfRepository;
            _rsvInfoRepository = rsvInfoRepository;
            _rsvFrameMstRepository = rsvFrameMstRepository;
            _rsvGrpMstRepository = rsvGrpMstRepository;
            _receptionRepository = receptionRepository;
        }

        public GetHeaderSumaryInfoOutputData Handle(GetHeaderSumaryInfoInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new GetHeaderSumaryInfoOutputData(null, null, null, GetHeaderSumaryInfoStatus.InvalidHpId);
            }
            if (inputData.PtId <= 0)
            {
                return new GetHeaderSumaryInfoOutputData(null, null, null, GetHeaderSumaryInfoStatus.InvalidPtId);
            }
            if (inputData.UserId <= 0)
            {
                return new GetHeaderSumaryInfoOutputData(null, null, null, GetHeaderSumaryInfoStatus.InvalidUserId);
            }
            if (inputData.SinDate <= 0)
            {
                return new GetHeaderSumaryInfoOutputData(null, null, null, GetHeaderSumaryInfoStatus.InvalidSinDate);
            }


            var _tempListHeader1InfoModels = new List<PtInfNotificationItem>();
            var _tempListHeader2InfoModels = new List<PtInfNotificationItem>();
            var _tempListNotification = new List<PtInfNotificationItem>();
            var infoType = InfoType.PtHeaderInfo; //temporaty hardcode

            string header1Property = string.Empty;
            string header2Property = string.Empty;

            var listUserconfig = new List<UserConfigModel>();
            if (infoType == InfoType.PtHeaderInfo)
            {
                header1Property = this.GetSettingParam(inputData.HpId, inputData.UserId, 910, defaultValue: "234");
                header2Property = this.GetSettingParam(inputData.HpId, inputData.UserId, 911, defaultValue: "567");
                listUserconfig = this.GetListUserConf(inputData.HpId, inputData.UserId, 912, true).ToList();
                _tempListNotification = GetNotification(inputData.UserId, inputData.HpId, inputData.PtId, inputData.SinDate);
            }
            else if (infoType == InfoType.SumaryInfo)
            {
                listUserconfig = this.GetListUserConf(inputData.HpId, inputData.UserId, 914, true);
            }

            foreach (char propertyCd in header1Property)
            {
                var ptHeaderInfoModel = GetSummaryInfo(inputData.PtId, inputData.HpId, inputData.RainNo, inputData.SinDate, propertyCd.AsString(), infoType);
                if (!string.IsNullOrEmpty(ptHeaderInfoModel.HeaderInfo))
                {
                    if (infoType == InfoType.PtHeaderInfo || infoType == InfoType.SumaryInfo)
                    {
                        _tempListHeader1InfoModels.Add(ptHeaderInfoModel);
                    }
                    else
                    {
                        _tempListHeader2InfoModels.Add(ptHeaderInfoModel);
                    }
                }
            }

            foreach (char propertyCd in header2Property)
            {
                var ptHeaderInfoModel = GetSummaryInfo(inputData.PtId, inputData.HpId, inputData.RainNo, inputData.SinDate, propertyCd.AsString(), infoType);
                if (!string.IsNullOrEmpty(ptHeaderInfoModel.HeaderInfo))
                {
                    _tempListHeader2InfoModels.Add(ptHeaderInfoModel);
                }
            }

            foreach (var userConfigurationModel in listUserconfig)
            {
                SetForeground(userConfigurationModel, _tempListHeader1InfoModels, _tempListHeader2InfoModels);
            }

            if (_tempListHeader1InfoModels == null && _tempListHeader2InfoModels == null && _tempListNotification == null)
            {
                return new GetHeaderSumaryInfoOutputData
                (
                    _tempListHeader1InfoModels,
                    _tempListHeader2InfoModels,
                    _tempListNotification,
                    GetHeaderSumaryInfoStatus.NoData
                );
            }

            return new GetHeaderSumaryInfoOutputData
            (
                _tempListHeader1InfoModels,
                _tempListHeader2InfoModels,
                _tempListNotification,
                GetHeaderSumaryInfoStatus.Successed
            );

        }

        private string GetSettingParam(int hpId, int userId, int groupCd, int grpItemCd = 0, string defaultValue = "", bool fromLastestDb = false)
        {
            //Wait user config
            if (!fromLastestDb)
            {
                var userConf = _userConfigRepository.GetList(groupCd, grpItemCd).FirstOrDefault(x => x.UserId == userId);
                return userConf != null ? userConf.Param : defaultValue;
            }
            else
            {
                var userConf = _userConfigRepository.GetList(hpId, groupCd, grpItemCd, userId).FirstOrDefault();
                return userConf != null ? userConf.Param : defaultValue;
            }
        }

        private PtInfNotificationItem GetSummaryInfo(long ptId, int hpId, long raiinNo, int sinDate, string propertyCd, InfoType infoType = InfoType.PtHeaderInfo)
        {
            PtInfNotificationItem ptHeaderInfoModel = new PtInfNotificationItem()
            {
                PropertyColor = "000000" // default color
            };

            GetData(hpId, ptId, sinDate, propertyCd, ref ptHeaderInfoModel);

            if (infoType == InfoType.PtHeaderInfo)
            {
                switch (propertyCd)
                {
                    case "C":
                        GetPhoneNumber(hpId, ptId, ptHeaderInfoModel);
                        //電話番号
                        break;
                    case "D":
                        GetReceptionComment(ptId, hpId, raiinNo, sinDate, ptHeaderInfoModel);
                        //受付コメント
                        break;
                    case "E":
                        GetFamilyList(ptId, hpId, ptHeaderInfoModel);
                        //家族歴
                        break;
                }
            }
            else if (infoType == InfoType.SumaryInfo)
            {
                switch (propertyCd)
                {
                    case "C":
                        //サマリー
                        ptHeaderInfoModel.GrpItemCd = 12;
                        ptHeaderInfoModel.HeaderName = "◆サマリー";

                        var summaryInf = GetSummaryInf(hpId, ptId);
                        if (summaryInf != null && !string.IsNullOrEmpty(summaryInf.Text))
                        {
                            ptHeaderInfoModel.HeaderInfo = summaryInf.Text;
                        }
                        break;
                    case "D":
                        //電話番号
                        GetPhoneNumber(hpId, ptId, ptHeaderInfoModel);
                        ptHeaderInfoModel.GrpItemCd = 13;
                        break;
                    case "E":
                        //受付コメント
                        GetReceptionComment(ptId, hpId, raiinNo, sinDate, ptHeaderInfoModel);
                        ptHeaderInfoModel.GrpItemCd = 14;
                        break;
                    case "F":
                        GetFamilyList(ptId, hpId, ptHeaderInfoModel);
                        ptHeaderInfoModel.GrpItemCd = 15;
                        //家族歴
                        break;
                }
            }

            return ptHeaderInfoModel;
        }

        private void GetData(int hpId, long ptId, int sinDate, string propertyCd, ref PtInfNotificationItem ptHeaderInfoModel)
        {
            switch (propertyCd)
            {
                case "1":
                    //身体情報
                    GetPhysicalInfo(hpId, ptId, sinDate, ptHeaderInfoModel);
                    break;
                case "2":
                    //アレルギー 
                    GetDrugInfo(ptId, sinDate, ptHeaderInfoModel);
                    break;
                case "3":
                    // 病歴
                    GetPathologicalStatus(ptId, ptHeaderInfoModel);
                    break;
                case "4":
                    // 服薬情報
                    GetInteraction(ptId, sinDate, ptHeaderInfoModel);
                    break;
                case "5":
                    //算定情報
                    GetCalculationInfo(ptId, hpId, sinDate, ptHeaderInfoModel);
                    break;
                case "6":
                    //出産予定
                    GetReproductionInfo(ptId, hpId, sinDate, ptHeaderInfoModel);
                    break;
                case "7":
                    //予約情報
                    GetReservationInf(hpId, ptId, ptHeaderInfoModel);
                    break;
                case "8":
                    //コメント
                    GetComment(ptId, hpId, ptHeaderInfoModel);
                    break;
                case "9":
                    //住所
                    GetAddress(hpId, ptId, ptHeaderInfoModel);
                    break;
                case "A":
                    //保険情報
                    GetInsuranceInfo(ptId, sinDate, hpId, ptHeaderInfoModel);
                    break;
                case "B":
                    //生活歴
                    GetLifeHistory(ptId, hpId, ptHeaderInfoModel);
                    break;
            }
        }

        private void GetDrugInfo(long ptId, int sinDate, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 2;
            ptHeaderInfoModel.HeaderName = "◆アレルギー";
            var strHeaderInfo = new StringBuilder();

            var listPtAlrgyElseItem = _ptAlrgryElseRepository.GetList(ptId).Select(p => new PtAlrgyElseItem(p)).Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate).OrderBy(p => p.PtAlrgyElse.SortNo).ToList();
            var listPtAlrgyFoodItem = _ptPtAlrgyFoodRepository.GetList(ptId).Select(p => new PtAlrgyFoodItem(p)).Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate).OrderBy(p => p.PtAlrgyFood.SortNo).ToList();
            var listPtAlrgyDrugItem = _ptPtAlrgyDrugRepository.GetList(ptId).Select(p => new PtAlrgyDrugItem(p)).Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate).OrderBy(p => p.PtAlrgyDrug.SortNo).ToList();


            foreach (var ptAlrgyDrugModel in listPtAlrgyDrugItem)
            {
                if (!string.IsNullOrEmpty(ptAlrgyDrugModel?.DrugName))
                {
                    strHeaderInfo.Append(ptAlrgyDrugModel.DrugName);
                    if (!string.IsNullOrEmpty(ptAlrgyDrugModel.Cmt))
                    {
                        strHeaderInfo.Append("／" + ptAlrgyDrugModel.Cmt);
                    }
                    strHeaderInfo.Append(Environment.NewLine);
                }
            }

            foreach (var ptAlrgyFoodModel in listPtAlrgyFoodItem)
            {
                if (!string.IsNullOrEmpty(ptAlrgyFoodModel.FoodName))
                {
                    strHeaderInfo.Append(ptAlrgyFoodModel.FoodName);
                    if (!string.IsNullOrEmpty(ptAlrgyFoodModel.Cmt))
                    {
                        strHeaderInfo.Append("／" + ptAlrgyFoodModel.Cmt);
                    }
                    strHeaderInfo.Append(Environment.NewLine);
                }
            }

            foreach (var ptAlrgyElseModel in listPtAlrgyElseItem)
            {
                if (!string.IsNullOrEmpty(ptAlrgyElseModel.AlrgyName))
                {
                    strHeaderInfo.Append(ptAlrgyElseModel.AlrgyName);
                    if (!string.IsNullOrEmpty(ptAlrgyElseModel.Cmt))
                    {
                        strHeaderInfo.Append("／" + ptAlrgyElseModel.Cmt);
                    }
                    strHeaderInfo.Append(Environment.NewLine);
                }
            }

            ptHeaderInfoModel.HeaderInfo = strHeaderInfo.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }

        private void GetPathologicalStatus(long ptId, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 3;
            ptHeaderInfoModel.HeaderName = "◆病歴";
            var listPtKioRekiItem = _ptKioRekiRepository.GetList(ptId);
            var listPtInfectionItem = _ptInfectionRepository.GetList(ptId);
            var strHeaderInfo = new StringBuilder();
            foreach (var ptKioRekiModel in listPtKioRekiItem)
            {
                if (!string.IsNullOrEmpty(ptKioRekiModel.Byomei))
                {
                    strHeaderInfo.Append(ptKioRekiModel.Byomei);
                    if (!string.IsNullOrEmpty(ptKioRekiModel.Cmt))
                    {
                        strHeaderInfo.Append("／" + ptKioRekiModel.Cmt);
                    }
                    strHeaderInfo.Append(Environment.NewLine);
                }
            }
            foreach (var ptInfectionModel in listPtInfectionItem)
            {
                if (!string.IsNullOrEmpty(ptInfectionModel.Byomei))
                {
                    strHeaderInfo.Append(ptInfectionModel.Byomei);
                    if (!string.IsNullOrEmpty(ptInfectionModel.Cmt))
                    {
                        strHeaderInfo.Append("／" + ptInfectionModel.Cmt);
                    }
                    strHeaderInfo.Append(Environment.NewLine);
                }
            }
            ptHeaderInfoModel.HeaderInfo = strHeaderInfo.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }

        private void GetInteraction(long ptId, int sinDate, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 4;
            ptHeaderInfoModel.HeaderName = "◆服薬情報";

            var listPtOtherDrugItem = _ptOtherDrugRepository.GetList(ptId).Select(p => new PtOtherDrugItem(p)).Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate).OrderBy(p => p.PtOtherDrug.SortNo).ToList();
            var listPtOtcDrugItem = _ptPtOtcDrugRepository.GetList(ptId).Select(p => new PtOtcDrugItem(p)).Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate)
                    .OrderBy(p => p.PtOtcDrug.SortNo);
            var listPtSuppleModel = _ptPtSuppleRepository.GetList(ptId).Select(p => new PtSuppleItem(p)).Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate)
                    .OrderBy(p => p.PtSupple.SortNo);
            var strHeaderInfo = new StringBuilder();

            foreach (var ptOtherDrugModel in listPtOtherDrugItem)
            {
                if (!string.IsNullOrEmpty(ptOtherDrugModel.DrugName))
                {
                    strHeaderInfo.Append(ptOtherDrugModel.DrugName);
                    if (!string.IsNullOrEmpty(ptOtherDrugModel.Cmt))
                    {
                        strHeaderInfo.Append("／" + ptOtherDrugModel.Cmt);
                    }
                    strHeaderInfo.Append(Environment.NewLine);
                }
            }
            foreach (var ptOtcDrugModel in listPtOtcDrugItem)
            {
                if (!string.IsNullOrEmpty(ptOtcDrugModel.TradeName))
                {
                    strHeaderInfo.Append(ptOtcDrugModel.TradeName);
                    if (!string.IsNullOrEmpty(ptOtcDrugModel.Cmt))
                    {
                        strHeaderInfo.Append("／" + ptOtcDrugModel.Cmt);
                    }
                    strHeaderInfo.Append(Environment.NewLine);
                }
            }
            foreach (var suppleModel in listPtSuppleModel)
            {
                if (!string.IsNullOrEmpty(suppleModel.IndexWord))
                {
                    strHeaderInfo.Append(suppleModel.IndexWord);
                    if (!string.IsNullOrEmpty(suppleModel.Cmt))
                    {
                        strHeaderInfo.Append("／" + suppleModel.Cmt);
                    }
                    strHeaderInfo.Append(Environment.NewLine);
                }
            }
            ptHeaderInfoModel.HeaderInfo = strHeaderInfo.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }

        private void GetCalculationInfo(long ptId, int hpId, int sinDate, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 5;
            ptHeaderInfoModel.HeaderName = "◆算定情報";
            var listSanteiInfModels = GetCalculationInfo(ptId, hpId, sinDate);
            var strHeaderInfo = new StringBuilder();

            if (listSanteiInfModels.Count > 0)
            {
                listSanteiInfModels = listSanteiInfModels.Where(u => u.DayCount > u.AlertDays).ToList();
                foreach (SanteiInfomationItem santeiInfomationModel in listSanteiInfModels)
                {
                    strHeaderInfo.Append(santeiInfomationModel.ItemName?.Trim() + "(" + santeiInfomationModel.KisanType + " " + CIUtil.SDateToShowSDate(santeiInfomationModel.LastOdrDate) + @"～　" + santeiInfomationModel.DayCountDisplay + ")" + Environment.NewLine);
                }
                ptHeaderInfoModel.HeaderInfo = strHeaderInfo.ToString().TrimEnd(Environment.NewLine.ToCharArray());
            }
        }

        public List<SanteiInfomationItem> GetCalculationInfo(long ptId, int hpId, int sinDate)
        {
            var santeiInfs = _santeiInfoRepository.GetList(hpId, ptId, sinDate);

            var tenMsts = _tenMstRepository.GetList(hpId, sinDate);

            // Query Santei inf code
            var kensaTenMst = _tenMstRepository.GetList(hpId, sinDate);

            var tenMstList = from santeiInf in santeiInfs
                             join tenMst in kensaTenMst on santeiInf.ItemCd
                                                    equals tenMst.SanteiItemCd into tenMstLeft
                             from tenMst in tenMstLeft.DefaultIfEmpty()
                             select new
                             {
                                 SanteiCd = santeiInf.ItemCd,
                                 ItemCd = tenMst?.ItemCd ?? santeiInf.ItemCd
                             };


            var odrInfs = _ordInfRepository.GetList(ptId, hpId, sinDate, false);


            var listOdrInfs = from odrInfItem in odrInfs
                              select new
                              {
                                  tenMstList.SingleOrDefault(t => odrInfItem?.OrdInfDetails?.Any(od => od.ItemCd == t.ItemCd) == true)?.SanteiCd,
                                  OdrInf = odrInfItem,
                                  OdrInfDetail = odrInfItem.OrdInfDetails,
                              };

            //Get last oder day by ItemCd
            var listOrdInfomation = listOdrInfs.AsEnumerable().OrderByDescending(u => u.OdrInf.SinDate).GroupBy(o => o.SanteiCd).Select(g => g.First()).ToList(); //select distinct by ItemCd
            var listOrdDetailInfomation = listOrdInfomation.Select(o => new { o.OdrInfDetail, o.SanteiCd }).ToList(); // only select OdrDetailInfo 

            var santeiQuery = from santeiInfItem in santeiInfs
                              join tenMstItem in tenMsts on santeiInfItem.ItemCd equals tenMstItem.ItemCd
                              select new
                              {
                                  SanteiInf = santeiInfItem,
                                  SnteiInfDetail = santeiInfItem.SanteiInfoDetailModel,
                                  TenMst = tenMstItem
                              };
            var result = santeiQuery.AsEnumerable().Select(u => new SanteiInfomationItem(u.SanteiInf, u.SnteiInfDetail?.FirstOrDefault(), u.TenMst, listOrdDetailInfomation.FirstOrDefault(o => o.SanteiCd == u.SanteiInf.ItemCd)?.OdrInfDetail?.FirstOrDefault(), sinDate)).OrderBy(t => t.ItemCd).ToList();
            return result;
        }


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
                }

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

        private void GetReservationInf(int hpId, long ptId, PtInfNotificationItem ptHeaderInfoModel)
        {
            int today = DateTime.Now.ToString("yyyyMMdd").AsInteger();
            ptHeaderInfoModel.GrpItemCd = 7;
            ptHeaderInfoModel.HeaderName = "■予約情報";
            var listRsvInfModel = GetRsvInfoByRsvInf(hpId, ptId, today);
            var listRaiinInfModel = _receptionRepository.GetList(hpId, ptId, today);
            listRaiinInfModel = listRaiinInfModel.Where(u => !listRsvInfModel.Any(r => r.RaiinNo == u.RaiinNo)).ToList();
            var strHeaderInfo = new StringBuilder();

            foreach (var raiinInf in listRaiinInfModel)
            {
                listRsvInfModel.Add(new RsvInfItem(null, null, null, raiinInf));
            }

            if (listRsvInfModel.Count > 0)
            {
                listRsvInfModel = listRsvInfModel.OrderBy(u => u.SinDate).ToList();
                foreach (var rsvInfModel in listRsvInfModel)
                {
                    if (rsvInfModel.RsvInf != null)
                    {
                        //formart for RsvInf
                        string startTime = rsvInfModel.StartTime > 0 ? " " + CIUtil.TimeToShowTime(rsvInfModel.StartTime) + " " : " ";
                        string rsvFrameName = string.IsNullOrEmpty(rsvInfModel.RsvFrameName) ? string.Empty : "[" + rsvInfModel.RsvFrameName + "]";
                        strHeaderInfo.Append(CIUtil.SDateToShowSDate2(rsvInfModel.SinDate) + startTime + rsvInfModel.RsvGrpName + " " + rsvFrameName + Environment.NewLine);
                    }
                    else
                    {
                        //formart for raiinInf
                        string kaName = string.IsNullOrEmpty(rsvInfModel?.RaiinInfModel?.KaSname) ? " " : " " + "[" + rsvInfModel.RaiinInfModel.KaSname + "]" + " ";
                        strHeaderInfo.Append(CIUtil.SDateToShowSDate2(rsvInfModel?.SinDate ?? 0) + " "
                                                        + FormatTime(rsvInfModel?.RaiinInfModel?.YoyakuTime ?? string.Empty)
                                                        + kaName
                                                        + rsvInfModel?.RaiinInfModel?.TatoName ?? string.Empty + " "
                                                        + (!string.IsNullOrEmpty(rsvInfModel?.RaiinInfModel?.RaiinCmt) ? "(" + rsvInfModel.RaiinInfModel.RaiinCmt + ")" : string.Empty)
                                                        + Environment.NewLine);
                    }
                }
            }
            ptHeaderInfoModel.HeaderInfo = strHeaderInfo.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }

        private void GetComment(long ptId, int hpId, PtInfNotificationItem ptHeaderInfoModel)
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

        private void GetAddress(int hpId, long ptId, PtInfNotificationItem ptHeaderInfoModel)
        {
            var ptInfo = _patientInfRepository.GetById(hpId, ptId, false);
            var _ptInfoItem = ptInfo == null ? null : new PtInfoItem(ptInfo);

            ptHeaderInfoModel.GrpItemCd = 9;
            ptHeaderInfoModel.HeaderName = "◆住所";
            if (_ptInfoItem != null && !string.IsNullOrEmpty(_ptInfoItem.HomeAddress1 + _ptInfoItem.HomeAddress2))
            {
                ptHeaderInfoModel.HeaderInfo = _ptInfoItem.HomeAddress1 + " " + _ptInfoItem.HomeAddress2;
            }
        }

        private void GetInsuranceInfo(long ptId, int sinDate, int hpId, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 10;
            ptHeaderInfoModel.HeaderName = "◆保険情報";
            var listPtHokenInfoItem = _insuranceRepository.GetInsuranceListById(hpId, ptId, sinDate, 28).ToList();
            if (listPtHokenInfoItem?.Count == 0) return;
            StringBuilder futanInfo = new StringBuilder();
            StringBuilder kohiInf = new StringBuilder();

            if (listPtHokenInfoItem?.Count > 0)
            {
                foreach (var ptHokenInfoModel in listPtHokenInfoItem)
                {

                    kohiInf?.Append(GetFutanInfo(ptHokenInfoModel.Kohi1));

                    kohiInf?.Append(GetFutanInfo(ptHokenInfoModel.Kohi2));

                    kohiInf?.Append(GetFutanInfo(ptHokenInfoModel.Kohi3));

                    kohiInf?.Append(GetFutanInfo(ptHokenInfoModel.Kohi4));

                    if (string.IsNullOrEmpty(kohiInf?.ToString()))
                    {
                        continue;
                    }
                    var strKohiInf = kohiInf?.ToString();
                    strKohiInf = strKohiInf?.TrimEnd();
                    strKohiInf = strKohiInf?.TrimEnd('　');
                    strKohiInf = strKohiInf?.TrimEnd(',');
                    futanInfo.Append(ptHokenInfoModel.HokenPid.ToString().PadLeft(3, '0') + ". ");
                    futanInfo.Append(strKohiInf);
                    futanInfo.Append(Environment.NewLine);
                }
            }
            var strFutanInfo = futanInfo?.ToString().TrimEnd();
            if (!string.IsNullOrEmpty(strFutanInfo?.Trim()))
            {
                ptHeaderInfoModel.HeaderInfo = futanInfo?.ToString() ?? String.Empty;
            }
        }

        private string GetFutanInfo(KohiInfModel ptKohi)
        {
            var hokenMst = ptKohi?.HokenMstModel;
            int gokenGaku = ptKohi?.GendoGaku ?? 0;
            string futanInfo = string.Empty;

            if (!string.IsNullOrEmpty(ptKohi?.FutansyaNo))
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

            if (hokenMst == null && !string.IsNullOrEmpty(ptKohi?.FutansyaNo))
            {
                return futanInfo + "," + " ";
            }
            if (hokenMst?.FutanKbn == 0)
            {
                //負担なし
                futanInfo += "0円";
            }
            else
            {
                if (hokenMst?.KaiLimitFutan > 0)
                {
                    if (hokenMst?.DayLimitFutan <= 0 && hokenMst?.MonthLimitFutan <= 0 && gokenGaku > 0)
                    {
                        futanInfo += gokenGaku.AsString() + "円/回・";
                    }
                    else
                    {
                        futanInfo += hokenMst?.KaiLimitFutan.AsString() + "円/回・";
                    }
                }

                if (hokenMst?.DayLimitFutan > 0)
                {
                    if (hokenMst?.KaiLimitFutan <= 0 && hokenMst?.MonthLimitFutan <= 0 && gokenGaku > 0)
                    {
                        futanInfo += gokenGaku.AsString() + "円/日・";
                    }
                    else
                    {
                        futanInfo += hokenMst?.DayLimitFutan.AsString() + "円/日・";
                    }
                }

                if (hokenMst?.DayLimitCount > 0)
                {
                    futanInfo = hokenMst?.DayLimitCount.AsString() + "回/日・";
                }

                if (hokenMst?.MonthLimitFutan > 0)
                {
                    if (hokenMst?.KaiLimitFutan <= 0 && hokenMst?.DayLimitFutan <= 0 && gokenGaku > 0)
                    {
                        futanInfo += gokenGaku.AsString() + "円/月・";
                    }
                    else
                    {
                        futanInfo += hokenMst?.MonthLimitFutan.AsString() + "円/月・";
                    }
                }

                if (hokenMst?.MonthLimitCount > 0)
                {
                    futanInfo += hokenMst?.MonthLimitCount.AsString() + "回/月";
                }
            }
            if (!string.IsNullOrEmpty(futanInfo))
            {
                futanInfo = futanInfo.TrimEnd('・');
                futanInfo = futanInfo + "," + " ";
            }
            return futanInfo;
        }

        private void GetLifeHistory(long ptId, int hpId, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 11;
            ptHeaderInfoModel.HeaderName = "■生活歴";

            var seikaturekiInfModel = _seikaturekiInRepository.GetList(ptId, hpId).FirstOrDefault();
            if (seikaturekiInfModel != null)
            {
                ptHeaderInfoModel.HeaderInfo = seikaturekiInfModel.Text;
            }
        }

        private void GetPhoneNumber(int hpId, long ptId, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 12;
            ptHeaderInfoModel.HeaderName = "◆電話番号";

            var ptInfo = _patientInfRepository.GetById(hpId, ptId, false);
            var _ptInfoItem = ptInfo == null ? null : new PtInfoItem(ptInfo);

            if (_ptInfoItem != null && !string.IsNullOrEmpty(_ptInfoItem.Tel1 + _ptInfoItem.Tel2))
            {
                ptHeaderInfoModel.HeaderInfo = _ptInfoItem.Tel1 + Environment.NewLine + _ptInfoItem.Tel2;
            }
        }

        private void GetReceptionComment(long ptId, int hpId, long raiinNo, int sinDate, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 13;
            ptHeaderInfoModel.HeaderName = "◆来院コメント";
            var raiinCmtInf = _raiinCmtInfRepository.GetList(hpId, ptId, sinDate, raiinNo).FirstOrDefault();
            if (raiinCmtInf != null)
            {
                ptHeaderInfoModel.HeaderInfo = raiinCmtInf.Text;
            }
        }

        private void GetPhysicalInfo(int hpId, long ptId, int sinDate, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 1;
            ptHeaderInfoModel.HeaderName = "■身体情報";
            var strHeaderInfo = new StringBuilder();
            var listKensaInfDetailItem = GetListKensaInfDetailModel(hpId, ptId, sinDate);
            long maxSortNo = listKensaInfDetailItem.Max(u => u.SortNo);
            var heightModel = listKensaInfDetailItem.FirstOrDefault(u => u.KensaItemCd == IraiCodeConstant.HEIGHT_CODE);
            var weightModel = listKensaInfDetailItem.FirstOrDefault(u => u.KensaItemCd == IraiCodeConstant.WEIGHT_CODE);
            var bmiModel = listKensaInfDetailItem.FirstOrDefault(u => u.KensaItemCd == IraiCodeConstant.BMI_CODE);

            if (heightModel == null ||
                weightModel == null ||
                heightModel.ResultVal.AsDouble() <= 0 ||
                weightModel.ResultVal.AsDouble() <= 0)
            {
                if (bmiModel != null)
                {
                    listKensaInfDetailItem.Remove(bmiModel);
                }
            }
            else
            {
                string bmi = String.Format("{0:0.0}", weightModel.ResultVal.AsDouble() / (heightModel.ResultVal.AsDouble() * heightModel.ResultVal.AsDouble() / 10000));
                if (bmiModel != null)
                {
                    bmiModel.ResultVal = bmi;
                    bmiModel.IraiDate = heightModel.IraiDate >= weightModel.IraiDate ? heightModel.IraiDate : weightModel.IraiDate;
                }
                else
                {
                    if (bmi.AsDouble() > 0)
                    {
                        listKensaInfDetailItem.Add(
                        new KensaInfDetailItem
                        ("BMI", IraiCodeConstant.BMI_CODE, bmi, maxSortNo + 1
                        ));
                    }
                }
            }
            listKensaInfDetailItem = listKensaInfDetailItem.OrderBy(u => u.SortNo).ToList();
            foreach (var kensaDetailModel in listKensaInfDetailItem)
            {
                string sSate = CIUtil.SDateToShowSDate(kensaDetailModel.IraiDate);
                string kensaName = string.Empty;
                if (kensaDetailModel.KensaItemCd != IraiCodeConstant.HEIGHT_CODE && kensaDetailModel.KensaItemCd != IraiCodeConstant.WEIGHT_CODE)
                {
                    kensaName = kensaDetailModel.KensaName;
                }
                else
                {
                    if (string.IsNullOrEmpty(kensaDetailModel.ResultVal))
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(kensaDetailModel.ResultVal))
                {
                    strHeaderInfo.Append((string.IsNullOrEmpty(kensaName) ? string.Empty : kensaName + ":" + " ") + kensaDetailModel.ResultVal + kensaDetailModel.Unit + " " + (string.IsNullOrEmpty(sSate) ? string.Empty : "(" + sSate + ")") + " " + "/");
                }
            }
            ptHeaderInfoModel.HeaderInfo = strHeaderInfo.ToString().TrimEnd('/');
        }


        private List<KensaInfDetailItem> GetListKensaInfDetailModel(int hpId, long ptId, int sinDate)
        {

            var KensaMstRepos = _kensaMstRepository.GetList(hpId)
                .Select(u => new
                {
                    KensaName = u.KensaName,
                    KensaItemCd = u.KensaItemCd,
                    Unit = u.Unit,
                    SortNo = u.SortNo
                });
            var kensaInfDetailRepos = _kensaInfDetailRepository.GetList(hpId, ptId, sinDate);
            var query = from KensaMst in KensaMstRepos
                        join kensaInfDetail in kensaInfDetailRepos on
                        KensaMst.KensaItemCd equals kensaInfDetail.KensaItemCd into listDetail
                        select new
                        {
                            KensaMst = KensaMst,
                            KensaInfDetail = listDetail.OrderByDescending(item => item.IraiDate).ThenByDescending(item => item.UpdateDate).FirstOrDefault()
                        };
            var result = query.AsEnumerable().Select(u => new KensaInfDetailItem(u.KensaInfDetail)
            {
                KensaItemCd = u.KensaMst.KensaItemCd,
                Unit = u.KensaMst.Unit,
                KensaName = u.KensaMst.KensaName,
                SortNo = u.KensaMst.SortNo
            }
            ).ToList();

            return result;
        }

        public SummaryInfModel? GetSummaryInf(int hpId, long ptId)
        {
            var summaryInf = _summaryInfRepository.GetList(hpId, ptId).FirstOrDefault();
            return summaryInf;
        }

        private List<UserConfigModel> GetListUserConf(int hpId, int userId, int groupCd, bool fromLastestDb = false)
        {
            if (!fromLastestDb)
            {
                return _userConfigRepository.GetList(groupCd);
            }
            else
            {
                return _userConfigRepository.GetList(hpId, groupCd, userId);
            }
        }

        //convert string to HH:mm
        private string FormatTime(string time)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(time))
            {
                if (time.Length > 4)
                {
                    result = time.Substring(0, 4);
                }
                else if (time.Length < 4)
                {
                    result = time.PadLeft(4, '0');
                }
                else
                {
                    result = time;
                }
                result = result.Insert(2, ":");
            }
            if (result == "00:00")
            {
                result = string.Empty;
            }
            return result;
        }

        private List<PtInfNotificationItem> GetNotificationContent(int userId, int hpId, long ptId, int sinDate)
        {
            List<PtInfNotificationItem> listNotification = new List<PtInfNotificationItem>();
            var _listNotifiProperty = this.GetListUserConf(hpId, userId, 915, true).Where(x => x.Val != 0).ToList();
            var listNotifiSort = this.GetListUserConf(hpId, userId, 916, true).OrderBy(x => x.Val).ToList();
            foreach (var sort in listNotifiSort)
            {
                var userConfiguration = _listNotifiProperty.FirstOrDefault(item => item.GrpItemCd == sort.GrpItemCd);
                if (userConfiguration != null)
                {
                    var ptInfNotificationModel = GetNotificationInfoToList(hpId, ptId, sinDate, userId, userConfiguration);
                    if (ptInfNotificationModel != null)
                    {
                        listNotification.Add(ptInfNotificationModel);
                    }
                }
            }

            return listNotification;
        }
        private List<PtInfNotificationItem> GetNotification(int userId, int hpId, long ptId, int sinDate)
        {
            List<PtInfNotificationItem> listNotification = GetNotificationContent(userId, hpId, ptId, sinDate);

            if (listNotification.Count >= 1)
            {
                listNotification.Last().SpaceHeaderInfo = 0;
            }
            return listNotification;
        }

        private PtInfNotificationItem? GetNotificationInfoToList(int hpId, long ptId, int sinDate, int userId, UserConfigModel userConfNoti)
        {
            PtInfNotificationItem ptInfNotificationModel = new PtInfNotificationItem();
            switch (userConfNoti.GrpItemCd.AsString())
            {
                case "1":
                    //身体情報
                    GetPhysicalInfo(hpId, ptId, sinDate, ptInfNotificationModel);
                    ptInfNotificationModel.GrpItemCd = 1;
                    break;
                case "2":
                    //アレルギー 
                    GetDrugInfo(ptId, sinDate, ptInfNotificationModel);
                    ptInfNotificationModel.GrpItemCd = 2;
                    break;
                case "3":
                    // 病歴
                    GetPathologicalStatus(ptId, ptInfNotificationModel);
                    ptInfNotificationModel.GrpItemCd = 3;
                    break;
                case "4":
                    // 服薬情報
                    GetInteraction(ptId, sinDate, ptInfNotificationModel);
                    ptInfNotificationModel.GrpItemCd = 4;
                    break;
                case "5":
                    //生活歴
                    GetLifeHistory(ptId, hpId, ptInfNotificationModel);
                    ptInfNotificationModel.GrpItemCd = 5;
                    break;
                case "6":
                    //コメント
                    GetComment(ptId, hpId, ptInfNotificationModel);
                    ptInfNotificationModel.GrpItemCd = 6;
                    break;
                case "7":
                    //経過日数
                    GetCalculationInfo(ptId, hpId, sinDate, ptInfNotificationModel);
                    ptInfNotificationModel.HeaderName = "◆経過日数";
                    ptInfNotificationModel.GrpItemCd = 7;
                    break;
                case "8":
                    //出産予定
                    GetReproductionInfo(ptId, hpId, sinDate, ptInfNotificationModel);
                    ptInfNotificationModel.GrpItemCd = 8;
                    break;
                case "9":
                    //予約情報
                    GetReservationInf(hpId, ptId, ptInfNotificationModel);
                    ptInfNotificationModel.GrpItemCd = 9;
                    break;
                case "0":
                    //検査結果速報
                    ptInfNotificationModel.HeaderName = "■検査結果速報";
                    ptInfNotificationModel.GrpItemCd = 0;
                    break;
            }
            if (!string.IsNullOrEmpty(ptInfNotificationModel.HeaderInfo))
            {

                ptInfNotificationModel.HeaderName = ptInfNotificationModel.HeaderName.Replace("◆", "【");
                ptInfNotificationModel.HeaderName = ptInfNotificationModel.HeaderName.Replace("■", "【");
                ptInfNotificationModel.HeaderName = ptInfNotificationModel.HeaderName.Insert(ptInfNotificationModel.HeaderName.Length, "】");
                if (ptInfNotificationModel.HeaderName.Contains("【コメント】"))
                {
                    ptInfNotificationModel.HeaderInfo = ptInfNotificationModel.HeaderInfo.Replace(Environment.NewLine, " ・ ");
                    ptInfNotificationModel.HeaderInfo = ptInfNotificationModel.HeaderInfo?.Trim() ?? string.Empty;
                    ptInfNotificationModel.HeaderInfo = ptInfNotificationModel.HeaderInfo?.TrimEnd('・') ?? string.Empty;
                }
                else
                {
                    ptInfNotificationModel.HeaderInfo = ptInfNotificationModel.HeaderInfo.Replace(Environment.NewLine, "    ");
                }
                ptInfNotificationModel.HeaderInfo = ptInfNotificationModel.HeaderInfo?.Trim() ?? string.Empty;

                if (!string.IsNullOrEmpty(ptInfNotificationModel.HeaderInfo))
                {
                    var colorTextNotifi = GetListUserConf(hpId, userId, 917, true).FirstOrDefault(x => x.GrpItemCd == userConfNoti.GrpItemCd);
                    if (colorTextNotifi != null)
                    {
                        ptInfNotificationModel.PropertyColor = colorTextNotifi.Param;
                    }

                    ptInfNotificationModel.SpaceHeaderName = 5.0;
                    ptInfNotificationModel.SpaceHeaderInfo = 30.0;
                    return ptInfNotificationModel;
                }
            }
            return null;
        }

        private List<PtFamilyItem> GetFamilyListByPtId(long ptId, int hpId)
        {

            var ptFamilyRepo = _ptFamilyRepository.GetList(ptId, hpId);
            var ptInfRepo = _patientInfRepository.GetById(ptId);

            var ptFamilyRekis = _ptFamilyRekiRepository.GetList(hpId).OrderBy(u => u.SortNo);
            var query =
            (
                from ptFamily in ptFamilyRepo
                join ptInf in ptInfRepo on ptFamily.FamilyPtId equals ptInf.PtId into ptInfList
                from ptInfItem in ptInfList.DefaultIfEmpty()
                select new
                {
                    PtFamily = ptFamily,
                    PtInf = ptInfItem,
                    ListPtFamilyRekiInfo = ptFamilyRekis.Where(c => c.FamilyId == ptFamily.FamilyId).ToList()
                }
            );
            return query.AsEnumerable().Select(data => new PtFamilyItem(data.PtFamily, data.PtInf)
            {
                ListPtFamilyRekiModel = data.ListPtFamilyRekiInfo == null
                    ? new List<PtFamilyRekiItem>()
                    : new List<PtFamilyRekiItem>(data.ListPtFamilyRekiInfo.Select(u => new PtFamilyRekiItem(u)))
            }).ToList();

        }

        private void GetFamilyList(long ptId, int hpId, PtInfNotificationItem ptHeaderInfoModel)
        {
            ptHeaderInfoModel.GrpItemCd = 14;
            ptHeaderInfoModel.HeaderName = "◆家族歴";

            var ptFamilyList = GetFamilyListByPtId(ptId, hpId);
            if (ptFamilyList != null)
            {
                var headerInfo = new StringBuilder();
                foreach (var ptFamilyModel in ptFamilyList)
                {
                    SetDiseaseName(ptFamilyModel);
                    if (!string.IsNullOrWhiteSpace(ptFamilyModel.DiseaseName))
                    {
                        if (!string.IsNullOrEmpty(headerInfo.ToString()))
                        {
                            headerInfo.Append(Environment.NewLine);
                        }
                        headerInfo.Append($"({GetRelationshipName(ptFamilyModel.ZokugaraCd)}){ptFamilyModel.DiseaseName}");
                    }
                }

                if (!string.IsNullOrEmpty(headerInfo.ToString()))
                {
                    ptHeaderInfoModel.HeaderInfo = headerInfo.ToString();
                }
            }
        }
        private void SetDiseaseName(PtFamilyItem ptFamilyModel)
        {
            var strDeaseName = new StringBuilder();
            if (ptFamilyModel.ListPtFamilyRekiModel != null && ptFamilyModel.ListPtFamilyRekiModel.Count > 0)
            {
                foreach (var ptByomeiMode in ptFamilyModel.ListPtFamilyRekiModel)
                {
                    if (ptByomeiMode.IsDeleted == 0)
                    {
                        if (!string.IsNullOrEmpty(ptFamilyModel.DiseaseName))
                        {
                            strDeaseName.Append("・");
                        }
                        strDeaseName.Append(ptByomeiMode.Byomei);
                    }
                }
            }
            ptFamilyModel.DiseaseName = strDeaseName.ToString();
        }

        private string GetRelationshipName(string zokugaraCd)
        {
            var relationshipName = string.Empty;

            if (_relationship == null)
            {
                _relationship = new Dictionary<string, string>()
                {
                    {nameof(Relationship.BR), "血縁"},
                    {nameof(Relationship.MA), "配偶者"},
                    {nameof(Relationship.FA), "父"},
                    {nameof(Relationship.MO), "母"},

                    {nameof(Relationship.GF1), "祖父(父方)"},
                    {nameof(Relationship.GM1), "祖母(父方)"},
                    {nameof(Relationship.GF2), "祖父(母方)"},
                    {nameof(Relationship.GM2), "祖母(母方)"},

                    {nameof(Relationship.SO), "息子"},
                    {nameof(Relationship.DA), "娘"},
                    {nameof(Relationship.BB), "兄"},
                    {nameof(Relationship.LB), "弟"},

                    {nameof(Relationship.BS), "姉"},
                    {nameof(Relationship.LS), "妹"},
                    {nameof(Relationship.GC), "孫"},
                    {nameof(Relationship.OT), "非血縁"},
                };
            }

            if (_relationship.Keys.Contains(zokugaraCd))
            {
                relationshipName = _relationship[zokugaraCd];
            }

            return relationshipName;
        }


        private void SetForeground(UserConfigModel userConfigurationModel, List<PtInfNotificationItem> listHeader1InfoModels, List<PtInfNotificationItem> listHeader2InfoModels)
        {
            var ptHeaderInfoModel = listHeader1InfoModels.FirstOrDefault(u => u.GrpItemCd == userConfigurationModel.GrpItemCd) != null ?
                                                  listHeader1InfoModels.FirstOrDefault(u => u.GrpItemCd == userConfigurationModel.GrpItemCd) :
                                                  listHeader2InfoModels.FirstOrDefault(u => u.GrpItemCd == userConfigurationModel.GrpItemCd);
            if (ptHeaderInfoModel != null)
            {
                ptHeaderInfoModel.PropertyColor = userConfigurationModel.Param;
            }
        }

        public List<RsvInfItem> GetRsvInfoByRsvInf(int hpId, long ptId, int sinDate)
        {
            var rsvInfs = _rsvInfoRepository.GetList(hpId, ptId, sinDate);
            var rsvFrmMsts = _rsvFrameMstRepository.GetList(hpId);

            var rsvGrpMsts = _rsvGrpMstRepository.GetList(hpId);
            var rsvDetailInfs = from rsvFrmMstItem in rsvFrmMsts
                                join rsvGrpMstItem in rsvGrpMsts on rsvFrmMstItem.RsvGrpId equals rsvGrpMstItem.RsvGrpId
                                select new
                                {
                                    RsvFrmMst = rsvFrmMstItem,
                                    RsvGrpMst = rsvGrpMstItem
                                };
            var query = from rsvInfItem in rsvInfs
                        select new
                        {
                            RsvInf = rsvInfItem,
                            Detail = rsvDetailInfs.FirstOrDefault(c => c.RsvFrmMst.RsvFrameId == rsvInfItem.RsvFrameId),
                        };
            var result = query.AsEnumerable().Select(u => new RsvInfItem(u.RsvInf, (u.Detail == null ? null : u.Detail.RsvFrmMst), (u.Detail == null ? null : u.Detail.RsvGrpMst), null)).OrderBy(r => r.SinDate).ToList();
            return result;
        }
    }
}
