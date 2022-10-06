using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.SystemGenerationConf;
using UseCase.OrdInfs.ValidationInputItem;
using static Helper.Constants.TodayOrderConst;

namespace Interactor.OrdInfs
{
    public class ValidationInputitemInteractor : IValidationInputItemInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ISystemGenerationConfRepository _systemGenerationConfRepository;
        public ValidationInputitemInteractor(IOrdInfRepository ordInfRepository, IMstItemRepository mstItemRepository, ISystemGenerationConfRepository systemGenerationConfRepository)
        {
            _ordInfRepository = ordInfRepository;
            _mstItemRepository = mstItemRepository;
            _systemGenerationConfRepository = systemGenerationConfRepository;
        }

        public ValidationInputItemOutputData Handle(ValidationInputItemInputData inputDatas)
        {
            try
            {
                var dicValidation = new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>();
                var allOdrInfs = new List<OrdInfModel>();
                var inputDataList = inputDatas.ToList();

                if (inputDataList.Count == 0)
                {
                    return new ValidationInputItemOutputData(dicValidation, ValidationInputItemStatus.Failed);
                }

                foreach (var item in inputDataList)
                {
                    var ordInf = new OrdInfModel(
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            item.OdrKouiKbn,
                            String.Empty,
                            item.InoutKbn,
                            0,
                            0,
                            0,
                            0,
                            item.DaysCnt,
                            0,
                            0,
                            0,
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
                            var inputItem = itemDetail == null ? null : _mstItemRepository.GetTenMst(item?.HpId ?? 0, item?.SinDate ?? 0, itemDetail?.ItemCd ?? string.Empty);
                            var refillSetting = itemDetail == null ? 999 : _systemGenerationConfRepository.GetSettingValue(item?.HpId ?? 0, 2002, 0, item?.SinDate ?? 0, 999);
                            var ipnMinYakaMst = (inputItem == null || (inputItem.HpId == 0 && string.IsNullOrEmpty(inputItem.ItemCd))) ? null : _ordInfRepository.FindIpnMinYakkaMst(item?.HpId ?? 0, inputItem?.IpnNameCd ?? string.Empty, item?.SinDate ?? 0);
                            var isCheckIpnKasanExclude = _ordInfRepository.CheckIsGetYakkaPrice(item?.HpId ?? 0, inputItem ?? new TenItemModel(), item?.SinDate ?? 0);

                            var ordInfDetail = new OrdInfDetailModel(
                                         0,
                                         0,
                                         0,
                                         0,
                                         0,
                                         0,
                                         0,
                                        itemDetail?.SinKouiKbn ?? 0,
                                        itemDetail?.ItemCd ?? string.Empty,
                                        itemDetail?.ItemName ?? string.Empty,
                                        itemDetail?.Suryo ?? 0,
                                        itemDetail?.UnitName ?? string.Empty,
                                        0,
                                        0,
                                         0,
                                        itemDetail?.SyohoKbn ?? 0,
                                        0,
                                        itemDetail?.DrugKbn ?? 0,
                                        itemDetail?.YohoKbn ?? 0,
                                        string.Empty,
                                        string.Empty,
                                        0,
                                        string.Empty,
                                        string.Empty,
                                        0,
                                        DateTime.MinValue,
                                        0,
                                        string.Empty,
                                        string.Empty,
                                        itemDetail?.Bunkatu ?? string.Empty,
                                        itemDetail?.CmtName ?? string.Empty,
                                        itemDetail?.CmtOpt ?? string.Empty,
                                        string.Empty,
                                        0,
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

                var count = 0;
                foreach (var item in allOdrInfs)
                {
                    var modelValidation = item.Validation(1);

                    if (modelValidation.Value != TodayOrdValidationStatus.Valid && !dicValidation.ContainsKey(count))
                    {
                        dicValidation.Add(count, modelValidation);
                    }

                    count++;
                }

                return new ValidationInputItemOutputData(dicValidation, ValidationInputItemStatus.Successed);
            }
            catch
            {
                return new ValidationInputItemOutputData(new Dictionary<int, KeyValuePair<int, TodayOrdValidationStatus>>(), ValidationInputItemStatus.Failed);
            }
        }
    }
}
