using Domain.Models.HpMst;
using Domain.Models.Insurance;
using Domain.Models.KaMst;
using Domain.Models.KarteInfs;
using Domain.Models.KarteKbnMst;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SystemGenerationConf;
using Domain.Models.TodayOdr;
using Domain.Models.User;
using Helper.Constants;
using Microsoft.EntityFrameworkCore;
using UseCase.MedicalExamination.UpsertTodayOrd;
using static Helper.Constants.TodayKarteConst;
using static Helper.Constants.TodayOrderConst;

namespace Interactor.MedicalExamination
{
    public class UpsertTodayOrdInteractor : IUpsertTodayOrdInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IReceptionRepository _receptionRepository;
        private readonly IKaMstRepository _kaRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ISystemGenerationConfRepository _systemGenerationConfRepository;
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IKarteKbnMstRepository _karteKbnInforRepository;
        private readonly IInsuranceRepository _insuranceInforRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHpInfRepository _hpInfRepository;
        private readonly ITodayOdrRepository _todayOdrRepository;

        public UpsertTodayOrdInteractor(IOrdInfRepository ordInfRepository, IReceptionRepository receptionRepository, IKaMstRepository kaRepository, IMstItemRepository mstItemRepository, ISystemGenerationConfRepository systemGenerationConfRepository, IPatientInforRepository patientInforRepository, IKarteKbnMstRepository karteKbnInforRepository, IInsuranceRepository insuranceInforRepository, IUserRepository userRepository, IHpInfRepository hpInfRepository, ITodayOdrRepository todayOdrRepository)
        {
            _ordInfRepository = ordInfRepository;
            _kaRepository = kaRepository;
            _receptionRepository = receptionRepository;
            _mstItemRepository = mstItemRepository;
            _systemGenerationConfRepository = systemGenerationConfRepository;
            _patientInforRepository = patientInforRepository;
            _karteKbnInforRepository = karteKbnInforRepository;
            _insuranceInforRepository = insuranceInforRepository;
            _userRepository = userRepository;
            _hpInfRepository = hpInfRepository;
            _todayOdrRepository = todayOdrRepository;
        }

