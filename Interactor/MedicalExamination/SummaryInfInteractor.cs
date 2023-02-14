using Domain.Models.Family;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using Domain.Models.PtCmtInf;
using Domain.Models.RaiinCmtInf;
using Domain.Models.RsvInf;
using Domain.Models.Santei;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.UserConf;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
using Helper.Extension;
using System.Text;
using UseCase.MedicalExamination.SummaryInf;
using SpecialNotePatienInfDomain = Domain.Models.SpecialNote.PatientInfo;

namespace Interactor.MedicalExamination
{
    public class SummaryInfInteractor : ISummaryInfInputPort
    {
        private const string space = " ";
        private readonly IPatientInforRepository _patientInfRepository;
        private readonly SpecialNotePatienInfDomain.IPatientInfoRepository _specialNotePatientInfRepository;
        private readonly IImportantNoteRepository _importantNoteRepository;
        private readonly ISanteiInfRepository _santeiInfRepository;
        private readonly IPtCmtInfRepository _ptCmtInfRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IFamilyRepository _familyRepository;
        private readonly IRaiinCmtInfRepository _raiinCmtInfRepository;
        private readonly IUserConfRepository _userConfRepository;
        private readonly IRsvInfRepository _rsvInfRepository;
        private Dictionary<string, string> _relationship = new();
        private List<SummaryInfItem> _header1Infos = new();
        private List<SummaryInfItem> _header2Infos = new();
        private List<SummaryInfItem> _notifications = new();
        private List<PopUpNotificationItem> _notificationPopUps = new();

        public SummaryInfInteractor(IPatientInforRepository patientInfRepository, SpecialNotePatienInfDomain.IPatientInfoRepository specialNotePatientInfRepository, IImportantNoteRepository importantNoteRepository, ISanteiInfRepository santeiInfRepository, IPtCmtInfRepository ptCmtInfRepository, IInsuranceRepository insuranceRepository, IRaiinCmtInfRepository raiinCmtInfRepository, IUserConfRepository userConfRepository, IFamilyRepository familyRepository, IRsvInfRepository rsvInfRepository)
        {
            _patientInfRepository = patientInfRepository;
            _specialNotePatientInfRepository = specialNotePatientInfRepository;
            _importantNoteRepository = importantNoteRepository;
            _santeiInfRepository = santeiInfRepository;
            _ptCmtInfRepository = ptCmtInfRepository;
            _insuranceRepository = insuranceRepository;
            _raiinCmtInfRepository = raiinCmtInfRepository;
            _userConfRepository = userConfRepository;
            _familyRepository = familyRepository;
            _rsvInfRepository = rsvInfRepository;
        }

        public SummaryInfOutputData Handle(SummaryInfInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new SummaryInfOutputData(new(), new(), new(), new(), SummaryInfStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new SummaryInfOutputData(new(), new(), new(), new(), SummaryInfStatus.InvalidPtId);
                }
                if (inputData.SinDate < 0)
                {
                    return new SummaryInfOutputData(new(), new(), new(), new(), SummaryInfStatus.InvalidSinDate);
                }
                if (inputData.RaiinNo < 0)
                {
                    return new SummaryInfOutputData(new(), new(), new(), new(), SummaryInfStatus.InvalidRaiinNo);
                }
                if (inputData.UserId < 0)
                {
                    return new SummaryInfOutputData(new(), new(), new(), new(), SummaryInfStatus.InvalidUserId);
                }
                if (inputData.InfoType < 0)
                {
                    return new SummaryInfOutputData(new(), new(), new(), new(), SummaryInfStatus.InvalidInfoType);
                }

                FormatData(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.UserId, inputData.RaiinNo, inputData.InfoType);

                return new SummaryInfOutputData(_header1Infos, _header2Infos, _notifications, _notificationPopUps, SummaryInfStatus.Successed);
            }
            finally
            {
                _patientInfRepository.ReleaseResource();
                _specialNotePatientInfRepository.ReleaseResource();
                _importantNoteRepository.ReleaseResource();
                _santeiInfRepository.ReleaseResource();
                _ptCmtInfRepository.ReleaseResource();
                _insuranceRepository.ReleaseResource();
                _familyRepository.ReleaseResource();
                _raiinCmtInfRepository.ReleaseResource();
                _userConfRepository.ReleaseResource();
                _rsvInfRepository.ReleaseResource();
            }
        }

        private void SetForeground(UserConfModel userConfigurationModel, List<SummaryInfItem> listHeader1InfoModels, List<SummaryInfItem> listHeader2InfoModels)
        {
            var ptHeaderInfoModel = listHeader1InfoModels.Where(u => u.GrpItemCd == userConfigurationModel.GrpItemCd).FirstOrDefault() != null ?
                                                  listHeader1InfoModels.Where(u => u.GrpItemCd == userConfigurationModel.GrpItemCd).FirstOrDefault() :
                                                  listHeader2InfoModels.Where(u => u.GrpItemCd == userConfigurationModel.GrpItemCd).FirstOrDefault();
            if (ptHeaderInfoModel != null)
            {
                ptHeaderInfoModel = ptHeaderInfoModel.ChangePropertyColor(userConfigurationModel.Param);
            }
        }

