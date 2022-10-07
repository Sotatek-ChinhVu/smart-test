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
                var dicValidation = new Dictionary<string, KeyValuePair<string, TodayOrdValidationStatus>>();
                var inputDataList = inputDatas.ToList();

                //Check common
                if (inputDataList.Count == 0)
                {
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Failed);
                }
                if (inputDataList.Select(i => i.HpId).Distinct().Count() > 1 || inputDataList.FirstOrDefault()?.HpId <= 0)
                {
                    dicValidation.Add("-1", new("-1", TodayOrdValidationStatus.InvalidHpId));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }
                if (inputDataList.Select(i => i.PtId).Distinct().Count() > 1 || inputDataList.FirstOrDefault()?.PtId <= 0)
                {
                    dicValidation.Add("-1", new("-1", TodayOrdValidationStatus.InvalidPtId));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }
                if (inputDataList.Select(i => i.RaiinNo).Distinct().Count() > 1 || inputDataList.FirstOrDefault()?.RaiinNo <= 0)
                {
                    dicValidation.Add("-1", new("-1", TodayOrdValidationStatus.InvalidRaiinNo));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }
                if (inputDataList.Select(i => i.SinDate).Distinct().Count() > 1 || inputDataList.FirstOrDefault()?.SinDate <= 0)
                {
                    dicValidation.Add("-1", new("-1", TodayOrdValidationStatus.InvalidSinDate));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }


                var hpId = inputDataList[0].HpId;
                var ptId = inputDataList[0].PtId;
                var raiinNo = inputDataList[0].RaiinNo;
                var sinDate = inputDataList[0].SinDate;

                //Check exist common
                var checkHpId = _hpInfRepository.CheckHpId(hpId);
                if (!checkHpId)
                {
                    dicValidation.Add("-1", new("-1", TodayOrdValidationStatus.HpIdNoExist));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }

                var checkPtId = _patientInforRepository.CheckListId(new List<long> { ptId });
                if (!checkPtId)
                {
                    dicValidation.Add("-1", new("-1", TodayOrdValidationStatus.PtIdNoExist));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }

                var checkRaiinNo = _receptionRepository.CheckListNo(new List<long> { raiinNo });
                if (!checkRaiinNo)
                {
                    dicValidation.Add("-1", new("-1", TodayOrdValidationStatus.RaiinNoNoExist));
                    return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
                }

                CheckAllItemsConvert(dicValidation, hpId, ptId, inputDataList);

                var allOdrInfs = ConvertInputDataToOrderInfs(hpId, sinDate, inputDataList);

                //Check in model
                Parallel.ForEach(allOdrInfs, item =>
                {
                    var index = allOdrInfs.IndexOf(item);

                    var modelValidation = item.Validation(0);
                    if (modelValidation.Value != TodayOrdValidationStatus.Valid && !dicValidation.ContainsKey(index.ToString()))
                    {
                        dicValidation.Add(index.ToString(), modelValidation);
                    }
                });

                return new ValidationOrdInfListOutputData(dicValidation, ValidationOrdInfListStatus.Successed);
            }
            catch
            {
                return new ValidationOrdInfListOutputData(new Dictionary<string, KeyValuePair<string, TodayOrdValidationStatus>>(), ValidationOrdInfListStatus.Failed);
            }
        }

        private List<OrdInfModel> ConvertInputDataToOrderInfs(int hpId, int sinDate, List<ValidationOdrInfItem> inputDataList)
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
                                itemDetail.IpnName,
                                itemDetail.JissiKbn,
                                itemDetail.JissiDate ?? DateTime.MinValue,
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
                                0
                            );
                    ordInf.OrdInfDetails.Add(ordInfDetail);
                });

                allOdrInfs.Add(ordInf);
            });

            return allOdrInfs;
        }

        private void CheckAllItemsConvert(Dictionary<string, KeyValuePair<string, TodayOrdValidationStatus>> dicValidation, int hpId, long ptId, List<ValidationOdrInfItem> inputDataList)
        {
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
                            dicValidation.Add(index.ToString(), new("-1", TodayOrdValidationStatus.InvalidTodayOrdUpdatedNoExist));
                            return;
                        }
                    }

                    var checkObjs = inputDataList.Where(o => item.Id > 0 && o.RpNo == item.RpNo).ToList();
                    var positionOrd = inputDataList.FindIndex(o => o == checkObjs.LastOrDefault());
                    if (checkObjs.Count >= 2 && positionOrd == index)
                    {
                        dicValidation.Add(positionOrd.ToString(), new("-1", TodayOrdValidationStatus.DuplicateTodayOrd));
                        return;
                    }

                    var checkHokenPid = checkHokens.Any(h => h.HpId == item.HpId && h.PtId == item.PtId && h.HokenId == item.HokenPid);
                    if (!checkHokenPid)
                    {
                        dicValidation.Add(index.ToString(), new("-1", TodayOrdValidationStatus.HokenPidNoExist));
                        return;
                    }

                    Parallel.ForEach(item.OdrDetails, itemOd =>
                    {
                        var indexOd = item.OdrDetails.IndexOf(itemOd);

                        if (item.RpNo != itemOd.RpNo || item.RpEdaNo != itemOd.RpEdaNo || item.HpId != itemOd.HpId || item.PtId != itemOd.PtId || item.SinDate != itemOd.SinDate || item.RaiinNo != itemOd.RaiinNo)
                        {
                            dicValidation.Add(index.ToString(), new(indexOd.ToString(), TodayOrdValidationStatus.OdrNoMapOdrDetail));
                        }
                    });
                }
            });
        }
    }
}