        public UpsertTodayOrdOutputData Handle(UpsertTodayOrdInputData inputDatas)
        {
            try
            {
                if (inputDatas.OdrItems.Count == 0 && inputDatas.KarteInfs.Count == 0)
                {
                    return new UpsertTodayOrdOutputData(UpsertTodayOrdStatus.Failed, RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid, new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>(), new Dictionary<int, TodayKarteValidationStatus>());
                }

                //Raiin Info
                var raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid;

                var inputDataList = inputDatas.OdrItems.ToList();
                var inputKarteDataList = inputDatas.KarteInfs.ToList();
                var hpIds = inputDataList.Select(x => x.HpId).Union(inputKarteDataList.Select(x => x.HpId)).Distinct().ToList();
                var ptIds = inputDataList.Select(x => x.PtId).Union(inputKarteDataList.Select(x => x.PtId)).Distinct().ToList();
                var raiinNos = inputDataList.Select(x => x.RaiinNo).Union(inputKarteDataList.Select(x => x.RaiinNo)).Distinct().ToList();
                var sinDates = inputDataList.Select(x => x.SinDate).Union(inputKarteDataList.Select(x => x.SinDate)).Distinct().ToList();

                var hpId = hpIds[0];
                var ptId = ptIds[0];
                var raiinNo = raiinNos[0];
                var sinDate = sinDates[0];

                if (hpIds.Count > 1 || hpIds.FirstOrDefault() <= 0)
                {
                    raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidHpId;
                }
                else if (ptIds.Count > 1 || ptIds.FirstOrDefault() <= 0)
                {
                    raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidPtId;
                }
                else if (raiinNos.Count > 1 || raiinNos.FirstOrDefault() <= 0)
                {
                    raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidRaiinNo;
                }
                else if (sinDates.Count > 1 || sinDates.FirstOrDefault() <= 0)
                {
                    raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSinDate;
                }
                else
                {
                    var checkHpId = _hpInfRepository.CheckHpId(hpId);
                    var checkPtId = _patientInforRepository.CheckListId(new List<long> { ptId });
                    var checkRaiinNo = _receptionRepository.CheckListNo(new List<long> { raiinNo });

                    if (!checkHpId)
                    {
                        raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.HpIdNoExist;
                    }
                    else if (!checkPtId)
                    {
                        raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.PtIdNoExist;
                    }
                    else if (!checkRaiinNo)
                    {
                        raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.RaiinIdNoExist;
                    }
                }

                if (raiinInfStatus != RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid)
                {
                    return new UpsertTodayOrdOutputData(UpsertTodayOrdStatus.Failed, raiinInfStatus, new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>(), new Dictionary<int, TodayKarteValidationStatus>());
                }

                if (!(inputDatas.SyosaiKbn >= 0 && inputDatas.SyosaiKbn <= 8))
                {
                    raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSyosaiKbn;
                }
                else if (!(inputDatas.JikanKbn >= 0 && inputDatas.JikanKbn <= 7))
                {
                    raiinInfStatus = RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidJikanKbn;
                }
                else if (inputDatas.HokenPid < 0)
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
                    var checkHokenId = _insuranceInforRepository.CheckHokenPid(inputDatas.HokenPid);
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



                var dicKarteValidation = new Dictionary<int, TodayKarteValidationStatus>();
                var dicValidation = new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>();
                var allOdrInfs = new List<OrdInfModel>();
                var karteModels = new List<KarteInfModel>();
                object obj = new();


                // Odr Info
                if (inputDatas.OdrItems.Count > 0)
                {
                    var raiinNoOdrs = inputDataList.Select(i => i.RaiinNo).Distinct().ToList();
                    var checkOderInfs = _ordInfRepository.GetListToCheckValidate(ptId, hpId, raiinNoOdrs ?? new List<long>());

                    var hokenPids = inputDataList.Select(i => i.HokenPid).Distinct().ToList();
                    var checkHokens = _insuranceInforRepository.GetCheckListHokenInf(hpId, ptId, hokenPids ?? new List<int>());
                    Parallel.For(0, inputDataList.Count, index =>
                    {
                        lock (obj)
                        {
                            var item = inputDataList[index];

                            if (item.Id > 0)
                            {
                                var check = checkOderInfs.Any(c => c.HpId == item.HpId && c.PtId == item.PtId && c.RaiinNo == item.RaiinNo && c.SinDate == item.SinDate && c.RpNo == item.RpNo && c.RpEdaNo == item.RpEdaNo);
                                if (!check)
                                {
                                    dicValidation.Add(index, new(-1, TodayOrdValidationStatus.InvalidTodayOrdUpdatedNoExist));
                                    return;
                                }
                            }
                            else
                            {
                                var check = checkOderInfs.Any(c => c.HpId == item.HpId && c.PtId == item.PtId && c.RaiinNo == item.RaiinNo && c.SinDate == item.SinDate && c.RpNo == item.RpNo && c.RpEdaNo == item.RpEdaNo);

                                if (check)
                                {
                                    dicValidation.Add(index, new(-1, TodayOrdValidationStatus.InvalidTodayOrdInsertedExist));
                                    return;
                                }
                            }

                            var checkObjs = inputDataList.Where(o => o.RpNo == item.RpNo && o.RpEdaNo == item.RpEdaNo).ToList();
                            var positionOrd = inputDataList.FindIndex(o => o == checkObjs.LastOrDefault());
                            if (checkObjs.Count >= 2 && positionOrd == index)
                            {
                                dicValidation.Add(positionOrd, new(-1, TodayOrdValidationStatus.DuplicateTodayOrd));
                                return;
                            }

                            var checkHokenPid = checkHokens.Any(h => h.HokenId == item.HokenPid);
                            if (!checkHokenPid)
                            {
                                dicValidation.Add(index, new(-1, TodayOrdValidationStatus.HokenPidNoExist));
                                return;
                            }

                            Parallel.ForEach(item.OdrDetails, itemOd =>
                            {
                                var indexOd = item.OdrDetails.IndexOf(itemOd);

                                if (item.RpNo != itemOd.RpNo || item.RpEdaNo != itemOd.RpEdaNo || item.HpId != itemOd.HpId || item.PtId != itemOd.PtId || item.SinDate != itemOd.SinDate || item.RaiinNo != itemOd.RaiinNo)
                                {
                                    dicValidation.Add(index, new(indexOd, TodayOrdValidationStatus.OdrNoMapOdrDetail));
                                }
                            });
                        }
                    });

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
                    var refillSetting = _systemGenerationConfRepository.GetSettingValue(hpId, 2002, 0, sinDate, 999);
                    var checkIsGetYakkaPrices = _ordInfRepository.CheckIsGetYakkaPrices(hpId, tenMsts ?? new List<TenItemModel>(), sinDate);

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
                                DateTime.MinValue
                            );

                        Parallel.ForEach(item.OdrDetails, itemDetail =>
                        {
                            var inputItem = itemDetail == null ? null : tenMsts?.FirstOrDefault(t => t.ItemCd == itemDetail.ItemCd);
                            refillSetting = itemDetail == null ? 999 : refillSetting;
                            var ipnMinYakaMst = (inputItem == null || (inputItem.HpId == 0 && string.IsNullOrEmpty(inputItem.ItemCd))) ? null : ipnMinYakaMsts.FirstOrDefault(i => i.IpnNameCd == itemDetail?.IpnCd);
                            var isCheckIpnKasanExclude = checkIsGetYakkaPrices.FirstOrDefault(y => y.Item1 == inputItem?.IpnNameCd && y.Item2 == inputItem?.ItemCd)?.Item3 == true;

                            var ordInfDetail = new OrdInfDetailModel(
                                        itemDetail?.HpId ?? 0,
                                        itemDetail?.RaiinNo ?? 0,
                                        itemDetail?.RpNo ?? 0,
                                        itemDetail?.RpEdaNo ?? 0,
                                        itemDetail?.RowNo ?? 0,
                                        itemDetail?.PtId ?? 0,
                                        itemDetail?.SinDate ?? 0,
                                        itemDetail?.SinKouiKbn ?? 0,
                                        itemDetail?.ItemCd ?? string.Empty,
                                        itemDetail?.ItemName ?? string.Empty,
                                        itemDetail?.Suryo ?? 0,
                                        itemDetail?.UnitName ?? string.Empty,
                                        itemDetail?.UnitSbt ?? 0,
                                        itemDetail?.TermVal ?? 0,
                                        itemDetail?.KohatuKbn ?? 0,
                                        itemDetail?.SyohoKbn ?? 0,
                                        itemDetail?.SyohoLimitKbn ?? 0,
                                        itemDetail?.DrugKbn ?? 0,
                                        itemDetail?.YohoKbn ?? 0,
                                        itemDetail?.Kokuji1 ?? string.Empty,
                                        itemDetail?.Kokuji2 ?? string.Empty,
                                        itemDetail?.IsNodspRece ?? 0,
                                        itemDetail?.IpnCd ?? string.Empty,
                                        itemDetail?.IpnName ?? string.Empty,
                                        itemDetail?.JissiKbn ?? 0,
                                        itemDetail?.JissiDate ?? DateTime.MinValue,
                                        itemDetail?.JissiId ?? 0,
                                        itemDetail?.JissiMachine ?? string.Empty,
                                        itemDetail?.ReqCd ?? string.Empty,
                                        itemDetail?.Bunkatu ?? string.Empty,
                                        itemDetail?.CmtName ?? string.Empty,
                                        itemDetail?.CmtOpt ?? string.Empty,
                                        itemDetail?.FontColor ?? string.Empty,
                                        itemDetail?.CommentNewline ?? 0,
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
                                        0
                                    );
                            ordInf.OrdInfDetails.Add(ordInfDetail);
                        });

                        allOdrInfs.Add(ordInf);
                    });