        /// <summary>
        /// infoType : 
        ///     0 : PtHeaderInfo
        ///     1 : SumaryInfo
        ///     2 : NotificationInfo
        /// </summary>
        /// <param name="infoType"></param>
        private void FormatData(int hpId, long ptId, int sinDate, int userId, long raiinNo, InfoType infoType)
        {
            var newTempListNotification = new List<SummaryInfItem>();
            string header1Property = string.Empty;
            string header2Property = string.Empty;
            var listUserconfig = new List<UserConfModel>();

            if (infoType == InfoType.PtHeaderInfo)
            {
                header1Property = _userConfRepository.GetSettingParam(hpId, userId, 910, defaultValue: "234");
                header2Property = _userConfRepository.GetSettingParam(hpId, userId, 911, defaultValue: "567");
                listUserconfig = _userConfRepository.GetList(hpId, userId, 912).ToList();
                _notifications = GetNotification(hpId, ptId, sinDate, userId);
                _notificationPopUps = GetPopUpNotification(hpId, userId, _notifications);
            }

            foreach (char propertyCd in header1Property)
            {
                var ptHeaderInfoModel = GetSummaryInfo(hpId, ptId, sinDate, propertyCd.AsString(), 1, raiinNo, infoType);
                if (!string.IsNullOrEmpty(ptHeaderInfoModel.HeaderInfo))
                {
                    if (infoType == InfoType.PtHeaderInfo || infoType == InfoType.SumaryInfo)
                    {
                        _header1Infos.Add(ptHeaderInfoModel);
                    }
                    else
                    {
                        _header2Infos.Add(ptHeaderInfoModel);
                    }
                }
            }

            foreach (char propertyCd in header2Property)
            {
                var ptHeaderInfoModel = GetSummaryInfo(hpId, ptId, sinDate, propertyCd.AsString(), 2, raiinNo, infoType);
                if (!string.IsNullOrEmpty(ptHeaderInfoModel.HeaderInfo))
                {
                    _header2Infos.Add(ptHeaderInfoModel);
                }
            }

            foreach (UserConfModel userConfigurationModel in listUserconfig)
            {
                SetForeground(userConfigurationModel, _header1Infos, _header2Infos);
            }

            var listNotifiProperty = _userConfRepository.GetListUserConf(hpId, userId, 915).Where(x => x.Val != 0).ToList();
            foreach (var item in _notifications)
            {

                var userConfNoti = listNotifiProperty.Where(u => u.GrpItemCd == item.GrpItemCd).FirstOrDefault();
                if (userConfNoti != null && userConfNoti.Val == 1)
                {
                    var newItem = item.ChangeHeader(item.HeaderName.Replace("】", "あり】"), string.Empty);
                    newTempListNotification.Add(newItem);
                }
                else
                {
                    newTempListNotification.Add(item);
                }
            }
            _notifications = newTempListNotification;
        }

        private List<PopUpNotificationItem> GetPopUpNotification(int hpId, int userId, List<SummaryInfItem> listNotification)
        {
            var listNotificationPopup = new List<PopUpNotificationItem>();
            var listNotifiProperty = _userConfRepository.GetList(hpId, userId, 915).Where(x => x.Val != 0).ToList();
            foreach (var item in listNotification)
            {
                var headerInfo = item.HeaderInfo;
                headerInfo = headerInfo?.Trim();
                if (headerInfo?.Contains(") /") == true)
                {
                    headerInfo = headerInfo.Replace(") /", ") " + Environment.NewLine);
                }
                else if (headerInfo?.Contains(" ・ ") == true)
                {
                    headerInfo = headerInfo.Replace(" ・ ", Environment.NewLine);
                }
                else
                {
                    headerInfo = headerInfo?.Replace("    ", Environment.NewLine);
                }
                if (headerInfo?.Contains("kg ") == true)
                {
                    headerInfo = headerInfo.Replace("kg ", "kg    ");
                }
                if (headerInfo?.Contains("BMI:" + space) == true)
                {
                    headerInfo = headerInfo.Replace(" /", Environment.NewLine);
                }

                var popUpNotificationModel = new PopUpNotificationItem(headerInfo ?? string.Empty, item?.HeaderName ?? string.Empty);
                listNotificationPopup.Add(popUpNotificationModel);
            }
            return listNotificationPopup;
        }


        private List<SummaryInfItem> GetNotification(int hpId, long ptId, int sinDate, int userId)
        {
            List<SummaryInfItem> listNotification = GetNotificationContent(hpId, ptId, userId, sinDate);
            SummaryInfItem? changedLast = null;
            if (listNotification.Count >= 1)
            {
                changedLast = listNotification.Last().ChangeSpaceHeaderInf(0);
            }
            if (changedLast != null)
            {
                listNotification.Remove(listNotification.Last());
                listNotification.Add(changedLast);
            }
            return listNotification;
        }


        private List<SummaryInfItem> GetNotificationContent(int hpId, long ptId, int userId, int sinDate)
        {
            List<SummaryInfItem> listNotification = new();
            var listNotifiProperty = _userConfRepository.GetList(hpId, userId, 915).Where(x => x.Val != 0).ToList();
            var listNotifiSort = _userConfRepository.GetList(hpId, userId, 916).OrderBy(x => x.Val).ToList();
            foreach (var sort in listNotifiSort)
            {
                var userConfiguration = listNotifiProperty.FirstOrDefault(item => item.GrpItemCd == sort.GrpItemCd);
                if (userConfiguration != null)
                {
                    var summaryInfItem = GetNotificationInfoToList(hpId, ptId, sinDate, userId, userConfiguration);
                    if (summaryInfItem != null)
                    {
                        listNotification.Add(summaryInfItem);
                    }
                }
            }

            return listNotification;
        }

