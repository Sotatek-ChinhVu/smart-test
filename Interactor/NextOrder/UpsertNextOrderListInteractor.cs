using Domain.Models.HpMst;
using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.NextOrder;
using Domain.Models.PatientInfor;
using Domain.Models.User;
using UseCase.NextOrder.Upsert;
using static Helper.Constants.KarteConst;
using static Helper.Constants.NextOrderConst;
using static Helper.Constants.OrderInfConst;
using static Helper.Constants.RsvkrtByomeiConst;

namespace Interactor.NextOrder
{
    public class UpsertNextOrderListInteractor : IUpsertNextOrderListInputPort
    {
        private readonly INextOrderRepository _nextOrderRepository;
        private readonly IHpInfRepository _hpInfRepository;
        private readonly IPatientInforRepository _patientInfRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IMstItemRepository _mstItemRepository;

        public UpsertNextOrderListInteractor(INextOrderRepository nextOrderRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInfRepository, IUserRepository userRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
        {
            _nextOrderRepository = nextOrderRepository;
            _hpInfRepository = hpInfRepository;
            _patientInfRepository = patientInfRepository;
            _userRepository = userRepository;
            _insuranceRepository = insuranceRepository;
            _mstItemRepository = mstItemRepository;
        }

        public UpsertNextOrderListOutputData Handle(UpsertNextOrderListInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0 || !_hpInfRepository.CheckHpId(inputData.HpId))
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidHpId, new(), new(), new(), new());
                }
                if (inputData.PtId <= 0 || !_patientInfRepository.CheckExistListId(new List<long> { inputData.PtId }))
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidPtId, new(), new(), new(), new());
                }
                if (inputData.UserId <= 0 || !_userRepository.CheckExistedUserId(inputData.UserId))
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidUserId, new(), new(), new(), new());
                }

                var itemCds = new List<string>();
                var ipnCds = new List<Tuple<string, string>>();
                List<long> rsvkrtNos = new();
                List<int> rsvkrtDates = new();

                foreach (var nextOrder in inputData.NextOrderItems)
                {
                    rsvkrtNos.Add(nextOrder.RsvkrtNo);
                    rsvkrtNos.Add(nextOrder.rsvkrtKarteInf.RsvkrtNo);
                    rsvkrtDates.Add(nextOrder.RsvDate);
                    rsvkrtDates.Add(nextOrder.rsvkrtKarteInf.RsvDate);
                    foreach (var orderInfModel in nextOrder.rsvKrtOrderInfItems)
                    {
                        rsvkrtNos.Add(orderInfModel.RsvkrtNo);
                        rsvkrtNos.AddRange(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.RsvkrtNo));
                        rsvkrtDates.Add(orderInfModel.RsvDate);
                        rsvkrtDates.AddRange(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.RsvDate));
                        itemCds.AddRange(_mstItemRepository.GetCheckItemCds(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.ItemCd.Trim()).ToList()));
                        ipnCds.AddRange(_mstItemRepository.GetCheckIpnCds(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.IpnCd.Trim()).ToList()));
                    }
                    foreach (var byomei in nextOrder.rsvKrtByomeiItems)
                    {
                        rsvkrtNos.Add(byomei.RsvkrtNo);
                    }
                }

                if (rsvkrtNos.Distinct().Count() > 1)
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidRsvkrtNo, new(), new(), new(), new());
                }

                if (rsvkrtDates.Distinct().Count() > 1)
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidRsvkrtDate, new(), new(), new(), new());
                }

                var nextOrderModels = inputData.NextOrderItems.Select(n => ConvertNextOrderToModel(inputData.HpId, inputData.PtId, ipnCds, n)).ToList();
                List<(int, KarteValidationStatus)> validationKarteInfs = new();
                Dictionary<int, NextOrderStatus> validationNextOrders = new();
                List<(int, int, RsvkrtByomeiStatus)> validationRsvkrtByomeis = new();
                var validationOrdInfs = new List<(int, Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>)>();

                for (int i = 0; i < nextOrderModels.Count; i++)
                {
                    var nextOrderModel = nextOrderModels[i];

                    var validationNextOrder = nextOrderModel.Validation();
                    if (validationNextOrder != NextOrderStatus.Valid)
                    {
                        validationNextOrders.Add(i, validationNextOrder);
                    }

                    var validationKarteInf = nextOrderModel.RsvkrtKarteInf.Validation();
                    if (validationKarteInf != KarteValidationStatus.Valid)
                    {
                        validationKarteInfs.Add((i, validationKarteInf));
                    }

                    for (int i1 = 0; i1 < nextOrderModel.RsvkrtByomeis.Count; i1++)
                    {
                        var byomei = nextOrderModel.RsvkrtByomeis[i1];
                        var validationByomei = byomei.Validation();
                        if (validationKarteInf != KarteValidationStatus.Valid)
                        {
                            validationRsvkrtByomeis.Add((i, i1, validationByomei));
                        }
                    }

                    var validationOneOrdInf = CheckValidateOrderInf(inputData.HpId, inputData.PtId, itemCds, ipnCds.Select(ipn => ipn.Item1).ToList(), nextOrderModel);

                    if (validationOneOrdInf.Any())
                        validationOrdInfs.Add((i, validationOneOrdInf));
                }
                if (validationKarteInfs.Count > 0 || validationNextOrders.Count > 0 || validationRsvkrtByomeis.Count > 0 || validationOrdInfs.Count > 0)
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.Failed, validationNextOrders, validationOrdInfs, validationKarteInfs, validationRsvkrtByomeis);
                }
                _nextOrderRepository.Upsert(inputData.UserId, inputData.HpId, inputData.PtId, nextOrderModels);
                return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.Successed, new(), new(), new(), new());
            }
            catch
            {
                return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.Failed, new(), new(), new(), new());
            }
        }

        private Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> CheckValidateOrderInf(int hpId, long ptId, List<string> itemCds, List<string> ipnCds, NextOrderModel nextOrderModel)
        {
            Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> validationOneOrdInf = new();
            for (int i2 = 0; i2 < nextOrderModel.RsvkrtOrderInfs.Count; i2++)
            {
                var order = nextOrderModel.RsvkrtOrderInfs[i2];
                var validationOrderInf = order.Validation(0);
                if (validationOrderInf.Value != OrdInfValidationStatus.Valid)
                {
                    validationOneOrdInf.Add(i2.ToString(), validationOrderInf);
                }
            }

            var checkOderInfs = _nextOrderRepository.GetCheckOrderInfs(hpId, ptId);
            var hokenPids = new List<int>();
            hokenPids = nextOrderModel.RsvkrtOrderInfs.Select(i => i.HokenPid).Distinct().ToList();

            var checkHokens = _insuranceRepository.GetCheckListHokenInf(hpId, ptId, hokenPids ?? new List<int>());
            object obj = new();
            Parallel.For(0, nextOrderModel.RsvkrtOrderInfs.Count, index =>
            {
                var item = nextOrderModel.RsvkrtOrderInfs[index];

                if (item.Id > 0)
                {
                    var check = checkOderInfs.Any(c => c.RsvkrtNo == item.RsvkrtNo && c.RsvDate == item.RsvDate && c.RpNo == item.RpNo && c.RpEdaNo == item.RpEdaNo);
                    if (!check)
                    {
                        AddErrorStatus(obj, validationOneOrdInf, index.ToString(), new("-1", OrdInfValidationStatus.InvalidTodayOrdUpdatedNoExist));
                        return;
                    }
                }

                var checkObjs = nextOrderModel.RsvkrtOrderInfs.Where(o => item.Id > 0 && o.RpNo == item.RpNo).ToList();
                var positionOrd = nextOrderModel.RsvkrtOrderInfs.FindIndex(o => o == checkObjs.LastOrDefault());
                if (checkObjs.Count >= 2 && positionOrd == index)
                {
                    AddErrorStatus(obj, validationOneOrdInf, positionOrd.ToString(), new("-1", OrdInfValidationStatus.DuplicateTodayOrd));
                    return;
                }

                var checkHokenPid = checkHokens.Any(h => h.HokenId == item.HokenPid);
                if (!checkHokenPid)
                {
                    AddErrorStatus(obj, validationOneOrdInf, index.ToString(), new("-1", OrdInfValidationStatus.HokenPidNoExist));

                    return;
                }

                var odrDetail = item.OrdInfDetails.FirstOrDefault(itemOd => item.RpNo != itemOd.RpNo || item.RpEdaNo != itemOd.RpEdaNo || item.RsvDate != itemOd.RsvDate || item.RsvkrtNo != itemOd.RsvkrtNo);
                if (odrDetail != null)
                {
                    var indexOdrDetail = item.OrdInfDetails.IndexOf(odrDetail);
                    AddErrorStatus(obj, validationOneOrdInf, index.ToString(), new(indexOdrDetail.ToString(), OrdInfValidationStatus.OdrNoMapOdrDetail));
                }

                odrDetail = item.OrdInfDetails.FirstOrDefault(od => !itemCds.Contains(od.ItemCd));
                if (odrDetail != null)
                {
                    var indexOdrDetail = item.OrdInfDetails.IndexOf(odrDetail);
                    AddErrorStatus(obj, validationOneOrdInf, index.ToString(), new(indexOdrDetail.ToString(), OrdInfValidationStatus.InvalidItemCd));
                }

                odrDetail = item.OrdInfDetails.FirstOrDefault(od => !ipnCds.Contains(od.IpnCd));
                if (odrDetail != null)
                {
                    var indexOdrDetail = item.OrdInfDetails.IndexOf(odrDetail);
                    AddErrorStatus(obj, validationOneOrdInf, index.ToString(), new(indexOdrDetail.ToString(), OrdInfValidationStatus.InvalidIpnCd));
                }
            });

            return validationOneOrdInf;
        }

        private NextOrderModel ConvertNextOrderToModel(int hpId, long ptId, List<Tuple<string, string>> ipnCds, NextOrderItem nextOrderItem)
        {
            return new NextOrderModel(
                    hpId,
                    ptId,
                    nextOrderItem.RsvkrtNo,
                    nextOrderItem.RsvkrtKbn,
                    nextOrderItem.RsvDate,
                    nextOrderItem.RsvName,
                    nextOrderItem.IsDeleted,
                    nextOrderItem.SortNo,
                    nextOrderItem.rsvKrtByomeiItems.Select(b => ConvertRsvkrtByomeiToModel(hpId, ptId, b)).ToList(),
                    ConvertRsvkrtKarteInfToModel(hpId, ptId, nextOrderItem.rsvkrtKarteInf),
                    nextOrderItem.rsvKrtOrderInfItems.Select(o => ConvertRsvkrtOrderInfToModel(hpId, ptId, ipnCds, o)).ToList()
                );
        }

        private RsvkrtByomeiModel ConvertRsvkrtByomeiToModel(int hpId, long ptId, RsvKrtByomeiItem rsvkrtByomeiItem)

        {
            return new RsvkrtByomeiModel(
                    rsvkrtByomeiItem.Id,
                    hpId,
                    ptId,
                    rsvkrtByomeiItem.RsvkrtNo,
                    rsvkrtByomeiItem.SeqNo,
                    rsvkrtByomeiItem.ByomeiCd,
                    rsvkrtByomeiItem.Byomei,
                    rsvkrtByomeiItem.SyobyoKbn,
                    rsvkrtByomeiItem.SikkanKbn,
                    rsvkrtByomeiItem.NanbyoCd,
                    rsvkrtByomeiItem.HosokuCmt,
                    rsvkrtByomeiItem.IsNodspRece,
                    rsvkrtByomeiItem.IsNodspKarte,
                    rsvkrtByomeiItem.IsDeleted,
                    rsvkrtByomeiItem.PrefixSuffixList,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty
                );
        }

        private RsvkrtKarteInfModel ConvertRsvkrtKarteInfToModel(int hpId, long ptId, RsvKrtKarteInfItem rsvkrtKarteInfItem)

        {
            return new RsvkrtKarteInfModel(
                    hpId,
                    ptId,
                    rsvkrtKarteInfItem.RsvDate,
                    rsvkrtKarteInfItem.RsvkrtNo,
                    rsvkrtKarteInfItem.SeqNo,
                    rsvkrtKarteInfItem.Text,
                    rsvkrtKarteInfItem.RichText,
                    rsvkrtKarteInfItem.IsDeleted
                );
        }

        private RsvkrtOrderInfModel ConvertRsvkrtOrderInfToModel(int hpId, long ptId, List<Tuple<string, string>> ipnCds, RsvKrtOrderInfItem rsvkrtOrderInfItem)

        {
            return new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvkrtOrderInfItem.RsvDate,
                    rsvkrtOrderInfItem.RsvkrtNo,
                    rsvkrtOrderInfItem.RpNo,
                    rsvkrtOrderInfItem.RpEdaNo,
                    rsvkrtOrderInfItem.Id,
                    rsvkrtOrderInfItem.HokenPid,
                    rsvkrtOrderInfItem.OdrKouiKbn,
                    rsvkrtOrderInfItem.RpName,
                    rsvkrtOrderInfItem.InoutKbn,
                    rsvkrtOrderInfItem.SikyuKbn,
                    rsvkrtOrderInfItem.SyohoSbt,
                    rsvkrtOrderInfItem.SanteiKbn,
                    rsvkrtOrderInfItem.TosekiKbn,
                    rsvkrtOrderInfItem.DaysCnt,
                    rsvkrtOrderInfItem.IsDeleted,
                    rsvkrtOrderInfItem.SortNo,
                    DateTime.MinValue,
                    0,
                    string.Empty,
                    rsvkrtOrderInfItem.RsvKrtOrderInfDetailItems.Select(od => ConvertRsvkrtOrderDetailToModel(hpId, ptId, ipnCds, od)).ToList()
                );
        }

        private RsvKrtOrderInfDetailModel ConvertRsvkrtOrderDetailToModel(int hpId, long ptId, List<Tuple<string, string>> ipnCds, RsvKrtOrderInfDetailItem rsvkrtOrderInfDetailItem)

        {
            return new RsvKrtOrderInfDetailModel(
                    hpId,
                    ptId,
                    rsvkrtOrderInfDetailItem.RsvkrtNo,
                    rsvkrtOrderInfDetailItem.RpNo,
                    rsvkrtOrderInfDetailItem.RpEdaNo,
                    rsvkrtOrderInfDetailItem.RowNo,
                    rsvkrtOrderInfDetailItem.SinKouiKbn,
                    rsvkrtOrderInfDetailItem.SinKouiKbn,
                    rsvkrtOrderInfDetailItem.ItemCd,
                    rsvkrtOrderInfDetailItem.ItemName,
                    rsvkrtOrderInfDetailItem.Suryo,
                    rsvkrtOrderInfDetailItem.UnitName,
                    rsvkrtOrderInfDetailItem.UnitSbt,
                    rsvkrtOrderInfDetailItem.TermVal,
                    rsvkrtOrderInfDetailItem.KohatuKbn,
                    rsvkrtOrderInfDetailItem.SyohoKbn,
                    rsvkrtOrderInfDetailItem.SyohoLimitKbn,
                    rsvkrtOrderInfDetailItem.DrugKbn,
                    rsvkrtOrderInfDetailItem.YohoKbn,
                    rsvkrtOrderInfDetailItem.Kokuji1,
                    rsvkrtOrderInfDetailItem.Kokuji2,
                    rsvkrtOrderInfDetailItem.IsNodspRece,
                    rsvkrtOrderInfDetailItem.IpnCd,
                    ipnCds.FirstOrDefault(ipn => ipn.Item1 == rsvkrtOrderInfDetailItem.IpnCd)?.Item2 ?? string.Empty,
                    rsvkrtOrderInfDetailItem.Bunkatu,
                    rsvkrtOrderInfDetailItem.CmtName,
                    rsvkrtOrderInfDetailItem.CmtOpt,
                    rsvkrtOrderInfDetailItem.FontColor,
                    rsvkrtOrderInfDetailItem.CommentNewline,
                    string.Empty,
                    0,
                    0,
                    false,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    string.Empty,
                    new(),
                    0,
                    0
                );
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
