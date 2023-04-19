﻿using Domain.Models.Family;
using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.Ka;
using Domain.Models.KarteInf;
using Domain.Models.KarteInfs;
using Domain.Models.Medical;
using Domain.Models.MonshinInf;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SystemGenerationConf;
using Domain.Models.TodayOdr;
using Domain.Models.User;
using Helper.Constants;
using Helper.Enum;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Interactor.CalculateService;
using Interactor.Family.ValidateFamilyList;
using Microsoft.Extensions.Options;
using UseCase.Accounting.Recaculate;
using UseCase.Family;
using UseCase.MedicalExamination.SaveMedical;
using UseCase.MedicalExamination.UpsertTodayOrd;
using static Helper.Constants.KarteConst;
using static Helper.Constants.OrderInfConst;

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
    private readonly IMonshinInforRepository _monshinInforRepository;
    private readonly AmazonS3Options _options;

    public SaveMedicalInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service, IOrdInfRepository ordInfRepository, IReceptionRepository receptionRepository, IKaRepository kaRepository, IMstItemRepository mstItemRepository, ISystemGenerationConfRepository systemGenerationConfRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceInforRepository, IUserRepository userRepository, IHpInfRepository hpInfRepository, ISaveMedicalRepository saveMedicalRepository, ITodayOdrRepository todayOdrRepository, IKarteInfRepository karteInfRepository, ICalculateService calculateService, IValidateFamilyList validateFamilyList, IMonshinInforRepository monshinInforRepository)
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
        _monshinInforRepository = monshinInforRepository;
    }

    public SaveMedicalOutputData Handle(SaveMedicalInputData inputDatas)
    {
        try
        {
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
                        0,
                        0,
                        0);
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

            // validate family
            var validateFamilyList = _validateFamilyList.ValidateData(hpId, ptId, inputDatas.FamilyList);

            if (raiinInfStatus != RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid || validateKarte != KarteValidationStatus.Valid || resultOrder.Item1.Any() || validateFamilyList != ValidateFamilyListStatus.ValidateSuccess)
            {
                return new SaveMedicalOutputData(
                    SaveMedicalStatus.Failed,
                    raiinInfStatus,
                    resultOrder.Item1,
                    validateKarte,
                    validateFamilyList,
                    0,
                    0,
                    0);
            }

            // Family list
            var familyList = ConvertToFamilyList(inputDatas.FamilyList);

            var check = _saveMedicalRepository.Upsert(hpId, ptId, raiinNo, sinDate, inputDatas.SyosaiKbn, inputDatas.JikanKbn, inputDatas.HokenPid, inputDatas.SanteiKbn, inputDatas.TantoId, inputDatas.KaId, inputDatas.UketukeTime, inputDatas.SinStartTime, inputDatas.SinEndTime, inputDatas.Status, allOdrInfs, karteModel, inputDatas.UserId, familyList);
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

            // Save Monshin
            if (inputDatas.Monshins != null)
            {
                _monshinInforRepository.SaveMonshinSheet(inputDatas.Monshins);
            }

            if (check)
            {
                Task.Run(() =>
               _calculateService.RunCalculate(new RecaculationInputDto(
                        hpId,
                        ptId,
                        sinDate,
                        0,
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
                    sinDate,
                    raiinNo,
                    ptId)
                :
                new SaveMedicalOutputData(
                    SaveMedicalStatus.Failed,
                    RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid,
                    new Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>(),
                    KarteValidationStatus.Valid,
                    ValidateFamilyListStatus.ValidateSuccess,
                    sinDate,
                    raiinNo,
                    ptId);
        }
        catch
        {
            return new SaveMedicalOutputData(
                SaveMedicalStatus.Failed,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid,
                new Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>(),
                KarteValidationStatus.Valid,
                ValidateFamilyListStatus.ValidateSuccess,
                0,
                0,
                0);
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
            _monshinInforRepository.ReleaseResource();
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
            _karteInfRepository.SaveListFileKarte(hpId, ptId, raiinNo, host, listUpdates.Select(item => new FileInfModel(false, item)).ToList(), false);
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

        var obj = new object();
        Parallel.ForEach(inputDataList, item =>
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

            var objDetail = new object();
            Parallel.ForEach(item.OdrDetails, itemDetail =>
            {
                var inputItem = itemDetail == null ? null : tenMsts?.FirstOrDefault(t => t.ItemCd == itemDetail.ItemCd);
                refillSetting = itemDetail == null ? 999 : refillSetting;
                var ipnMinYakaMst = (inputItem == null || (inputItem.HpId == 0 && string.IsNullOrEmpty(inputItem.ItemCd))) ? null : ipnMinYakaMsts.FirstOrDefault(i => i.IpnNameCd == itemDetail?.IpnCd);
                var isCheckIpnKasanExclude = checkIsGetYakkaPrices.FirstOrDefault(y => y.Item1 == inputItem?.IpnNameCd && y.Item2 == inputItem?.ItemCd)?.Item3 == true;

                if (itemDetail == null)
                {
                    return;
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
                lock (objDetail)
                {
                    ordInf.OrdInfDetails.Add(ordInfDetail);
                }
            });
            lock (obj)
            {
                allOdrInfs.Add(ordInf);
            }
        });

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
                Parallel.For(0, inputDataList.Count, index =>
                {
                    var item = inputDataList[index];

                    if (item.Id > 0)
                    {
                        var check = checkOderInfs.Any(c => c.HpId == item.HpId && c.PtId == item.PtId && c.RaiinNo == item.RaiinNo && c.SinDate == item.SinDate && c.RpNo == item.RpNo && c.RpEdaNo == item.RpEdaNo);
                        if (!check)
                        {
                            AddErrorStatus(obj, dicValidation, index.ToString(), new("-1", OrdInfValidationStatus.InvalidTodayOrdUpdatedNoExist));
                            return;
                        }
                    }

                    var checkObjs = inputDataList.Where(o => item.Id > 0 && o.RpNo == item.RpNo).ToList();
                    var positionOrd = inputDataList.FindIndex(o => o == checkObjs.LastOrDefault());
                    if (checkObjs.Count >= 2 && positionOrd == index)
                    {

                        AddErrorStatus(obj, dicValidation, positionOrd.ToString(), new("-1", OrdInfValidationStatus.DuplicateTodayOrd));
                        return;
                    }

                    var checkHokenPid = checkHokens.Any(h => h.HokenId == item.HokenPid);
                    if (!checkHokenPid)
                    {
                        AddErrorStatus(obj, dicValidation, index.ToString(), new("-1", OrdInfValidationStatus.HokenPidNoExist));
                        return;
                    }

                    var odrDetail = item.OdrDetails.FirstOrDefault(itemOd => item.RpNo != itemOd.RpNo || item.RpEdaNo != itemOd.RpEdaNo || item.HpId != itemOd.HpId || item.PtId != itemOd.PtId || item.SinDate != itemOd.SinDate || item.RaiinNo != itemOd.RaiinNo);
                    if (odrDetail != null)
                    {
                        var indexOdrDetail = item.OdrDetails.IndexOf(odrDetail);
                        AddErrorStatus(obj, dicValidation, index.ToString(), new(indexOdrDetail.ToString(), OrdInfValidationStatus.OdrNoMapOdrDetail));
                    }
                });
            }

            allOdrInfs = ConvertInputDataToOrderInfs(hpId, sinDate, inputDataList);

            if (status != (byte)ModeSaveData.TempSave)
            {
                Parallel.For(0, allOdrInfs.Count, index =>
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
                });
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
}
