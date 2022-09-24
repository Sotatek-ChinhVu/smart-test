using Domain.Models.HpMst;
using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SystemGenerationConf;
using UseCase.OrdInfs.Validation;
using static Helper.Constants.TodayOrderConst;

namespace Interactor.OrdInfs
{
    public class ValidationOrdInfListInteractor : IValidationOrdInfListInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ISystemGenerationConfRepository _systemGenerationConfRepository;
        private readonly IHpInfRepository _hpInfRepository;
        private readonly IInsuranceRepository _insuranceInforRepository;
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IReceptionRepository _receptionRepository;

        public ValidationOrdInfListInteractor(IOrdInfRepository ordInfRepository, IMstItemRepository mstItemRepository, ISystemGenerationConfRepository systemGenerationConfRepository, IHpInfRepository hpInfRepository, IInsuranceRepository insuranceInforRepository, IPatientInforRepository patientInforRepository, IReceptionRepository receptionRepository)
        {
            _ordInfRepository = ordInfRepository;
            _mstItemRepository = mstItemRepository;
            _systemGenerationConfRepository = systemGenerationConfRepository;
            _hpInfRepository = hpInfRepository;
            _insuranceInforRepository = insuranceInforRepository;
            _patientInforRepository = patientInforRepository;
            _receptionRepository = receptionRepository;
        }

        public ValidationOrdInfListOutputData Handle(ValidationOrdInfListInputData inputDatas)
        {
            try
            {
                var dicValidation = new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>();
                var allOdrInfs = new List<OrdInfModel>();
                var inputDataList = inputDatas.ToList();
                if (inputDataList.Select(i => i.HpId).Distinct().Count() > 1 || inputDataList.FirstOrDefault()?.HpId <= 0)
                {
                    dicValidation.Add(-1, new(-1, TodayOrdValidationStatus.InvalidHpId));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }
                if (inputDataList.Select(i => i.PtId).Distinct().Count() > 1 || inputDataList.FirstOrDefault()?.PtId <= 0)
                {
                    dicValidation.Add(-1, new(-1, TodayOrdValidationStatus.InvalidPtId));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }
                if (inputDataList.Select(i => i.RaiinNo).Distinct().Count() > 1 || inputDataList.FirstOrDefault()?.RaiinNo <= 0)
                {
                    dicValidation.Add(-1, new(-1, TodayOrdValidationStatus.InvalidRaiinNo));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }
                if (inputDataList.Select(i => i.SinDate).Distinct().Count() > 1 || inputDataList.FirstOrDefault()?.SinDate <= 0)
                {
                    dicValidation.Add(-1, new(-1, TodayOrdValidationStatus.InvalidSinDate));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }


                var hpId = inputDataList[0].HpId;
                var ptId = inputDataList[0].PtId;
                var raiinNo = inputDataList[0].RaiinNo;
                var sinDate = inputDataList[0].SinDate;

                var checkHpId = _hpInfRepository.CheckHpId(hpId);
                if (!checkHpId)
                {
                    dicValidation.Add(-1, new(-1, TodayOrdValidationStatus.HpIdNoExist));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }

                var checkPtId = _patientInforRepository.CheckListId(new List<long> { ptId });
                if (!checkPtId)
                {
                    dicValidation.Add(-1, new(-1, TodayOrdValidationStatus.PtIdNoExist));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }

                var checkRaiinNo = _receptionRepository.CheckListNo(new List<long> { raiinNo });
                if (!checkRaiinNo)
                {
                    dicValidation.Add(-1, new(-1, TodayOrdValidationStatus.RaiinNoNoExist));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }


                var raiinNos = inputDataList.Select(i => i.RaiinNo).Distinct().ToList();
                var checkOderInfs = _ordInfRepository.GetListToCheckValidate(ptId, hpId, raiinNos ?? new List<long>());

                var hokenPids = inputDataList.Select(i => i.HokenPid).Distinct().ToList();
                var checkHokens = _insuranceInforRepository.GetCheckListHokenInf(hpId, ptId, hokenPids ?? new List<int>());
                object obj = new();
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

                        var checkHokenPid = checkHokens.Any(h => h.HpId == item.HpId && h.PtId == item.PtId && h.HokenId == item.HokenPid);
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

                return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
            }
            catch
            {
                return new ValidationOrdInfListOutputData(new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>(), ValidationOrdInfListStatus.Faild);
            }
        }
    }
}
