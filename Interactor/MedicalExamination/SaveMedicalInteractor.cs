﻿using Domain.Models.Diseases;
using Domain.Models.Family;
using Domain.Models.FlowSheet;
using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.Ka;
using Domain.Models.KarteInf;
using Domain.Models.KarteInfs;
using Domain.Models.Lock;
using Domain.Models.Medical;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Domain.Models.SystemGenerationConf;
using Domain.Models.TodayOdr;
using Domain.Models.User;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Interactor.CalculateService;
using Interactor.Family.ValidateFamilyList;
using Interactor.NextOrder;
using Microsoft.Extensions.Options;
using UseCase.Accounting.Recaculate;
using UseCase.Diseases.Upsert;
using UseCase.Family;
using UseCase.FlowSheet.Upsert;
using UseCase.MedicalExamination.SaveMedical;
using UseCase.MedicalExamination.UpsertTodayOrd;
using static Helper.Constants.KarteConst;
using static Helper.Constants.OrderInfConst;
using DiseaseValidationStatus = Helper.Constants.PtDiseaseConst.ValidationStatus;

namespace Interactor.MedicalExamination;

public class SaveMedicalInteractor : ISaveMedicalInputPort
{
    private readonly IOrdInfRepository _ordInfRepository;
    private readonly IReceptionRepository _receptionRepository;
    private readonly IKaRepository _kaRepository;
    private readonly IMstItemRepository _mstItemRepository;
    private readonly ISystemGenerationConfRepository _systemGenerationConfRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceInforRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly ITodayOdrRepository _todayOdrRepository;
    private readonly IKarteInfRepository _karteInfRepository;
    private readonly ISaveMedicalRepository _saveMedicalRepository;
    private readonly IValidateFamilyList _validateFamilyList;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly ICalculateService _calculateService;
    private readonly AmazonS3Options _options;

