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

                var count = 0;
                foreach (var item in inputDataList)
                {
                    var check = _ordInfRepository.CheckExistOrder(item.RpNo, item.RpEdaNo);
                    if (!check && item.Status == 1)
                    {
                        dicValidation.Add(count, new(-1, TodayOrdValidationStatus.InvalidTodayOrdUpdatedNoExist));
                    }
                    else if (check && item.Status == 0)
                    {
                        dicValidation.Add(count, new(-1, TodayOrdValidationStatus.InvalidTodayOrdInsertedExist));
                    }

                    var checkObjs = inputDataList.Where(o => o.RpNo == item.RpNo && o.RpEdaNo == item.RpEdaNo).ToList();
                    var positionOrd = inputDataList.FindIndex(o => o == checkObjs.LastOrDefault());
                    if (checkObjs.Count >= 2 && !dicValidation.ContainsKey(positionOrd))
                    {
                        dicValidation.Add(positionOrd, new(-1, TodayOrdValidationStatus.DuplicateTodayOrd));
                    }

                    var checkHpId = _hpInfRepository.CheckHpId(item.HpId);
                    if (!checkHpId && !dicValidation.ContainsKey(count))
                    {
                        dicValidation.Add(count, new(-1, TodayOrdValidationStatus.HpIdNoExist));
                    }

                    var checkRaiinNo = _receptionRepository.CheckListNo(new List<long> { item.RaiinNo });
                    if (!checkRaiinNo && !dicValidation.ContainsKey(count))
                    {
                        dicValidation.Add(count, new(-1, TodayOrdValidationStatus.RaiinNoNoExist));
                    }

                    var checkPtId = _patientInforRepository.CheckListId(new List<long> { item.PtId });
                    if (!checkPtId && !dicValidation.ContainsKey(count))
                    {
                        dicValidation.Add(count, new(-1, TodayOrdValidationStatus.PtIdNoExist));
                    }

                    var checkHokenPid = _insuranceInforRepository.CheckHokenPid(item.HokenPid);
                    if (!checkHokenPid && !dicValidation.ContainsKey(count))
                    {
                        dicValidation.Add(count, new(-1, TodayOrdValidationStatus.HokenPidNoExist));
                    }

                    var countOd = 0;
                    foreach (var itemOd in item.OdrDetails)
                    {
                        if ((item.RpNo != itemOd.RpNo || item.RpEdaNo != itemOd.RpEdaNo || item.HpId != itemOd.HpId || item.PtId != itemOd.PtId && item.SinDate != itemOd.SinDate) && !dicValidation.ContainsKey(count))
                        {
                            dicValidation.Add(count, new(countOd, TodayOrdValidationStatus.OdrNoMapOdrDetail));
                        }

                        var checkHpIdOd = _hpInfRepository.CheckHpId(itemOd.HpId);
                        if (!checkHpIdOd && !dicValidation.ContainsKey(count) && !dicValidation.Values.Any(d => d.Key == countOd))
                        {
                            dicValidation.Add(count, new(countOd, TodayOrdValidationStatus.HpIdNoExist));
                        }

                        var checkRaiinNoOd = _receptionRepository.CheckListNo(new List<long> { itemOd.RaiinNo });
                        if (!checkRaiinNoOd && !dicValidation.ContainsKey(count) && !dicValidation.Values.Any(d => d.Key == countOd))
                        {
                            dicValidation.Add(count, new(countOd, TodayOrdValidationStatus.RaiinNoNoExist));
                        }

                        var checkPtIdOd = _patientInforRepository.CheckListId(new List<long> { itemOd.PtId });
                        if (!checkPtIdOd && !dicValidation.ContainsKey(count) && !dicValidation.Values.Any(d => d.Key == countOd))
                        {
                            dicValidation.Add(count, new(countOd, TodayOrdValidationStatus.PtIdNoExist));
                        }

                        countOd++;
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

                return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
            }
            catch
            {
                return new ValidationOrdInfListOutputData(new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>(), ValidationOrdInfListStatus.Faild);
            }
        }
    }
}