        private SummaryInfItem GetNotificationInfoToList(int hpId, long ptId, int sinDate, int userId, UserConfModel userConfNoti)
        {
            SummaryInfItem summaryInfItem = new SummaryInfItem();
            int grpItemCd = 0;
            string propertyColor = "";
            string headerName = summaryInfItem.HeaderName;
            string headerInfo = summaryInfItem.HeaderInfo;
            double spaceHeaderName = 0;
            double spaceHeaderInfo = 0;

            switch (userConfNoti.GrpItemCd.AsString())
            {
                case "1":
                    //身体情報
                    GetPhysicalInfo(hpId, ptId, sinDate, summaryInfItem);
                    grpItemCd = 1;
                    break;
                case "2":
                    //アレルギー 
                    GetDrugInfo(ptId, sinDate, summaryInfItem);
                    grpItemCd = 2;
                    break;
                case "3":
                    // 病歴
                    GetPathologicalStatus(ptId, summaryInfItem);
                    grpItemCd = 3;
                    break;
                case "4":
                    // 服薬情報
                    GetInteraction(ptId, sinDate, summaryInfItem);
                    grpItemCd = 4;
                    break;
                case "5":
                    //生活歴
                    GetLifeHistory(hpId, ptId, summaryInfItem);
                    grpItemCd = 5;
                    break;
                case "6":
                    //コメント
                    GetComment(hpId, ptId, summaryInfItem);
                    grpItemCd = 6;
                    break;
                case "7":
                    //経過日数
                    GetCalculationInfo(hpId, ptId, sinDate, summaryInfItem);
                    headerName = "◆経過日数";
                    grpItemCd = 7;
                    break;
                case "8":
                    //出産予定
                    GetReproductionInfo(ptId, sinDate, summaryInfItem);
                    grpItemCd = 8;
                    break;
                case "9":
                    //予約情報
                    GetReservationInf(hpId, ptId, sinDate, summaryInfItem);
                    grpItemCd = 9;
                    break;
                    //case "0":
                    //    //検査結果速報
                    //    ptInfNotificationModel.HeaderName = "■検査結果速報";
                    //    ptInfNotificationModel.GrpItemCd = 0;
                    //    break;
            }
            if (!string.IsNullOrEmpty(headerInfo))
            {

                headerName = headerName.Replace("◆", "【");
                headerName = headerName.Replace("■", "【");
                headerName = headerName.Insert(headerName.Length, "】");
                if (headerName.Contains("【コメント】"))
                {
                    headerInfo = headerInfo.Replace(Environment.NewLine, " ・ ");
                    headerInfo = headerInfo.Trim();
                    headerInfo = headerInfo.TrimEnd('・');
                }
                else
                {
                    headerInfo = headerInfo.Replace(Environment.NewLine, "    ");
                }
                headerInfo = headerInfo.Trim();

                if (!string.IsNullOrEmpty(headerInfo))
                {
                    var colorTextNotifi = _userConfRepository.GetListUserConf(hpId, userId, 917).Where(x => x.GrpItemCd == userConfNoti.GrpItemCd).FirstOrDefault();
                    if (colorTextNotifi != null)
                    {
                        propertyColor = colorTextNotifi.Param;
                    }

                    spaceHeaderName = 5.0;
                    spaceHeaderInfo = 30.0;
                    var result = new SummaryInfItem(headerInfo, headerName, propertyColor, spaceHeaderName, spaceHeaderInfo, summaryInfItem.HeaderNameSize, grpItemCd, summaryInfItem.Text);
                    return summaryInfItem;
                }
            }
            return new();
        }

        private SummaryInfItem GetSummaryInfo(int hpId, long ptId, int sinDate, string propertyCd, int headerType, long raiinNo, InfoType infoType = InfoType.PtHeaderInfo)
        {
            SummaryInfItem summaryInfItem = new SummaryInfItem();

            GetData(hpId, ptId, sinDate, propertyCd, ref summaryInfItem);

            if (infoType == InfoType.PtHeaderInfo)
            {
                switch (propertyCd)
                {
                    case "C":
                        GetPhoneNumber(hpId, ptId, summaryInfItem);
                        //電話番号
                        break;
                    case "D":
                        GetReceptionComment(hpId, ptId, sinDate, raiinNo, summaryInfItem);
                        //受付コメント
                        break;
                    case "E":
                        GetFamilyList(hpId, ptId, sinDate, summaryInfItem);
                        //家族歴
                        break;
                }
            }

            summaryInfItem = summaryInfItem.ChangePropertyColor("000000");

            return summaryInfItem;
        }