    public SaveMedicalInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service, IOrdInfRepository ordInfRepository, IReceptionRepository receptionRepository, IKaRepository kaRepository, IMstItemRepository mstItemRepository, ISystemGenerationConfRepository systemGenerationConfRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceInforRepository, IUserRepository userRepository, IHpInfRepository hpInfRepository, ISaveMedicalRepository saveMedicalRepository, ITodayOdrRepository todayOdrRepository, IKarteInfRepository karteInfRepository, ICalculateService calculateService, IValidateFamilyList validateFamilyList)
    {
        _amazonS3Service = amazonS3Service;
        _options = optionsAccessor.Value;
        _ordInfRepository = ordInfRepository;
        _kaRepository = kaRepository;
        _receptionRepository = receptionRepository;
        _mstItemRepository = mstItemRepository;
        _systemGenerationConfRepository = systemGenerationConfRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceInforRepository = insuranceInforRepository;
        _userRepository = userRepository;
        _hpInfRepository = hpInfRepository;
        _todayOdrRepository = todayOdrRepository;
        _saveMedicalRepository = saveMedicalRepository;
        _karteInfRepository = karteInfRepository;
        _calculateService = calculateService;
        _validateFamilyList = validateFamilyList;
    }

    public SaveMedicalOutputData Handle(SaveMedicalInputData inputDatas)
    {
        try
        {
            var notAllowSave = _userRepository.NotAllowSaveMedicalExamination(inputDatas.HpId, inputDatas.PtId, inputDatas.RaiinNo, inputDatas.SinDate, inputDatas.UserId);
            if (notAllowSave)
            {
                return new SaveMedicalOutputData(
                       SaveMedicalStatus.MedicalScreenLocked,
                       RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid,
                       new(),
                       KarteValidationStatus.Valid,
                       ValidateFamilyListStatus.ValidateSuccess,
                       UpsertFlowSheetStatus.Valid,
                       UpsertPtDiseaseListStatus.Valid,
                       0,
                       0,
                       0
                       );
            }

            //Raiin Info
            var inputDataList = inputDatas.OdrItems.ToList();
            var hpIds = inputDataList.Select(x => x.HpId).ToList();
            var ptIds = inputDataList.Select(x => x.PtId).ToList();
            var raiinNos = inputDataList.Select(x => x.RaiinNo).ToList();
            var sinDates = inputDataList.Select(x => x.SinDate).ToList();
            if (inputDatas.KarteInf.HpId != 0 && inputDatas.KarteInf.PtId != 0 && inputDatas.KarteInf.SinDate != 0 && inputDatas.KarteInf.RaiinNo != 0)
            {
                hpIds.Add(inputDatas.KarteInf.HpId);
                ptIds.Add(inputDatas.KarteInf.PtId);
                raiinNos.Add(inputDatas.KarteInf.RaiinNo);
                sinDates.Add(inputDatas.KarteInf.SinDate);
            }
            var hpId = inputDatas.HpId;
            var ptId = inputDatas.PtId;

            ptIds.Add(ptId);
            hpIds.Add(hpId);
            ptIds = ptIds.Distinct().ToList();
            hpIds = hpIds.Distinct().ToList();
            raiinNos = raiinNos.Distinct().ToList();
            sinDates = sinDates.Distinct().ToList();

            var raiinNo = raiinNos.Any() ? raiinNos[0] : 0;
            var sinDate = sinDates.Any() ? sinDates[0] : 0;

            var raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid;
            if (inputDatas.Status != (byte)ModeSaveData.TempSave)
            {
                raiinInfStatus = CheckCommon(inputDataList.Count > 0, hpIds, ptIds, raiinNos, sinDates, hpId, ptId, raiinNo);

                if (raiinInfStatus != RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid)
                {
                    return new SaveMedicalOutputData(
                        SaveMedicalStatus.Failed,
                        raiinInfStatus,
                        new Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>(),
                        KarteValidationStatus.Valid,
                        ValidateFamilyListStatus.ValidateSuccess,
                        UpsertFlowSheetStatus.Valid,
                        UpsertPtDiseaseListStatus.Valid,
                        0,
                        0,
                        0
                        );
                }

                raiinInfStatus = CheckRaiinInf(inputDatas);
            }

            //Odr
            var resultOrder = CheckOrder(hpId, ptId, sinDate, inputDatas, inputDataList, inputDatas.Status);
            var allOdrInfs = resultOrder.Item2;

            // Karte
            var karteModel = new KarteInfModel(
                    inputDatas.KarteInf.HpId,
                    inputDatas.KarteInf.RaiinNo,
                    1,
                    0,
                    inputDatas.KarteInf.PtId,
                    inputDatas.KarteInf.SinDate,
                    inputDatas.KarteInf.Text,
                    inputDatas.KarteInf.IsDeleted,
                    inputDatas.KarteInf.RichText,
                    DateTime.MinValue,
                    DateTime.MinValue,
                    ""
                );
            KarteValidationStatus validateKarte = KarteValidationStatus.Valid;

            if (karteModel.PtId > 0 && karteModel.HpId > 0 && karteModel.RaiinNo > 0 && karteModel.SinDate > 0)
            {
                validateKarte = karteModel.Validation();
            }

            // Validate family
            var validateFamilyList = ValidateFamilyListStatus.ValidateSuccess;
            if (inputDatas.FamilyList.Count > 0)
            {
                validateFamilyList = _validateFamilyList.ValidateData(hpId, ptId, inputDatas.FamilyList);
            }

            var validateFlowsheet = UpsertFlowSheetStatus.Valid;
            // Validate flowsheet
            if (inputDatas.FlowSheetItems.Count > 0)
            {
                validateFlowsheet = ValidateFlowSheet(inputDatas.FlowSheetItems);
            }

            // Validate disease
            var ptDiseaseModels = inputDatas.UpsertPtDiseaseListInputItems.Select(i => new PtDiseaseModel(
                     i.HpId,
                     i.PtId,
                     i.SeqNo,
                     i.ByomeiCd,
                     i.SortNo,
                     i.PrefixList,
                     i.SuffixList,
                     i.Byomei,
                     i.StartDate,
                     i.TenkiKbn,
                     i.TenkiDate,
                     i.SyubyoKbn,
                     i.SikkanKbn,
                     i.NanByoCd,
                     i.IsNodspRece,
                     i.IsNodspKarte,
                     i.IsDeleted,
                     i.Id,
                     i.IsImportant,
                     0,
                     "",
                     "",
                     "",
                     "",
                     i.HokenPid,
                     i.HosokuCmt
                 )).ToList();
            var validateDisease = ValidateDiseaseList(ptDiseaseModels);

            if (raiinInfStatus != RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid || validateKarte != KarteValidationStatus.Valid || resultOrder.Item1.Any() || validateFamilyList != ValidateFamilyListStatus.ValidateSuccess || validateFlowsheet != UpsertFlowSheetStatus.Valid && validateDisease != UpsertPtDiseaseListStatus.Valid)
            {
                return new SaveMedicalOutputData(
                    SaveMedicalStatus.Failed,
                    raiinInfStatus,
                    resultOrder.Item1,
                    validateKarte,
                    validateFamilyList,
                    validateFlowsheet,
                    validateDisease,
                    0,
                    0,
                    0
                    );
            }

            var familyList = new List<FamilyModel>();
            // Family list
            if (inputDatas.FamilyList.Any())
            {
                familyList = ConvertToFamilyList(inputDatas.FamilyList);
            }

            // Next Order
            var ipnCds = new List<Tuple<string, string>>();
            foreach (var nextOrder in inputDatas.NextOrderItems)
            {
                foreach (var orderInfModel in nextOrder.RsvKrtOrderInfItems)
                {
                    ipnCds.AddRange(_mstItemRepository.GetCheckIpnCds(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.IpnCd.Trim()).ToList()));
                }
            }
            var nextOrderModels = inputDatas.NextOrderItems.Select(n => NextOrderCommon.ConvertNextOrderToModel(inputDatas.HpId, inputDatas.PtId, ipnCds, n)).ToList();

            //Special Note
            var summaryTab = inputDatas.SpecialNoteItem.SummaryTab;
            var summaryInfModel = new SummaryInfModel(summaryTab.Id, summaryTab.HpId, summaryTab.PtId, summaryTab.SeqNo, summaryTab.Text, summaryTab.Rtext, CIUtil.GetJapanDateTimeNow(), CIUtil.GetJapanDateTimeNow());
            var patientInfTab = new PatientInfoModel(inputDatas.SpecialNoteItem.PatientInfoTab.PregnancyItems.Select(p => new PtPregnancyModel(
                        p.Id,
                        p.HpId,
                        p.PtId,
                        p.SeqNo,
                        p.StartDate,
                        p.EndDate,
                        p.PeriodDate,
                        p.PeriodDueDate,
                        p.OvulationDate,
                        p.OvulationDueDate,
                        p.IsDeleted,
                        CIUtil.GetJapanDateTimeNow(),
                        inputDatas.UserId,
                        string.Empty,
                        p.SinDate

                    )).ToList(), inputDatas.SpecialNoteItem.PatientInfoTab.PtCmtInfItems, inputDatas.SpecialNoteItem.PatientInfoTab.SeikatureInfItems, new List<PhysicalInfoModel> { new PhysicalInfoModel(inputDatas.SpecialNoteItem.PatientInfoTab.KensaInfDetailItems.Select(k => new KensaInfDetailModel(k.HpId, k.PtId, k.IraiCd, k.SeqNo, k.IraiDate, k.RaiinNo, k.KensaItemCd, k.ResultVal, k.ResultType, k.AbnormalKbn, k.IsDeleted, k.CmtCd1, k.CmtCd2, DateTime.MinValue, string.Empty, string.Empty, 0)).ToList()) }, new());

            var flowSheetData = inputDatas.FlowSheetItems.Select(i => new FlowSheetModel(
                       i.SinDate,
                       i.TagNo,
                       "",
                       i.RainNo,
                       0,
                       i.Cmt,
                       0,
                       true,
                       true,
                       new List<RaiinListInfModel>(),
                       i.PtId,
                       false
                   )).ToList() ?? new List<FlowSheetModel>();

            var check = _saveMedicalRepository.Upsert(hpId, ptId, raiinNo, sinDate, inputDatas.SyosaiKbn, inputDatas.JikanKbn, inputDatas.HokenPid, inputDatas.SanteiKbn, inputDatas.TantoId, inputDatas.KaId, inputDatas.UketukeTime, inputDatas.SinStartTime, inputDatas.SinEndTime, inputDatas.Status, allOdrInfs, karteModel, inputDatas.UserId, familyList, nextOrderModels, summaryInfModel, inputDatas.SpecialNoteItem.ImportantNoteTab, patientInfTab, ptDiseaseModels, flowSheetData, inputDatas.Monshins);
            if (inputDatas.FileItem.IsUpdateFile)
            {
                if (check)
                {
                    var listFileItems = inputDatas.FileItem.ListFileItems;
                    if (!listFileItems.Any())
                    {
                        listFileItems = new List<string> { string.Empty };
                    }
                    SaveFileKarte(hpId, ptId, raiinNo, listFileItems, true);
                }
                else
                {
                    SaveFileKarte(hpId, ptId, raiinNo, inputDatas.FileItem.ListFileItems, false);
                }
            }

            if (check)
            {
                Task.Run(() =>
               _calculateService.RunCalculate(new RecaculationInputDto(
                        hpId,
                        ptId,
                        sinDate,
                        inputDatas.IsSagaku ? 1 : 0,
                        ""
                    )));
            }

            return check ?
                new SaveMedicalOutputData(
                    SaveMedicalStatus.Successed,
                    RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid,
                    new Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>(),
                    KarteValidationStatus.Valid,
                    ValidateFamilyListStatus.ValidateSuccess,
                    UpsertFlowSheetStatus.Valid,
                    UpsertPtDiseaseListStatus.Valid,
                    sinDate,
                    raiinNo,
                    ptId
                    )
                :
                new SaveMedicalOutputData(
                    SaveMedicalStatus.Failed,
                    RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid,
                    new Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>(),
                    KarteValidationStatus.Valid,
                    ValidateFamilyListStatus.ValidateSuccess,
                    UpsertFlowSheetStatus.Valid,
                    UpsertPtDiseaseListStatus.Valid,
                    sinDate,
                    raiinNo,
                    ptId
                    );
        }
        finally
        {
            _ordInfRepository.ReleaseResource();
            _kaRepository.ReleaseResource();
            _receptionRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
            _systemGenerationConfRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _insuranceInforRepository.ReleaseResource();
            _userRepository.ReleaseResource();
            _hpInfRepository.ReleaseResource();
            _todayOdrRepository.ReleaseResource();
            _karteInfRepository.ReleaseResource();
            _validateFamilyList.ReleaseResource();
        }
    }

    private void SaveFileKarte(int hpId, long ptId, long raiinNo, List<string> listFileName, bool saveSuccess)
    {
        var ptInf = _patientInforRepository.GetById(hpId, ptId, 0, 0);
        List<string> listFolders = new();
        string path = string.Empty;
        listFolders.Add(CommonConstants.Store);
        listFolders.Add(CommonConstants.Karte);
        path = _amazonS3Service.GetFolderUploadToPtNum(listFolders, ptInf != null ? ptInf.PtNum : 0);
        string host = _options.BaseAccessUrl + "/" + path;
        var listUpdates = listFileName.Select(item => item.Replace(host, string.Empty)).ToList();
        if (saveSuccess)
        {
            List<FileInfModel> fileList = new();
            var fileInfUpdateTemp = CopyFileFromDoActionToKarte(ptInf != null ? ptInf.PtNum : 0, listFileName);
            if (fileInfUpdateTemp.Any())
            {
                var checkIsSchemaList = _karteInfRepository.ListCheckIsSchema(hpId, ptId, fileInfUpdateTemp);
                foreach (var item in fileInfUpdateTemp.Select(item => item.Value))
                {
                    var isSchema = checkIsSchemaList.ContainsKey(item) && checkIsSchemaList[item];
                    fileList.Add(new FileInfModel(isSchema, item));
                }
            }

            _karteInfRepository.SaveListFileKarte(hpId, ptId, raiinNo, host, fileList, false);
        }
        else
        {
            _karteInfRepository.ClearTempData(hpId, ptId, listUpdates.ToList());
            foreach (var item in listUpdates)
            {
                _amazonS3Service.DeleteObjectAsync(path + item);
            }
        }
    }

    private Dictionary<string, string> CopyFileFromDoActionToKarte(long ptNum, List<string> listFileDo)
    {
        Dictionary<string, string> fileInfUpdateTemp = new();

        var listFolderPath = new List<string>(){
                                            CommonConstants.Store,
                                            CommonConstants.Karte
                                        };
        string baseAccessUrl = _options.BaseAccessUrl;
        string host = baseAccessUrl + "/" + _amazonS3Service.GetFolderUploadToPtNum(listFolderPath, ptNum);

        string keyNextPic = "/" + CommonConstants.Store + "/" + CommonConstants.Karte + "/" + CommonConstants.NextPic + "/";
        string keySetPic = "/" + CommonConstants.Store + "/" + CommonConstants.Karte + "/" + CommonConstants.SetPic + "/";

        foreach (var oldFileLink in listFileDo)
        {
            if (!oldFileLink.Contains(baseAccessUrl))
            {
                continue;
            }
            string oldFileName = Path.GetFileName(oldFileLink);
            if (oldFileLink.Contains(keyNextPic) || oldFileLink.Contains(keySetPic))
            {
                string newFile = host + _amazonS3Service.GetUniqueFileNameKey(oldFileName.Trim());
                var copySuccess = _amazonS3Service.CopyObjectAsync(oldFileLink.Replace(baseAccessUrl, string.Empty), newFile.Replace(baseAccessUrl, string.Empty)).Result;
                if (copySuccess)
                {
                    fileInfUpdateTemp.Add(oldFileName, newFile);
                }
            }
            else
            {
                fileInfUpdateTemp.Add(oldFileName, oldFileName);
            }
        }
        return fileInfUpdateTemp;
    }

    private List<OrdInfModel> ConvertInputDataToOrderInfs(int hpId, int sinDate, List<OdrInfItemInputData> inputDataList)
    {
        var allOdrInfs = new List<OrdInfModel>();

        var itemCds = new List<string>();
        var ipnNameCds = new List<string>();
        foreach (var item in inputDataList.Select(o => o.OdrDetails))
        {
            itemCds.AddRange(item?.Select(od => od.ItemCd).Distinct() ?? new List<string>());
            ipnNameCds.AddRange(item?.Select(od => od.IpnCd).Distinct() ?? new List<string>());
        }
        itemCds = itemCds?.Distinct().ToList() ?? new List<string>();
        ipnNameCds = ipnNameCds?.Distinct().ToList() ?? new List<string>();

        var tenMsts = _mstItemRepository.GetCheckTenItemModels(hpId, sinDate, itemCds);
        var ipnMinYakaMsts = _ordInfRepository.GetCheckIpnMinYakkaMsts(hpId, sinDate, ipnNameCds);
        var refillSetting = _systemGenerationConfRepository.GetSettingValue(hpId, 2002, 0, sinDate, 999).Item1;
        var checkIsGetYakkaPrices = _ordInfRepository.CheckIsGetYakkaPrices(hpId, tenMsts ?? new List<TenItemModel>(), sinDate);
        var sinDateMax = inputDataList.Count > 0 ? inputDataList.Max(i => i.SinDate) : 0;
        var sinDateMin = inputDataList.Count > 0 ? inputDataList.Min(i => i.SinDate) : 0;
        var ipnNameMsts = _ordInfRepository.GetIpnMst(hpId, sinDateMin, sinDateMax, ipnNameCds);

        foreach (var item in inputDataList)
        {
            var ordInf = new OrdInfModel(
                    item.HpId,
                    item.RaiinNo,
                    item.RpNo,
                    item.RpEdaNo,
                    item.PtId,
                    item.SinDate,
                    item.HokenPid,
                    item.OdrKouiKbn,
                    item.RpName,
                    item.InoutKbn,
                    item.SikyuKbn,
                    item.SyohoSbt,
                    item.SanteiKbn,
                    item.TosekiKbn,
                    item.DaysCnt,
                    item.SortNo,
                    item.IsDeleted,
                    item.Id,
                    new List<OrdInfDetailModel>(),
                    DateTime.MinValue,
                    0,
                    "",
                    DateTime.MinValue,
                    0,
                    "",
                    string.Empty,
                    string.Empty
                );

            foreach (var itemDetail in item.OdrDetails)
            {
                var inputItem = itemDetail == null ? null : tenMsts?.FirstOrDefault(t => t.ItemCd == itemDetail.ItemCd);
                refillSetting = itemDetail == null ? 999 : refillSetting;
                var ipnMinYakaMst = (inputItem == null || (inputItem.HpId == 0 && string.IsNullOrEmpty(inputItem.ItemCd))) ? null : ipnMinYakaMsts.FirstOrDefault(i => i.IpnNameCd == itemDetail?.IpnCd);
                var isCheckIpnKasanExclude = checkIsGetYakkaPrices.FirstOrDefault(y => y.Item1 == inputItem?.IpnNameCd && y.Item2 == inputItem?.ItemCd)?.Item3 == true;

                if (itemDetail == null)
                {
                    break;
                }

                var ordInfDetail = new OrdInfDetailModel(
                            itemDetail.HpId,
                            itemDetail.RaiinNo,
                            itemDetail.RpNo,
                            itemDetail.RpEdaNo,
                            itemDetail.RowNo,
                            itemDetail.PtId,
                            itemDetail.SinDate,
                            itemDetail.SinKouiKbn,
                            itemDetail.ItemCd,
                            itemDetail.ItemName,
                            itemDetail.Suryo,
                            itemDetail.UnitName,
                            itemDetail.UnitSbt,
                            itemDetail.TermVal,
                            itemDetail.KohatuKbn,
                            itemDetail.SyohoKbn,
                            itemDetail.SyohoLimitKbn,
                            itemDetail.DrugKbn,
                            itemDetail.YohoKbn,
                            itemDetail.Kokuji1,
                            itemDetail.Kokuji2,
                            itemDetail.IsNodspRece,
                            itemDetail.IpnCd,
                            ipnNameMsts.FirstOrDefault(i => i.Item1 == itemDetail.IpnCd)?.Item2 ?? string.Empty,
                            itemDetail.JissiKbn,
                            itemDetail.JissiDate,
                            itemDetail.JissiId,
                            itemDetail.JissiMachine,
                            itemDetail.ReqCd,
                            itemDetail.Bunkatu,
                            itemDetail.CmtName,
                            itemDetail.CmtOpt,
                            itemDetail.FontColor,
                            itemDetail.CommentNewline,
                            inputItem?.MasterSbt ?? string.Empty,
                            item?.InoutKbn ?? 0,
                            ipnMinYakaMst?.Yakka ?? 0,
                            isCheckIpnKasanExclude,
                            refillSetting,
                            inputItem?.CmtCol1 ?? 0,
                            inputItem?.Ten ?? 0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            "",
                            new List<YohoSetMstModel>(),
                            0,
                            0,
                            "",
                            "",
                            "",
                            ""
                        );
                ordInf.OrdInfDetails.Add(ordInfDetail);
            }
            allOdrInfs.Add(ordInf);
        }

        return allOdrInfs;
    }

    private RaiinInfConst.RaiinInfTodayOdrValidationStatus CheckCommon(bool isCheckOrder, List<int> hpIds, List<long> ptIds, List<long> raiinNos, List<int> sinDates, int hpId, long ptId, long raiinNo)
    {
        RaiinInfConst.RaiinInfTodayOdrValidationStatus raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid;

        if (hpIds.Count > 1 || hpIds.FirstOrDefault() <= 0)
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidHpId;
        }
        else if (ptIds.Count > 1 || ptIds.FirstOrDefault() <= 0)
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidPtId;
        }
        else if (isCheckOrder && (raiinNos.Count > 1 || raiinNos.FirstOrDefault() <= 0))
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidRaiinNo;
        }
        else if (isCheckOrder && (sinDates.Count > 1 || sinDates.FirstOrDefault() <= 0))
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSinDate;
        }
        else
        {
            var checkHpId = _hpInfRepository.CheckHpId(hpId);
            var checkPtId = _patientInforRepository.CheckExistIdList(new List<long> { ptId });
            var checkRaiinNo = _receptionRepository.CheckListNo(new List<long> { raiinNo });

            if (!checkHpId)
            {
                raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.HpIdNoExist;
            }
            else if (!checkPtId)
            {
                raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.PtIdNoExist;
            }
            else if (!checkRaiinNo && isCheckOrder)
            {
                raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.RaiinIdNoExist;
            }
        }

        return raiinInfStatus;
    }

    private RaiinInfConst.RaiinInfTodayOdrValidationStatus CheckRaiinInf(SaveMedicalInputData inputDatas)
    {
        RaiinInfConst.RaiinInfTodayOdrValidationStatus raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid;

        if (!(inputDatas.SyosaiKbn >= 0 && inputDatas.SyosaiKbn <= 8))
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSyosaiKbn;
        }
        else if (!(inputDatas.JikanKbn >= 0 && inputDatas.JikanKbn <= 7))
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidJikanKbn;
        }
        else if (inputDatas.HokenPid <= 0)
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidHokenPid;
        }
        else if (!(inputDatas.SanteiKbn >= 0 && inputDatas.SanteiKbn <= 2))
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSanteiKbn;
        }
        if (inputDatas.TantoId < 0)
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidTantoId;
        }
        else if (inputDatas.KaId < 0)
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidKaId;
        }

        else if (inputDatas.UketukeTime.Length > 6)
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidUKetukeTime;
        }
        else if (inputDatas.SinStartTime.Length > 6)
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSinStartTime;

        }
        else if (inputDatas.SinEndTime.Length > 6)
        {
            raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSinEndTime;
        }

        if (inputDatas.HokenPid > 0)
        {
            var checkHokenId = _insuranceInforRepository.CheckExistHokenPid(inputDatas.HokenPid);
            if (!checkHokenId)
            {
                raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.HokenPidNoExist;
            }
        }

        if (inputDatas.TantoId > 0)
        {
            var checkHokenId = _userRepository.CheckExistedUserId(inputDatas.TantoId);
            if (!checkHokenId)
            {
                raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.TatoIdNoExist;
            }
        }

        if (inputDatas.KaId > 0)
        {
            var checkHokenId = _kaRepository.CheckKaId(inputDatas.KaId);
            if (!checkHokenId)
            {
                raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.KaIdNoExist;
            }
        }

        return raiinInfStatus;
    }

    private (Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>, List<OrdInfModel>) CheckOrder(int hpId, long ptId, int sinDate, SaveMedicalInputData inputDatas, List<OdrInfItemInputData> inputDataList, byte status)
    {
        var dicValidation = new Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>();
        object obj = new();
        var allOdrInfs = new List<OrdInfModel>();

        if (inputDatas.OdrItems.Count > 0)
        {
            var raiinNoOdrs = status != (byte)ModeSaveData.TempSave ? inputDataList.Select(i => i.RaiinNo).Distinct().ToList() : new();
            var checkOderInfs = status != (byte)ModeSaveData.TempSave ? _ordInfRepository.GetListToCheckValidate(ptId, hpId, raiinNoOdrs ?? new List<long>()) : Enumerable.Empty<OrdInfModel>();

            var hokenPids = status != (byte)ModeSaveData.TempSave ? inputDataList.Select(i => i.HokenPid).Distinct().ToList() : new();
            var checkHokens = status != (byte)ModeSaveData.TempSave ? _insuranceInforRepository.GetCheckListHokenInf(hpId, ptId, hokenPids ?? new List<int>()) : new();

            if (status != (byte)ModeSaveData.TempSave)
            {
                for (int index = 0; index < inputDataList.Count; index++)
                {
                    var item = inputDataList[index];
                    if (item.IsDeleted != 0)
                    {
                        continue;
                    }

                    if (item.Id > 0)
                    {
                        var check = checkOderInfs.Any(c => c.HpId == item.HpId && c.PtId == item.PtId && c.RaiinNo == item.RaiinNo && c.SinDate == item.SinDate && c.RpNo == item.RpNo && c.RpEdaNo == item.RpEdaNo);
                        if (!check)
                        {
                            dicValidation.Add(index.ToString(), new("-1", OrdInfValidationStatus.InvalidTodayOrdUpdatedNoExist));
                            break;
                        }
                    }

                    var checkObjs = inputDataList.Where(o => item.Id > 0 && o.RpNo == item.RpNo).ToList();
                    var positionOrd = inputDataList.FindIndex(o => o == checkObjs.LastOrDefault());
                    if (checkObjs.Count >= 2 && positionOrd == index)
                    {
                        dicValidation.Add(positionOrd.ToString(), new("-1", OrdInfValidationStatus.DuplicateTodayOrd));
                        break;
                    }

                    var checkHokenPid = checkHokens.Any(h => h.HokenId == item.HokenPid);
                    if (!checkHokenPid)
                    {
                        dicValidation.Add(index.ToString(), new("-1", OrdInfValidationStatus.HokenPidNoExist));
                        break;
                    }

                    var odrDetail = item.OdrDetails.FirstOrDefault(itemOd => item.RpNo != itemOd.RpNo || item.RpEdaNo != itemOd.RpEdaNo || item.HpId != itemOd.HpId || item.PtId != itemOd.PtId || item.SinDate != itemOd.SinDate || item.RaiinNo != itemOd.RaiinNo);
                    if (odrDetail != null)
                    {
                        var indexOdrDetail = item.OdrDetails.IndexOf(odrDetail);
                        dicValidation.Add(index.ToString(), new(indexOdrDetail.ToString(), OrdInfValidationStatus.OdrNoMapOdrDetail));
                    }
                }
            }

            allOdrInfs = ConvertInputDataToOrderInfs(hpId, sinDate, inputDataList);

            if (status != (byte)ModeSaveData.TempSave)
            {
                for (int index = 0; index < allOdrInfs.Count; index++)
                {

                    var item = allOdrInfs[index];

                    var modelValidation = item.Validation(0);
                    if (modelValidation.Value != OrdInfValidationStatus.Valid && !dicValidation.ContainsKey(index.ToString()))
                    {
                        lock (obj)
                        {
                            dicValidation.Add(index.ToString(), modelValidation);
                        }
                    }
                }
            }
        }

        return (dicValidation, allOdrInfs);
    }

    private void AddErrorStatus(object obj, Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> dicValidation, string key, KeyValuePair<string, OrdInfValidationStatus> status)
    {
        lock (obj)
        {
            dicValidation.Add(key, status);
        }
    }

    private List<FamilyModel> ConvertToFamilyList(List<FamilyItem> familyInputList)
    {
        List<FamilyModel> result = new();
        foreach (var family in familyInputList)
        {
            result.Add(new FamilyModel(
                           family.FamilyId,
                           family.PtId,
                           family.ZokugaraCd,
                           family.FamilyPtId,
                           family.Name,
                           family.KanaName,
                           family.Sex,
                           family.Birthday,
                           family.IsDead,
                           family.IsSeparated,
                           family.Biko,
                           family.SortNo,
                           family.IsDeleted,
                           family.PtFamilyRekiList.Select(reki => new PtFamilyRekiModel(
                                                                      reki.Id,
                                                                      reki.ByomeiCd,
                                                                      reki.Byomei,
                                                                      reki.Cmt,
                                                                      reki.SortNo,
                                                                      reki.IsDeleted
                                                          )).ToList()));
        }
        return result;
    }

    private UpsertFlowSheetStatus ValidateFlowSheet(List<UpsertFlowSheetItemInputData> flowSheets)
    {
        foreach (var flowSheet in flowSheets)
        {
            if (flowSheet.TagNo < -1 || flowSheet.TagNo > 7)
            {
                return UpsertFlowSheetStatus.TagNoNoValid;
            }
        }
        return UpsertFlowSheetStatus.Valid;
    }

    private UpsertPtDiseaseListStatus ValidateDiseaseList(List<PtDiseaseModel> ptDiseases)
    {
        foreach (var data in ptDiseases)
        {
            var status = data.Validation();
            if (status != DiseaseValidationStatus.Valid)
            {
                return ConvertStatus(status);
            }
        }

        if (!_patientInforRepository.CheckExistIdList(ptDiseases.Select(i => i.PtId).Distinct().ToList()))
        {
            return UpsertPtDiseaseListStatus.PtDiseaseListPtIdNoExist;
        }

        return UpsertPtDiseaseListStatus.Valid;
    }

    private static UpsertPtDiseaseListStatus ConvertStatus(DiseaseValidationStatus status)
    {
        if (status == DiseaseValidationStatus.InvalidTenkiKbn)
            return UpsertPtDiseaseListStatus.PtDiseaseListInvalidTenkiKbn;
        if (status == DiseaseValidationStatus.InvalidSikkanKbn)
            return UpsertPtDiseaseListStatus.PtDiseaseListInvalidSikkanKbn;
        if (status == DiseaseValidationStatus.InvalidNanByoCd)
            return UpsertPtDiseaseListStatus.PtDiseaseListInvalidNanByoCd;
        if (status == DiseaseValidationStatus.InvalidFreeWord)
            return UpsertPtDiseaseListStatus.PtDiseaseListInvalidFreeWord;
        if (status == DiseaseValidationStatus.InvalidTenkiDateContinue)
            return UpsertPtDiseaseListStatus.PtDiseaseListInvalidTenkiDateContinue;
        if (status == DiseaseValidationStatus.InvalidTenkiDateCommon)
            return UpsertPtDiseaseListStatus.PtDiseaseListInvalidTenkiDateCommon;
        if (status == DiseaseValidationStatus.InvalidTekiDateAndStartDate)
            return UpsertPtDiseaseListStatus.PtDiseaseListInvalidTekiDateAndStartDate;
        if (status == DiseaseValidationStatus.InvalidByomei)
            return UpsertPtDiseaseListStatus.PtDiseaseListInvalidByomei;
        if (status == DiseaseValidationStatus.InvalidId)
            return UpsertPtDiseaseListStatus.PtInvalidId;
        if (status == DiseaseValidationStatus.InvalidHpId)
            return UpsertPtDiseaseListStatus.PtInvalidHpId;
        if (status == DiseaseValidationStatus.InvalidPtId)
            return UpsertPtDiseaseListStatus.PtInvalidPtId;
        if (status == DiseaseValidationStatus.InvalidSortNo)
            return UpsertPtDiseaseListStatus.PtInvalidSortNo;
        if (status == DiseaseValidationStatus.InvalidByomeiCd)
            return UpsertPtDiseaseListStatus.PtInvalidByomeiCd;
        if (status == DiseaseValidationStatus.InvalidStartDate)
            return UpsertPtDiseaseListStatus.PtInvalidStartDate;
        if (status == DiseaseValidationStatus.InvalidTenkiDate)
            return UpsertPtDiseaseListStatus.PtInvalidTenkiDate;
        if (status == DiseaseValidationStatus.InvalidSyubyoKbn)
            return UpsertPtDiseaseListStatus.PtInvalidSyubyoKbn;
        if (status == DiseaseValidationStatus.InvalidHosokuCmt)
            return UpsertPtDiseaseListStatus.PtInvalidHosokuCmt;
        if (status == DiseaseValidationStatus.InvalidHokenPid)
            return UpsertPtDiseaseListStatus.PtInvalidHokenPid;
        if (status == DiseaseValidationStatus.InvalidIsNodspRece)
            return UpsertPtDiseaseListStatus.PtInvalidIsNodspRece;
        if (status == DiseaseValidationStatus.InvalidIsNodspKarte)
            return UpsertPtDiseaseListStatus.PtInvalidIsNodspKarte;
        if (status == DiseaseValidationStatus.InvalidSeqNo)
            return UpsertPtDiseaseListStatus.PtInvalidSeqNo;
        if (status == DiseaseValidationStatus.InvalidIsImportant)
            return UpsertPtDiseaseListStatus.PtInvalidIsImportant;
        if (status == DiseaseValidationStatus.InvalidIsDeleted)
            return UpsertPtDiseaseListStatus.PtInvalidIsDeleted;

        return UpsertPtDiseaseListStatus.Valid;
    }

}
