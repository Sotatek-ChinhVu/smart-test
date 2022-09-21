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
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using UseCase.MedicalExamination.UpsertTodayOrd;
using static Helper.Constants.TodayKarteConst;
using static Helper.Constants.TodayOrderConst;

namespace Interactor.MedicalExamination
{
    public class UpsertTodayOrdInteractor : IUpsertTodayOrdInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IKarteInfRepository _karteInfRepository;
        private readonly IReceptionRepository _receptionRepository;
        private readonly IKaMstRepository _kaRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ISystemGenerationConfRepository _systemGenerationConfRepository;
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IKarteKbnMstRepository _karteKbnInforRepository;
        private readonly IInsuranceRepository _insuranceInforRepository;
        private readonly IUserRepository _userRepository;
        private readonly TenantDataContext _tenantTrackingDataContext;

        public UpsertTodayOrdInteractor(IOrdInfRepository ordInfRepository, IKarteInfRepository karteInfRepository, IReceptionRepository receptionRepository, IKaMstRepository kaRepository, IMstItemRepository mstItemRepository, ISystemGenerationConfRepository systemGenerationConfRepository, IPatientInforRepository patientInforRepository, IKarteKbnMstRepository karteKbnInforRepository, IInsuranceRepository insuranceInforRepository, IUserRepository userRepository, ITenantProvider tenantProvider)
        {
            _ordInfRepository = ordInfRepository;
            _karteInfRepository = karteInfRepository;
            _kaRepository = kaRepository;
            _receptionRepository = receptionRepository;
            _mstItemRepository = mstItemRepository;
            _systemGenerationConfRepository = systemGenerationConfRepository;
            _patientInforRepository = patientInforRepository;
            _karteKbnInforRepository = karteKbnInforRepository;
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
            _insuranceInforRepository = insuranceInforRepository;
            _userRepository = userRepository;
        }

