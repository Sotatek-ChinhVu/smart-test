using Domain.Models.KensaInfDetail;
using Domain.Models.KensaMst;
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
using Domain.Models.SeikaturekiInf;
using Domain.Models.SummaryInf;
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
        private readonly ISeikaturekiInfRepository _seikaturekiInRepository;
        private readonly IKensaInfDetailRepository _kensaInfDetailRepository;
        private readonly IKensaMstRepository _kensaMstRepository;
        private readonly ISummaryInfRepository _summaryInfRepository;
        private readonly IUserConfigRepository _userConfigRepository;

        public GetHeaderSumaryInfoInteractor(IPtAlrgyElseRepository ptAlrgryElseRepository, IPtAlrgyFoodRepository ptPtAlrgyFoodRepository, IPtAlrgyDrugRepository ptPtAlrgyDrugRepository, IPtKioRekiRepository ptKioRekiRepository, IPtInfectionRepository ptInfectionRepository, IPtOtherDrugRepository ptOtherDrugRepository, IPtOtcDrugRepository ptPtOtcDrugRepository, IPtSuppleRepository ptPtSuppleRepository, IPtPregnancyRepository ptPregnancyRepository, IPtCmtInfRepository ptCmtInfRepository, ISeikaturekiInfRepository seikaturekiInRepository, IKensaInfDetailRepository kensaInfDetailRepository, IKensaMstRepository kensaMstRepository, ISummaryInfRepository summaryInfRepository, IUserConfigRepository userConfigRepository)
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
            _seikaturekiInRepository = seikaturekiInRepository;
            _kensaInfDetailRepository = kensaInfDetailRepository;
            _kensaMstRepository = kensaMstRepository;
            _summaryInfRepository = summaryInfRepository;
            _userConfigRepository = userConfigRepository;
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
                var ptHeaderInfoModel = GetSummaryInfo(inputData.PtId, inputData.HpId, inputData.SinDate, propertyCd.AsString(), infoType);
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
                var ptHeaderInfoModel = GetSummaryInfo(inputData.PtId, inputData.HpId, inputData.SinDate, propertyCd.AsString(), infoType);
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
                var userConf = _userConfigRepository.Get(userId, groupCd, grpItemCd);
                return userConf != null ? userConf.Param : defaultValue;
            }
            else
            {
                var userConf = _userConfigRepository.GetList(hpId, groupCd, grpItemCd, userId).FirstOrDefault();
                return userConf != null ? userConf.Param : defaultValue;
            }
        }

        private SummaryTabItem? GetSummaryInfo(long ptId, int hpId, int sinDate, string propertyCd, InfoType infoType = InfoType.PtHeaderInfo)
        {
            //SummaryTabItem ptHeaderInfoModel = new PtInfNotificationItem()
            //{
            //    PropertyColor = "000000" // default color
            //};

            //GetData(hpId, ptId, sinDate, propertyCd, ref ptHeaderInfoModel);

            if (infoType == InfoType.SumaryInfo && propertyCd == "C")
            {
                //サマリー
                //ptHeaderInfoModel.GrpItemCd = 12;
                //ptHeaderInfoModel.HeaderName = "◆サマリー";

                var summaryInf = GetSummaryInf(hpId, ptId);
                if (summaryInf != null && !string.IsNullOrEmpty(summaryInf.Text))
                {
                    //SummaryTabItem.HeaderInfo = summaryInf.Text;
                    return new SummaryTabItem(summaryInf.Text);
                }
            }

            return null;
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
                case "6":
                    //出産予定
                    GetReproductionInfo(ptId, hpId, sinDate, ptHeaderInfoModel);
                    break;
                case "8":
                    //コメント
                    GetComment(ptId, hpId, ptHeaderInfoModel);
                    break;
                case "B":
                    //生活歴
                    GetLifeHistory(ptId, hpId, ptHeaderInfoModel);
                    break;
            }
        }

        private ImportantNoteTabItem ImportantNoteTabItem(long ptId, int sinDate)
        {
            var listPtAlrgyElseItem = _ptAlrgryElseRepository.GetList(ptId).Where(ag => ConvertToDate(ag.StartDate, true) <= sinDate && ConvertToDate(ag.StartDate, false) >= sinDate).OrderBy(p => p.SortNo).ToList();
            var listPtAlrgyFoodItem = _ptPtAlrgyFoodRepository.GetList(ptId).Where(ag => ConvertToDate(ag.StartDate, true) <= sinDate && ConvertToDate(ag.StartDate, false) >= sinDate).OrderBy(p => p.SortNo).ToList();
            var listPtAlrgyDrugItem = _ptPtAlrgyDrugRepository.GetList(ptId).Where(ag => ConvertToDate(ag.StartDate, true) <= sinDate && ConvertToDate(ag.StartDate, false) >= sinDate).OrderBy(p => p.SortNo).ToList();

            var listPtOtherDrugItem = _ptOtherDrugRepository.GetList(ptId).Where(ag => ConvertToDate(ag.StartDate, true) <= sinDate && ConvertToDate(ag.StartDate, false) >= sinDate).OrderBy(p => p.SortNo).ToList();
            var listPtOtcDrugItem = _ptPtOtcDrugRepository.GetList(ptId).Where(ag => ConvertToDate(ag.StartDate, true) <= sinDate && ConvertToDate(ag.StartDate, false) >= sinDate).OrderBy(p => p.SortNo).ToList();
            var listPtSuppleItem = _ptPtSuppleRepository.GetList(ptId).Where(ag => ConvertToDate(ag.StartDate, true) <= sinDate && ConvertToDate(ag.StartDate, false) >= sinDate).OrderBy(p => p.SortNo).ToList();

            var listPtKioRekiItem = _ptKioRekiRepository.GetList(ptId);
            var listPtInfectionItem = _ptInfectionRepository.GetList(ptId);

            return new ImportantNoteTabItem(listPtAlrgyFoodItem, listPtAlrgyElseItem, listPtAlrgyDrugItem, listPtKioRekiItem, listPtInfectionItem, listPtOtherDrugItem, listPtOtcDrugItem, listPtSuppleItem);
        }
        private PatientInfoTab PatientInfoTab(long ptId, int hpId, int sinDate)
        {
            var listPtPregnancyItems = _ptPregnancyRepository.GetList(ptId, hpId, sinDate);
            var ptCmtInfs = _ptCmtInfRepository.GetList(ptId, hpId)
                              .FirstOrDefault();
            var seikaturekiInfModel = _seikaturekiInRepository.GetList(ptId, hpId).FirstOrDefault();
            var listKensaInfDetailItem = GetListKensaInfDetailModel(hpId, ptId, sinDate);


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
                case "8":
                    //出産予定
                    GetReproductionInfo(ptId, hpId, sinDate, ptInfNotificationModel);
                    ptInfNotificationModel.GrpItemCd = 8;
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

        private int ConvertToDate(int date, bool isStartDate)
        {
            if (isStartDate)
            {
                if (date.AsString().Count() == 8)
                {
                    //Format of StartDate is yyyymmdd
                    return date;
                }
                else
                {
                    //Format of StartDate is yyyymm
                    //Need to convert to yyyymm01
                    return date * 100 + 1;
                }
            }

            else
            {
                if (date.AsString().Count() == 8)
                {
                    //Format of EndDate is yyyymmdd
                    return date;
                }
                else
                {
                    //Format of EndDate is yyyymm
                    //Need to convert to yyyymm31
                    return date * 100 + 31;
                }
            }
        }

    }
}
