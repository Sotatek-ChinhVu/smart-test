using Domain.Models.InputItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.SystemGenerationConf;
using UseCase.OrdInfs.Validation;
using static Helper.Constants.TodayOrderConst;

namespace Interactor.OrdInfs
{
    public class ValidationOrdInfListInteractor : IValidationOrdInfListInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IInputItemRepository _inputItemRepository;
        private readonly ISystemGenerationConfRepository _systemGenerationConfRepository;
        public ValidationOrdInfListInteractor(IOrdInfRepository ordInfRepository, IInputItemRepository inputItemRepository, ISystemGenerationConfRepository systemGenerationConfRepository)
        {
            _ordInfRepository = ordInfRepository;
            _inputItemRepository = inputItemRepository;
            _systemGenerationConfRepository = systemGenerationConfRepository;
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
                            ""
                        );
                    if (item != null)
                    {
                        foreach (var itemDetail in item.OdrDetails)
                        {
                            var inputItem = itemDetail == null ? null : _inputItemRepository.GetTenMst(itemDetail.HpId, itemDetail.SinDate, itemDetail?.ItemCd ?? string.Empty);
                            var refillSetting = itemDetail == null ? 999 : _systemGenerationConfRepository.GetSettingValue(itemDetail.HpId, 2002, 0, itemDetail?.SinDate ?? 0, 999);
                            var ipnMinYakaMst = (inputItem == null || (inputItem.HpId == 0 && string.IsNullOrEmpty(inputItem.ItemCd))) ? null : _ordInfRepository.FindIpnMinYakkaMst(itemDetail?.HpId ?? 0, inputItem?.IpnNameCd ?? string.Empty, itemDetail?.SinDate ?? 0);
                            var isCheckIpnKasanExclude = _ordInfRepository.CheckIsGetYakkaPrice(itemDetail?.HpId ?? 0, inputItem, itemDetail?.SinDate ?? 0);

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
                                        ""
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