        public UpsertTodayOrdOutputData Handle(UpsertTodayOrdInputData inputData)
        {
            var executionStrategy = _tenantTrackingDataContext.Database.CreateExecutionStrategy();

            return executionStrategy.Execute(
                () =>
                {
                    using (var transaction = _tenantTrackingDataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.Valid;
                            //Raiin Info
                            if (!RaiinState.ReceptionStatusToText.Keys.Contains(inputData.Status))
                            {
                                raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.InvalidStatus;
                            }
                            else if (!(inputData.SyosaiKbn >= 0 && inputData.SyosaiKbn <= 8))
                            {
                                raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.InvalidSyosaiKbn;
                            }
                            else if (!(inputData.JikanKbn >= 0 && inputData.JikanKbn <= 7))
                            {
                                raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.InvalidJikanKbn;
                            }
                            else if (inputData.HokenPid < 0)
                            {
                                raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.InvalidHokenPid;
                            }
                            else if (!(inputData.SanteiKbn >= 0 && inputData.SanteiKbn <= 2))
                            {
                                raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.InvalidSanteiKbn;
                            }
                            if (inputData.TantoId < 0)
                            {
                                raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.InvalidTantoId;
                            }
                            else if (inputData.KaId < 0)
                            {
                                raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.InvalidKaId;
                            }

                            else if (inputData.UketukeTime.Length > 6)
                            {
                                raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.InvalidUKetukeTime;
                            }
                            else if (inputData.SinStartTime.Length > 6)
                            {
                                raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.InvalidSinStartTime;

                            }
                            else if (inputData.SinEndTime.Length > 6)
                            {
                                raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.InvalidSinEndTime;
                            }

                            if (inputData.HokenPid > 0)
                            {
                                var checkHokenId = _insuranceInforRepository.CheckHokenPid(inputData.HokenPid);
                                if (!checkHokenId)
                                {
                                    raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.HokenPidNoExist;
                                }
                            }

                            if (inputData.TantoId > 0)
                            {
                                var checkHokenId = _userRepository.CheckExistedUserId(inputData.TantoId);
                                if (!checkHokenId)
                                {
                                    raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.TatoIdNoExist;
                                }
                            }

                            if (inputData.KaId > 0)
                            {
                                var checkHokenId = _kaRepository.CheckKaId(inputData.KaId);
                                if (!checkHokenId)
                                {
                                    raiinInfStatus = RaiinInfConst.RaiinInfValidationStatus.KaIdNoExist;
                                }
                            }

                            // Odr Info
                            var dicValidation = new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>();
                            var allOdrInfs = new List<OrdInfModel>();
                            var inputDataList = inputData.OdrItems.ToList();

                            var count = 0;
                            foreach (var item in inputDataList)
                            {
                                var check = _ordInfRepository.CheckExistOrder(item.RpNo, item.RpEdaNo);
                                if (!check && item.Id > 0 && item.IsDeleted == 0)
                                {
                                    dicValidation.Add(count, new(-1, TodayOrdValidationStatus.InvalidTodayOrdUpdatedNoExist));
                                }
                                else if (check && item.Id == 0 && item.IsDeleted == 0)
                                {
                                    dicValidation.Add(count, new(-1, TodayOrdValidationStatus.InvalidTodayOrdInsertedExist));
                                }

                                var checkObjs = inputDataList.Where(o => o.RpNo == item.RpNo && o.RpEdaNo == item.RpEdaNo).ToList();
                                var positionOrd = inputDataList.FindIndex(o => o == checkObjs.LastOrDefault());
                                if (checkObjs.Count >= 2 && !dicValidation.ContainsKey(positionOrd))
                                {
                                    dicValidation.Add(positionOrd, new(-1, TodayOrdValidationStatus.DuplicateTodayOrd));
                                }

                                count++;
                            }

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
                                        DateTime.MinValue
                                    );
                                if (item != null)
                                {
                                    foreach (var itemDetail in item.OdrDetails)
                                    {
                                        var inputItem = itemDetail == null ? null : _mstItemRepository.GetTenMst(itemDetail.HpId, itemDetail.SinDate, itemDetail?.ItemCd ?? string.Empty);
                                        var refillSetting = itemDetail == null ? 999 : _systemGenerationConfRepository.GetSettingValue(itemDetail.HpId, 2002, 0, itemDetail?.SinDate ?? 0, 999);
                                        var ipnMinYakaMst = (inputItem == null || (inputItem.HpId == 0 && string.IsNullOrEmpty(inputItem.ItemCd))) ? null : _ordInfRepository.FindIpnMinYakkaMst(itemDetail?.HpId ?? 0, inputItem?.IpnNameCd ?? string.Empty, itemDetail?.SinDate ?? 0);
                                        var isCheckIpnKasanExclude = _ordInfRepository.CheckIsGetYakkaPrice(itemDetail?.HpId ?? 0, inputItem ?? new TenItemModel(), itemDetail?.SinDate ?? 0);

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
                                    }
                                }

                                allOdrInfs.Add(ordInf);
                            }

                            count = 0;
                            foreach (var item in allOdrInfs)
                            {
                                var modelValidation = item.Validation();
                                if (modelValidation.Value != TodayOrdValidationStatus.Valid && !dicValidation.ContainsKey(count))
                                {
                                    dicValidation.Add(count, modelValidation);
                                }

                                count++;
                            }

                            // Karte
                            var dicKarteValidation = new Dictionary<int, TodayKarteValidationStatus>();
                            var inputKarteDataList = inputData.KarteInfs.ToList();

                            var karteModels = inputKarteDataList.Select(k => new KarteInfModel(
                                    k.HpId,
                                    k.RaiinNo,
                                    k.KarteKbn,
                                    0,
                                    k.PtId,
                                    k.SinDate,
                                    k.Text,
                                    k.IsDeleted,
                                    k.RichText,
                                    DateTime.MinValue,
                                    DateTime.MinValue
                                )).ToList();

                            count = 0;
                            foreach (var karte in karteModels)
                            {
                                var modelValidation = karte.Validation();
                                if (modelValidation != TodayKarteValidationStatus.Valid && !dicKarteValidation.ContainsKey(count))
                                {
                                    dicKarteValidation.Add(count, modelValidation);
                                }

                                var checkRaiinNo = _receptionRepository.CheckListNo(new List<long> { karte.RaiinNo });
                                if (!checkRaiinNo && !dicKarteValidation.ContainsKey(count))
                                {
                                    dicKarteValidation.Add(count, TodayKarteValidationStatus.RaiinNoNoExist);
                                }

                                var checkPtId = _patientInforRepository.CheckListId(new List<long> { karte.PtId });
                                if (!checkPtId && !dicKarteValidation.ContainsKey(count))
                                {
                                    dicKarteValidation.Add(count, TodayKarteValidationStatus.PtIdNoExist);
                                }

                                var checkKarteKbn = _karteKbnInforRepository.CheckKarteKbn(karte.KarteKbn);
                                if (!checkKarteKbn && !dicKarteValidation.ContainsKey(count))
                                {
                                    dicKarteValidation.Add(count, TodayKarteValidationStatus.KarteKbnNoExist);
                                }

                                count++;
                            }

                            if (raiinInfStatus != RaiinInfConst.RaiinInfValidationStatus.Valid || dicKarteValidation.Any() || dicValidation.Any())
                            {
                                return new UpsertTodayOrdOutputData(UpsertTodayOrdStatus.Failed, raiinInfStatus, dicValidation, dicKarteValidation);
                            }

                            if (inputData.OdrItems.Count > 0)
                            {
                                int hpId = inputData.OdrItems[0].HpId;
                                long raiinNo = inputData.OdrItems[0].RaiinNo;
                                long ptId = inputData.OdrItems[0].PtId;
                                int sinDate = inputData.OdrItems[0].SinDate;

                                _receptionRepository.SaveRaiinInfTodayOdr(inputData.Status, hpId, ptId, raiinNo, sinDate, inputData.SyosaiKbn, inputData.JikanKbn, inputData.HokenPid, inputData.SanteiKbn, inputData.TantoId, inputData.KaId, inputData.UketukeTime, inputData.SinStartTime, inputData.SinEndTime);
                            }
                            _ordInfRepository.Upsert(allOdrInfs);
                            _karteInfRepository.Upsert(karteModels);
                            _ordInfRepository.SaveRaiinListInf(allOdrInfs);

                            transaction.Commit();

                            return new UpsertTodayOrdOutputData(UpsertTodayOrdStatus.Successed, RaiinInfConst.RaiinInfValidationStatus.Valid, new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>(), new Dictionary<int, TodayKarteValidationStatus>());
                        }
                        catch
                        {
                            transaction.Rollback();

                            return new UpsertTodayOrdOutputData(UpsertTodayOrdStatus.Failed, RaiinInfConst.RaiinInfValidationStatus.Valid, new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>(), new Dictionary<int, TodayKarteValidationStatus>());
                        }
                    }
                });
        }
    }
}