        private void GetData(int hpId, long ptId, int sinDate, string propertyCd, ref SummaryInfItem summaryInfItem)
        {
            switch (propertyCd)
            {
                case "1":
                    //身体情報
                    GetPhysicalInfo(hpId, ptId, sinDate, summaryInfItem);
                    break;
                case "2":
                    //アレルギー 
                    GetDrugInfo(ptId, sinDate, summaryInfItem);
                    break;
                case "3":
                    // 病歴
                    GetPathologicalStatus(ptId, summaryInfItem);
                    break;
                case "4":
                    // 服薬情報
                    GetInteraction(ptId, sinDate, summaryInfItem);
                    break;
                case "5":
                    //算定情報
                    GetCalculationInfo(hpId, ptId, sinDate, summaryInfItem);
                    break;
                case "6":
                    //出産予定
                    GetReproductionInfo(ptId, sinDate, summaryInfItem);
                    break;
                case "7":
                    //予約情報
                    GetReservationInf(hpId, ptId, sinDate, summaryInfItem);
                    break;
                case "8":
                    //コメント
                    GetComment(hpId, ptId, summaryInfItem);
                    break;
                case "9":
                    //住所
                    GetAddress(hpId, ptId, summaryInfItem);
                    break;
                case "A":
                    //保険情報
                    GetInsuranceInfo(hpId, ptId, sinDate, summaryInfItem);
                    break;
                case "B":
                    //生活歴
                    GetLifeHistory(hpId, ptId, summaryInfItem);
                    break;
            }
        }