                    Parallel.ForEach(allOdrInfs, item =>
                    {
                        var index = allOdrInfs.IndexOf(item);

                        var modelValidation = item.Validation();
                        if (modelValidation.Value != TodayOrdValidationStatus.Valid && !dicValidation.ContainsKey(index))
                        {
                            dicValidation.Add(index, modelValidation);
                        }
                    });
                }

                // Karte
                if (inputDatas.KarteInfs.Count > 0)
                {
                    karteModels = inputKarteDataList.Select(k => new KarteInfModel(
                            k.HpId,
                            k.RaiinNo,
                            1,
                            0,
                            k.PtId,
                            k.SinDate,
                            k.Text,
                            k.IsDeleted,
                            k.RichText,
                            DateTime.MinValue,
                            DateTime.MinValue
                        )).ToList();

                    Parallel.For(0, karteModels.Count, index =>
                    {
                        lock (obj)
                        {
                            var karte = karteModels[index];
                            var modelValidation = karte.Validation();
                            if (modelValidation != TodayKarteValidationStatus.Valid)
                            {
                                dicKarteValidation.Add(index, modelValidation);
                            }
                        }
                    });
                }

                if (raiinInfStatus != RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid || dicKarteValidation.Any() || dicValidation.Any())
                {
                    return new UpsertTodayOrdOutputData(UpsertTodayOrdStatus.Failed, raiinInfStatus, dicValidation, dicKarteValidation);
                }

                var check = _todayOdrRepository.Upsert(hpId, ptId, raiinNo, sinDate, inputDatas.SyosaiKbn, inputDatas.JikanKbn, inputDatas.HokenPid, inputDatas.SanteiKbn, inputDatas.TantoId, inputDatas.KaId, inputDatas.UketukeTime, inputDatas.SinStartTime, inputDatas.SinEndTime, allOdrInfs, karteModels);

                return check ? new UpsertTodayOrdOutputData(UpsertTodayOrdStatus.Successed, RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid, new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>(), new Dictionary<int, TodayKarteValidationStatus>()) : new UpsertTodayOrdOutputData(UpsertTodayOrdStatus.Failed, RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid, new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>(), new Dictionary<int, TodayKarteValidationStatus>());
            }
            catch
            {
                return new UpsertTodayOrdOutputData(UpsertTodayOrdStatus.Failed, RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid, new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>(), new Dictionary<int, TodayKarteValidationStatus>());
            }
        }
    }
}
