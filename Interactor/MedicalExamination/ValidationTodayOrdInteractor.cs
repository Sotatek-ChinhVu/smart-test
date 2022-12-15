using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.Ka;
using Domain.Models.KarteInfs;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SystemGenerationConf;
using Domain.Models.User;
using Helper.Constants;
using UseCase.OrdInfs.ValidationTodayOrd;
using static Helper.Constants.KarteConst;
using static Helper.Constants.OrderInfConst;

namespace Interactor.MedicalExamination
{
    public class ValidationTodayOrdInteractor : IValidationTodayOrdInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ISystemGenerationConfRepository _systemGenerationConfRepository;
        private readonly IHpInfRepository _hpInfRepository;
        private readonly IInsuranceRepository _insuranceInforRepository;
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IReceptionRepository _receptionRepository;
        private readonly IKaRepository _kaRepository;
        private readonly IUserRepository _userRepository;

        public ValidationTodayOrdInteractor(IOrdInfRepository ordInfRepository, IMstItemRepository mstItemRepository, ISystemGenerationConfRepository systemGenerationConfRepository, IHpInfRepository hpInfRepository, IInsuranceRepository insuranceInforRepository, IPatientInforRepository patientInforRepository, IReceptionRepository receptionRepository, IKaRepository kaRepository, IUserRepository userRepository)
        {
            _ordInfRepository = ordInfRepository;
            _mstItemRepository = mstItemRepository;
            _systemGenerationConfRepository = systemGenerationConfRepository;
            _hpInfRepository = hpInfRepository;
            _insuranceInforRepository = insuranceInforRepository;
            _patientInforRepository = patientInforRepository;
            _receptionRepository = receptionRepository;
            _kaRepository = kaRepository;
            _userRepository = userRepository;
        }

        public ValidationTodayOrdOutputData Handle(ValidationTodayOrdInputData inputDatas)
        {
            try
            {
                var dicValidation = new Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>();
                var inputDataList = inputDatas.ToList();
                var hpIds = inputDataList.Select(x => x.HpId).ToList();
                hpIds.Add(inputDatas.Karte.HpId);
                hpIds = hpIds.Distinct().ToList();
                var ptIds = inputDataList.Select(x => x.PtId).ToList();
                ptIds.Add(inputDatas.Karte.PtId);
                ptIds = ptIds.Distinct().ToList();
                var raiinNos = inputDataList.Select(x => x.RaiinNo).ToList();
                raiinNos.Add(inputDatas.Karte.RaiinNo);
                raiinNos = raiinNos.Distinct().ToList();
                var sinDates = inputDataList.Select(x => x.SinDate).ToList();
                sinDates.Add(inputDatas.Karte.SinDate);
                sinDates = sinDates.Distinct().ToList();

                var hpId = hpIds[0];
                var ptId = ptIds[0];
                var raiinNo = raiinNos[0];
                var sinDate = sinDates[0];

                //Check common
                var raiinInfStatus = CheckCommon(hpIds, ptIds, raiinNos, sinDates, hpId, ptId, raiinNo);

                if (raiinInfStatus != RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid)
                {
                    return new ValidationTodayOrdOutputData(ValidationTodayOrdStatus.Failed, new Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>(), raiinInfStatus, KarteValidationStatus.Valid);
                }
                raiinInfStatus = CheckRaiinInf(inputDatas);

                CheckAllItemsConvert(dicValidation, hpId, ptId, inputDataList);

                var allOdrInfs = ConvertInputDataToOrderInfs(hpId, sinDate, inputDataList);

                //Check in model
                var obj = new object();
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

                // Karte
                var karteModel = new KarteInfModel(
                        inputDatas.Karte.HpId,
                        inputDatas.Karte.RaiinNo,
                        1,
                        0,
                        inputDatas.Karte.PtId,
                        inputDatas.Karte.SinDate,
                        inputDatas.Karte.Text,
                        inputDatas.Karte.IsDeleted,
                        inputDatas.Karte.RichText,
                        DateTime.MinValue,
                        DateTime.MinValue,
                        ""
                    );

                var validateKarte = karteModel.Validation();

                return new ValidationTodayOrdOutputData(ValidationTodayOrdStatus.Successed, dicValidation, raiinInfStatus, validateKarte);
            }
            catch
            {
                return new ValidationTodayOrdOutputData(ValidationTodayOrdStatus.Failed, new Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>(), RaiinInfConst.RaiinInfTodayOdrValidationStatus.Valid, KarteValidationStatus.Valid);
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
            var refillSetting = _systemGenerationConfRepository.GetSettingValue(hpId, 2002, 0, sinDate, 999).Item1;
            var checkIsGetYakkaPrices = _ordInfRepository.CheckIsGetYakkaPrices(hpId, tenMsts ?? new List<TenItemModel>(), sinDate);

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
                        ""
                    );

                var objDetail = new object();
                Parallel.ForEach(item.OdrDetails, itemDetail =>
                {
                    var inputItem = itemDetail == null ? null : tenMsts?.FirstOrDefault(t => t.ItemCd == itemDetail.ItemCd);
                    refillSetting = itemDetail == null ? 999 : refillSetting;
                    var ipnMinYakaMst = inputItem == null || inputItem.HpId == 0 && string.IsNullOrEmpty(inputItem.ItemCd) ? null : ipnMinYakaMsts.FirstOrDefault(i => i.IpnNameCd == itemDetail?.IpnCd);
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
                                0,
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

        private void CheckAllItemsConvert(Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> dicValidation, int hpId, long ptId, List<ValidationOdrInfItem> inputDataList)
        {
            var raiinNos = inputDataList.Select(i => i.RaiinNo).Distinct().ToList();
            var checkOderInfs = _ordInfRepository.GetListToCheckValidate(ptId, hpId, raiinNos ?? new List<long>());

            var hokenPids = inputDataList.Select(i => i.HokenPid).Distinct().ToList();
            var checkHokens = _insuranceInforRepository.GetCheckListHokenInf(hpId, ptId, hokenPids ?? new List<int>());
            object obj = new();
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

                var checkHokenPid = checkHokens.Any(h => h.HpId == item.HpId && h.PtId == item.PtId && h.HokenId == item.HokenPid);
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

        private RaiinInfConst.RaiinInfTodayOdrValidationStatus CheckRaiinInf(ValidationTodayOrdInputData inputDatas)
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

        private RaiinInfConst.RaiinInfTodayOdrValidationStatus CheckCommon(List<int> hpIds, List<long> ptIds, List<long> raiinNos, List<int> sinDates, int hpId, long ptId, long raiinNo)
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
                var checkPtId = _patientInforRepository.CheckExistListId(new List<long> { ptId });
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

            return raiinInfStatus;
        }

        private void AddErrorStatus(object obj, Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> dicValidation, string key, KeyValuePair<string, OrdInfValidationStatus> status)
        {
            lock (obj)
            {
                dicValidation.Add(key, status);
            }
        }
    }
}