        private void GetPhysicalInfo(int hpId, long ptId, int sinDate, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 1;
            string headerName = "■身体情報";
            string headerInfo = summaryInfItem.HeaderInfo;
            List<KensaInfDetailModel> listKensaInfDetailModel = _specialNotePatientInfRepository.GetListKensaInfDetailModel(hpId, ptId, sinDate);
            long maxSortNo = listKensaInfDetailModel.Max(u => u.SortNo);
            KensaInfDetailModel heightModel = listKensaInfDetailModel.Where(u => u.KensaItemCd == IraiCodeConstant.HEIGHT_CODE).FirstOrDefault() ?? new();
            KensaInfDetailModel weightModel = listKensaInfDetailModel.Where(u => u.KensaItemCd == IraiCodeConstant.WEIGHT_CODE).FirstOrDefault() ?? new();
            KensaInfDetailModel bmiModel = listKensaInfDetailModel.Where(u => u.KensaItemCd == IraiCodeConstant.BMI_CODE).FirstOrDefault() ?? new();
            var newlistKensaInfDetailModel = new List<KensaInfDetailModel>();
            newlistKensaInfDetailModel.AddRange(listKensaInfDetailModel.Where(u => u != bmiModel));

            if (heightModel == null ||
                weightModel == null ||
                heightModel.ResultVal.AsDouble() <= 0 ||
                weightModel.ResultVal.AsDouble() <= 0)
            {
                if (bmiModel != null)
                {
                    listKensaInfDetailModel.Remove(bmiModel);
                }
            }
            else
            {
                string bmi = string.Format("{0:0.0}", weightModel.ResultVal.AsDouble() / (heightModel.ResultVal.AsDouble() * heightModel.ResultVal.AsDouble() / 10000));
                if (bmiModel != null)
                {
                    string resultVal = bmi;
                    int iraiDate = heightModel.IraiDate >= weightModel.IraiDate ? heightModel.IraiDate : weightModel.IraiDate;
                    var newBMIModel = new KensaInfDetailModel(iraiDate, resultVal);
                    newlistKensaInfDetailModel.Add(newBMIModel);
                }
                else
                {
                    if (bmi.AsDouble() > 0)
                    {
                        listKensaInfDetailModel.Add(
                        new KensaInfDetailModel(
                            ++maxSortNo,
                            "BMI",
                            IraiCodeConstant.BMI_CODE,
                            bmi
                        ));
                    }
                }
            }
            newlistKensaInfDetailModel = newlistKensaInfDetailModel.OrderBy(u => u.SortNo).ToList();
            foreach (var kensaDetailModel in newlistKensaInfDetailModel)
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
                    headerInfo += (string.IsNullOrEmpty(kensaName) ? string.Empty : kensaName + ":" + space) + kensaDetailModel.ResultVal + kensaDetailModel.Unit + space + (string.IsNullOrEmpty(sSate) ? string.Empty : "(" + sSate + ")") + space + "/";
                }
            }
            headerInfo = headerInfo.TrimEnd('/') ?? string.Empty;

            summaryInfItem = new SummaryInfItem(headerInfo, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetDrugInfo(long ptId, int sinDate, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 2;
            string headerName = "◆アレルギー";
            StringBuilder headerInf = new StringBuilder();
            List<PtAlrgyElseModel> listPtAlrgyElseModel = new List<PtAlrgyElseModel>();
            List<PtAlrgyFoodModel> listPtAlrgyFoodModel = new List<PtAlrgyFoodModel>();
            List<PtAlrgyDrugModel> listPtAlrgyDrugModel = new List<PtAlrgyDrugModel>();

            listPtAlrgyElseModel = _importantNoteRepository.GetAlrgyElseList(ptId, sinDate);
            listPtAlrgyFoodModel = _importantNoteRepository.GetAlrgyFoodList(ptId, sinDate);
            listPtAlrgyDrugModel = _importantNoteRepository.GetAlrgyDrugList(ptId, sinDate);

            foreach (var ptAlrgyDrugModel in listPtAlrgyDrugModel)
            {
                if (!string.IsNullOrEmpty(ptAlrgyDrugModel.DrugName))
                {
                    headerInf.Append(ptAlrgyDrugModel.DrugName);
                    if (!string.IsNullOrEmpty(ptAlrgyDrugModel.Cmt))
                    {
                        headerInf.Append("／" + ptAlrgyDrugModel.Cmt);
                    }
                    headerInf.Append(Environment.NewLine);
                }
            }

            foreach (var ptAlrgyFoodModel in listPtAlrgyFoodModel)
            {
                if (!string.IsNullOrEmpty(ptAlrgyFoodModel.FoodName))
                {
                    headerInf.Append(ptAlrgyFoodModel.FoodName);
                    if (!string.IsNullOrEmpty(ptAlrgyFoodModel.Cmt))
                    {
                        headerInf.Append("／" + ptAlrgyFoodModel.Cmt);
                    }
                    headerInf.Append(Environment.NewLine);
                }
            }

            foreach (var ptAlrgyElseModel in listPtAlrgyElseModel)
            {
                if (!string.IsNullOrEmpty(ptAlrgyElseModel.AlrgyName))
                {
                    headerInf.Append(ptAlrgyElseModel.AlrgyName);
                    if (!string.IsNullOrEmpty(ptAlrgyElseModel.Cmt))
                    {
                        headerInf.Append("／" + ptAlrgyElseModel.Cmt);
                    }
                    headerInf.Append(Environment.NewLine);
                }
            }

            string strHeaderInf = headerInf.ToString().TrimEnd(Environment.NewLine.ToCharArray());

            summaryInfItem = new SummaryInfItem(strHeaderInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetPathologicalStatus(long ptId, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 3;
            string headerName = "◆病歴";
            StringBuilder headerInfo = new StringBuilder();
            List<PtKioRekiModel> listPtKioRekiModel = new List<PtKioRekiModel>();
            List<PtInfectionModel> listPtInfectionModel = new List<PtInfectionModel>();

            listPtKioRekiModel = _importantNoteRepository.GetKioRekiList(ptId);
            listPtInfectionModel = _importantNoteRepository.GetInfectionList(ptId);

            foreach (var ptKioRekiModel in listPtKioRekiModel)
            {
                if (!string.IsNullOrEmpty(ptKioRekiModel.Byomei))
                {
                    headerInfo.Append(ptKioRekiModel.Byomei);
                    if (!string.IsNullOrEmpty(ptKioRekiModel.Cmt))
                    {
                        headerInfo.Append("／" + ptKioRekiModel.Cmt);
                    }
                    headerInfo.Append(Environment.NewLine);
                }
            }
            foreach (var ptInfectionModel in listPtInfectionModel)
            {
                if (!string.IsNullOrEmpty(ptInfectionModel.Byomei))
                {
                    headerInfo.Append(ptInfectionModel.Byomei);
                    if (!string.IsNullOrEmpty(ptInfectionModel.Cmt))
                    {
                        headerInfo.Append("／" + ptInfectionModel.Cmt);
                    }
                    headerInfo.Append(Environment.NewLine);
                }
            }
            string strHeaderInfo = headerInfo.ToString().TrimEnd(Environment.NewLine.ToCharArray());
            summaryInfItem = new SummaryInfItem(strHeaderInfo, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetInteraction(long ptId, int sinDate, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 4;
            string headerName = "◆服薬情報";
            StringBuilder headerInf = new StringBuilder();
            List<PtOtherDrugModel> listPtOtherDrugModel = new List<PtOtherDrugModel>();
            List<PtOtcDrugModel> listPtOtcDrugModel = new List<PtOtcDrugModel>();
            List<PtSuppleModel> listPtSuppleModel = new List<PtSuppleModel>();

            listPtOtherDrugModel = _importantNoteRepository.GetOtherDrugList(ptId, sinDate);
            listPtOtcDrugModel = _importantNoteRepository.GetOtcDrugList(ptId, sinDate);
            listPtSuppleModel = _importantNoteRepository.GetSuppleList(ptId, sinDate);

            foreach (var ptOtherDrugModel in listPtOtherDrugModel)
            {
                if (!string.IsNullOrEmpty(ptOtherDrugModel.DrugName))
                {
                    headerInf.Append(ptOtherDrugModel.DrugName);
                    if (!string.IsNullOrEmpty(ptOtherDrugModel.Cmt))
                    {
                        headerInf.Append("／" + ptOtherDrugModel.Cmt);
                    }
                    headerInf.Append(Environment.NewLine);
                }
            }
            foreach (var ptOtcDrugModel in listPtOtcDrugModel)
            {
                if (!string.IsNullOrEmpty(ptOtcDrugModel.TradeName))
                {
                    headerInf.Append(ptOtcDrugModel.TradeName);
                    if (!string.IsNullOrEmpty(ptOtcDrugModel.Cmt))
                    {
                        headerInf.Append("／" + ptOtcDrugModel.Cmt);
                    }
                    headerInf.Append(Environment.NewLine);
                }
            }
            foreach (var suppleModel in listPtSuppleModel)
            {
                if (!string.IsNullOrEmpty(suppleModel.IndexWord))
                {
                    headerInf.Append(suppleModel.IndexWord);
                    if (!string.IsNullOrEmpty(suppleModel.Cmt))
                    {
                        headerInf.Append("／" + suppleModel.Cmt);
                    }
                    headerInf.Append(Environment.NewLine);
                }
            }
            string strHeaderInfo = headerInf.ToString().TrimEnd(Environment.NewLine.ToCharArray());
            summaryInfItem = new SummaryInfItem(strHeaderInfo, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetCalculationInfo(int hpId, long ptId, int sinDate, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 5;
            string headerName = "◆算定情報";
            string headerInf = "";
            List<SanteiInfModel> listSanteiInfModels = _santeiInfRepository.GetCalculationInfo(hpId, ptId, sinDate);
            if (listSanteiInfModels.Count > 0)
            {
                listSanteiInfModels = listSanteiInfModels.Where(u => u.DayCount > u.AlertDays).ToList();
                foreach (var santeiInfomationModel in listSanteiInfModels)
                {
                    headerInf += santeiInfomationModel.ItemName?.Trim() + "(" + santeiInfomationModel.KisanType + " " + CIUtil.SDateToShowSDate(santeiInfomationModel.LastOdrDate) + "～　" + santeiInfomationModel.DayCountDisplay + ")" + Environment.NewLine;
                }
                headerInf = headerInf.TrimEnd(Environment.NewLine.ToCharArray());
                summaryInfItem = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
            }
        }

        private void GetReproductionInfo(long ptId, int sinDate, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 6;
            string headerName = "■出産予定";
            StringBuilder headerInf = new StringBuilder();
            List<PtPregnancyModel> listPtPregnancyModels = new List<PtPregnancyModel>();

            listPtPregnancyModels = _specialNotePatientInfRepository.GetPregnancyList(ptId, sinDate);

            if (listPtPregnancyModels.Count > 0)
            {
                string GetSDateFromDateTime(DateTime? dateTime)
                {
                    if (dateTime == null)
                    {
                        return string.Empty;
                    }
                    return CIUtil.SDateToShowSDate(CIUtil.DateTimeToInt((DateTime)dateTime));
                };

                PtPregnancyModel ptPregnancyModel = listPtPregnancyModels.FirstOrDefault() ?? new();
                if (ptPregnancyModel.PeriodDate != 0)
                {
                    headerInf.Append("月経日(" + GetSDateFromDateTime(CIUtil.IntToDate(ptPregnancyModel.PeriodDate)) + ")" + space + "/");
                }
                if (!string.IsNullOrEmpty(ptPregnancyModel.PeriodWeek) && ptPregnancyModel.PeriodWeek != "0W0D")
                {
                    headerInf.Append("妊娠週(" + ptPregnancyModel.PeriodWeek + ")" + space + "/");
                }
                if (ptPregnancyModel.PeriodDueDate != 0)
                {
                    headerInf.Append("予定日(" + GetSDateFromDateTime(CIUtil.IntToDate(ptPregnancyModel.PeriodDueDate)) + ")" + space + "/");
                }
                if (ptPregnancyModel.OvulationDate != 0)
                {
                    headerInf.Append("排卵日(" + GetSDateFromDateTime(CIUtil.IntToDate(ptPregnancyModel.OvulationDate)) + ")" + space + "/");
                }
                if (!string.IsNullOrEmpty(ptPregnancyModel.OvulationWeek) && ptPregnancyModel.OvulationWeek != "0W0D")
                {
                    headerInf.Append("妊娠週(" + ptPregnancyModel.OvulationWeek + ")" + space + "/");
                }
                if (ptPregnancyModel.OvulationDueDate != 0)
                {
                    headerInf.Append("予定日(" + GetSDateFromDateTime(CIUtil.IntToDate(ptPregnancyModel.OvulationDueDate)) + ")");
                }
                string strHeaderInfo = headerInf.ToString().TrimEnd('/');
                summaryInfItem = new SummaryInfItem(strHeaderInfo, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
            }
        }

        private void GetReservationInf(int hpId, long ptId, int sinDate, SummaryInfItem summaryInfItem)
        {
            int today = DateTime.Now.ToString("yyyyMMdd").AsInteger();
            int grpItemCd = 7;
            string headerName = "■予約情報";
            string headerInf = "";
            var listRsvInfModel = _rsvInfRepository.GetList(hpId, ptId, sinDate);

            if (listRsvInfModel.Count > 0)
            {
                listRsvInfModel = listRsvInfModel.OrderBy(u => u.SinDate).ToList();
                foreach (RsvInfModel rsvInfModel in listRsvInfModel)
                {
                    if (rsvInfModel.PtId == 0 && rsvInfModel.HpId == 0 && rsvInfModel.RsvFrameId == 0)
                    {
                        //formart for RsvInf
                        string startTime = rsvInfModel.StartTime > 0 ? space + CIUtil.TimeToShowTime(rsvInfModel.StartTime) + space : space;
                        string rsvFrameName = string.IsNullOrEmpty(rsvInfModel.RsvFrameName) ? string.Empty : "[" + rsvInfModel.RsvFrameName + "]";
                        headerInf += CIUtil.SDateToShowSDate2(rsvInfModel.SinDate) + startTime + rsvInfModel.RsvGrpName + space + rsvFrameName + Environment.NewLine;
                    }
                    else
                    {
                        //formart for raiinInf
                        string kaName = string.IsNullOrEmpty(rsvInfModel.KaSName) ? space : space + "[" + rsvInfModel.KaSName + "]" + space;
                        headerInf += CIUtil.SDateToShowSDate2(rsvInfModel.SinDate) + space
                                                        + FormatTime(rsvInfModel.YoyakuTime)
                                                        + kaName
                                                        + rsvInfModel.TantoName + space
                                                        + (!string.IsNullOrEmpty(rsvInfModel.RaiinCmt) ? "(" + rsvInfModel.RaiinCmt + ")" : string.Empty)
                                                        + Environment.NewLine;
                    }
                }
            }
            headerInf = headerInf.TrimEnd(Environment.NewLine.ToCharArray());
            summaryInfItem = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetComment(int hpId, long ptId, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 8;
            string headerName = "■コメント";
            string headerInf = "";
            PtCmtInfModel ptCmtInfModel = _ptCmtInfRepository.GetPtCmtInfo(hpId, ptId);
            if (ptCmtInfModel != null && !string.IsNullOrEmpty(ptCmtInfModel.Text))
            {
                headerInf += ptCmtInfModel.Text + Environment.NewLine;
            }
            headerInf = headerInf.TrimEnd(Environment.NewLine.ToCharArray());
            summaryInfItem = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetAddress(int hpId, long ptId, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 9;
            string headerName = "◆住所";
            string headerInf = "";
            var ptInfModel = _patientInfRepository.GetPtInf(hpId, ptId);
            if (ptInfModel != null && !string.IsNullOrEmpty(ptInfModel.HomeAddress1 + ptInfModel.HomeAddress2))
            {
                headerInf = ptInfModel.HomeAddress1 + space + ptInfModel.HomeAddress2;
            }
            summaryInfItem = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetInsuranceInfo(int hpId, long ptId, int sinDate, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 10;
            string headerName = "◆保険情報";
            var ptHoken = _insuranceRepository.GetInsuranceListById(hpId, ptId, sinDate);
            var ptHokenInfs = ptHoken.ListInsurance;
            if (ptHokenInfs.Count == 0) return;
            StringBuilder futanInfo = new StringBuilder();
            StringBuilder kohiInf = new StringBuilder();
            string headerInf = "";
            foreach (var ptHokenInfoModel in ptHokenInfs)
            {
                kohiInf.Clear();
                if (!ptHokenInfoModel.IsEmptyKohi1)
                {
                    kohiInf.Append(GetFutanInfo(ptHokenInfoModel.Kohi1));
                }
                if (!ptHokenInfoModel.IsEmptyKohi2)
                {
                    kohiInf.Append(GetFutanInfo(ptHokenInfoModel.Kohi2));
                }
                if (!ptHokenInfoModel.IsEmptyKohi3)
                {
                    kohiInf.Append(GetFutanInfo(ptHokenInfoModel.Kohi3));
                }
                if (!ptHokenInfoModel.IsEmptyKohi4)
                {
                    kohiInf.Append(GetFutanInfo(ptHokenInfoModel.Kohi4));
                }
                if (string.IsNullOrEmpty(kohiInf.ToString()))
                {
                    continue;
                }
                string strKohiInf = kohiInf.ToString();
                strKohiInf = strKohiInf.TrimEnd();
                strKohiInf = strKohiInf.TrimEnd('　');
                strKohiInf = strKohiInf.TrimEnd(',');
                futanInfo.Append(ptHokenInfoModel.HokenPid.ToString().PadLeft(3, '0') + ". ");
                futanInfo.Append(strKohiInf);
                futanInfo.Append(Environment.NewLine);
            }
            string strFutanInfo = futanInfo.ToString();
            strFutanInfo = strFutanInfo.TrimEnd();
            if (!string.IsNullOrEmpty(strFutanInfo?.Trim()))
            {
                headerInf = strFutanInfo;
            }
            summaryInfItem = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private string GetFutanInfo(KohiInfModel ptKohi)
        {
            HokenMstModel hokenMst = ptKohi.HokenMstModel;
            int gokenGaku = ptKohi.GendoGaku;
            StringBuilder futanInfo = new StringBuilder();

            if (!string.IsNullOrEmpty(ptKohi.FutansyaNo))
            {
                futanInfo.Append("[" + ptKohi.FutansyaNo + "]");
            }
            else
            {
                if (hokenMst == null)
                {
                    return string.Empty;
                }
                futanInfo.Append("[" + hokenMst.Houbetu + "]");
            }

            if (hokenMst == null && !string.IsNullOrEmpty(ptKohi.FutansyaNo))
            {
                return futanInfo + "," + space;
            }
            if (hokenMst.FutanKbn == 0)
            {
                //負担なし
                futanInfo.Append("0円");
            }
            else
            {
                if (hokenMst.KaiLimitFutan > 0)
                {
                    if (hokenMst.DayLimitFutan <= 0 && hokenMst.MonthLimitFutan <= 0 && gokenGaku > 0)
                    {
                        futanInfo.Append(gokenGaku.AsString() + "円/回・");
                    }
                    else
                    {
                        futanInfo.Append(hokenMst.KaiLimitFutan.AsString() + "円/回・");
                    }
                }

                if (hokenMst.DayLimitFutan > 0)
                {
                    if (hokenMst.KaiLimitFutan <= 0 && hokenMst.MonthLimitFutan <= 0 && gokenGaku > 0)
                    {
                        futanInfo.Append(gokenGaku.AsString() + "円/日・");
                    }
                    else
                    {
                        futanInfo.Append(hokenMst.DayLimitFutan.AsString() + "円/日・");
                    }
                }

                if (hokenMst.DayLimitCount > 0)
                {
                    futanInfo.Append(hokenMst.DayLimitCount.AsString() + "回/日・");
                }

                if (hokenMst.MonthLimitFutan > 0)
                {
                    if (hokenMst.KaiLimitFutan <= 0 && hokenMst.DayLimitFutan <= 0 && gokenGaku > 0)
                    {
                        futanInfo.Append(gokenGaku.AsString() + "円/月・");
                    }
                    else
                    {
                        futanInfo.Append(hokenMst.MonthLimitFutan.AsString() + "円/月・");
                    }
                }

                if (hokenMst.MonthLimitCount > 0)
                {
                    futanInfo.Append(hokenMst.MonthLimitCount.AsString() + "回/月");
                }
            }
            string strFutanInfo = futanInfo.ToString();
            if (!string.IsNullOrEmpty(strFutanInfo))
            {
                strFutanInfo = strFutanInfo.TrimEnd('・');
                strFutanInfo = strFutanInfo + "," + space;
            }

            return strFutanInfo;
        }

        private void GetPhoneNumber(int hpId, long ptId, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 12;
            string headerName = "◆電話番号";
            var ptInfModel = _patientInfRepository.GetPtInf(hpId, ptId);
            string headerInf = "";
            if (ptInfModel != null && !string.IsNullOrEmpty(ptInfModel.Tel1 + ptInfModel.Tel2))
            {
                headerInf = ptInfModel.Tel1 + Environment.NewLine + ptInfModel.Tel2;
            }
            summaryInfItem = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetLifeHistory(int hpId, long ptId, SummaryInfItem summaryInfItem)
        {
            var grpItemCd = 11;
            var headerName = "■生活歴";
            string headerInf = "";
            var seikaturekiInfModel = _specialNotePatientInfRepository.GetSeikaturekiInfList(ptId, hpId).FirstOrDefault();
            if (seikaturekiInfModel != null)
            {
                headerInf = seikaturekiInfModel.Text;
            }
            summaryInfItem = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetFamilyList(int hpId, long ptId, int sinDate, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 14;
            string headerName = "◆家族歴";
            string headerInf = "";
            var ptFamilyList = _familyRepository.GetFamilyListByPtId(hpId, ptId, sinDate);
            if (ptFamilyList != null)
            {
                string headerInfo = string.Empty;
                foreach (var ptFamilyModel in ptFamilyList)
                {
                    SetDiseaseName(ptFamilyModel);
                    if (!string.IsNullOrWhiteSpace(ptFamilyModel.DiseaseName))
                    {
                        if (!string.IsNullOrEmpty(headerInfo))
                        {
                            headerInfo += Environment.NewLine;
                        }
                        headerInfo += $"({GetRelationshipName(ptFamilyModel.ZokugaraCd)}){ptFamilyModel.DiseaseName}";
                    }
                }

                if (!string.IsNullOrEmpty(headerInfo))
                {
                    headerInf = headerInfo;
                }
            }
            summaryInfItem = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private string GetRelationshipName(string zokugaraCd)
        {
            var relationshipName = string.Empty;

            if (_relationship.Count == 0)
            {
                _relationship = new Dictionary<string, string>()
                {
                    {nameof(RelationshipEnum.BR), "血縁"},
                    {nameof(RelationshipEnum.MA), "配偶者"},
                    {nameof(RelationshipEnum.FA), "父"},
                    {nameof(RelationshipEnum.MO), "母"},

                    {nameof(RelationshipEnum.GF1), "祖父(父方)"},
                    {nameof(RelationshipEnum.GM1), "祖母(父方)"},
                    {nameof(RelationshipEnum.GF2), "祖父(母方)"},
                    {nameof(RelationshipEnum.GM2), "祖母(母方)"},

                    {nameof(RelationshipEnum.SO), "息子"},
                    {nameof(RelationshipEnum.DA), "娘"},
                    {nameof(RelationshipEnum.BB), "兄"},
                    {nameof(RelationshipEnum.LB), "弟"},

                    {nameof(RelationshipEnum.BS), "姉"},
                    {nameof(RelationshipEnum.LS), "妹"},
                    {nameof(RelationshipEnum.GC), "孫"},
                    {nameof(RelationshipEnum.OT), "非血縁"},
                };
            }

            if (_relationship.Keys.Contains(zokugaraCd))
            {
                relationshipName = _relationship[zokugaraCd];
            }

            return relationshipName;
        }

        private void GetReceptionComment(int hpId, long ptId, int sinDate, long raiinNo, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 13;
            string headerName = "◆来院コメント";
            string textRaiinCmtInf = _raiinCmtInfRepository.GetRaiinCmtByPtId(hpId, ptId, sinDate, raiinNo);
            summaryInfItem = new SummaryInfItem(textRaiinCmtInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
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

        public void SetDiseaseName(FamilyModel ptFamilyModel)
        {
            string diseaseName = string.Empty;
            if (ptFamilyModel.ListPtFamilyRekis != null && ptFamilyModel.ListPtFamilyRekis.Count > 0)
            {
                foreach (PtFamilyRekiModel ptByomeiMode in ptFamilyModel.ListPtFamilyRekis)
                {
                    if (!ptByomeiMode.IsDeleted)
                    {
                        if (!string.IsNullOrEmpty(ptFamilyModel.DiseaseName))
                        {
                            diseaseName += "・";
                        }
                        diseaseName += ptByomeiMode.Byomei;
                    }
                }
            }
            ptFamilyModel = new FamilyModel(
                    ptFamilyModel.SeqNo,
                    ptFamilyModel.ZokugaraCd,
                    ptFamilyModel.FamilyPtNum,
                    ptFamilyModel.Name,
                    ptFamilyModel.Sex,
                    ptFamilyModel.Birthday,
                    ptFamilyModel.Age,
                    ptFamilyModel.IsDead,
                    ptFamilyModel.IsSeparated,
                    ptFamilyModel.Biko,
                    ptFamilyModel.SortNo,
                    ptFamilyModel.ListPtFamilyRekis ?? new(),
                    diseaseName
                );
        }
    }
}
