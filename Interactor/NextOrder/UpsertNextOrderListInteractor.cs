using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.NextOrder;
using Domain.Models.PatientInfor;
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Interfaces;
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
                //if (inputData.HpId <= 0 || !_hpInfRepository.CheckHpId(inputData.HpId))
                //{
                //    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidHpId, new(), new(), new(), new());
                //}
                //if (inputData.PtId <= 0 || !_patientInfRepository.CheckExistListId(new List<long> { inputData.PtId }))
                //{
                //    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidPtId, new(), new(), new(), new());
                //}
                //if (inputData.UserId <= 0 || !_userRepository.CheckExistedUserId(inputData.UserId))
                //{
                //    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidUserId, new(), new(), new(), new());
                //}

                //var itemCds = new List<string>();
                //var ipnCds = new List<Tuple<string, string>>();
                //List<long> rsvkrtNos = new();
                //List<int> rsvkrtDates = new();
                //var checkOderInfs = _nextOrderRepository.GetCheckOrderInfs(inputData.HpId, inputData.PtId);
                //var hokenPids = new List<int>();
                //foreach (var item in inputData.NextOrderItems)
                //{
                //    hokenPids.AddRange(item.RsvKrtOrderInfItems.Select(o => o.HokenPid));
                //}

                //var checkHokens = _insuranceRepository.GetCheckListHokenInf(inputData.HpId, inputData.PtId, hokenPids.Distinct().ToList() ?? new List<int>());

                //foreach (var nextOrder in inputData.NextOrderItems)
                //{
                //    //rsvkrtNos.Add(nextOrder.RsvkrtNo);
                //    //rsvkrtNos.Add(nextOrder.RsvkrtKarteInf.RsvkrtNo);
                //    //rsvkrtDates.Add(nextOrder.RsvDate);
                //    //rsvkrtDates.Add(nextOrder.RsvkrtKarteInf.RsvDate);
                //    foreach (var orderInfModel in nextOrder.RsvKrtOrderInfItems)
                //    {
                //        //rsvkrtNos.Add(orderInfModel.RsvkrtNo);
                //        //rsvkrtNos.AddRange(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.RsvkrtNo));
                //        //rsvkrtDates.Add(orderInfModel.RsvDate);
                //        //rsvkrtDates.AddRange(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.RsvDate));
                //        //itemCds.AddRange(_mstItemRepository.GetCheckItemCds(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.ItemCd.Trim()).ToList()));
                //        ipnCds.AddRange(_mstItemRepository.GetCheckIpnCds(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.IpnCd.Trim()).ToList()));
                //    }
                //    //foreach (var byomei in nextOrder.RsvKrtByomeiItems)
                //    //{
                //    //    rsvkrtNos.Add(byomei.RsvkrtNo);
                //    //}
                //}

                //if (rsvkrtnos.distinct().count() > 1)
                //{
                //    return new upsertnextorderlistoutputdata(upsertnextorderliststatus.invalidrsvkrtno, new(), new(), new(), new());
                //}

                //if (rsvkrtdates.distinct().count() > 1)
                //{
                //    return new upsertnextorderlistoutputdata(upsertnextorderliststatus.invalidrsvkrtdate, new(), new(), new(), new());
                //}

                //var nextOrderModels = inputData.NextOrderItems.Select(n => NextOrderCommon.ConvertNextOrderToModel(inputData.HpId, inputData.PtId, ipnCds, n)).ToList();
                //List<(int, KarteValidationStatus)> validationKarteInfs = new();
                //Dictionary<int, NextOrderStatus> validationNextOrders = new();
                //List<(int, int, RsvkrtByomeiStatus)> validationRsvkrtByomeis = new();
                //var validationOrdInfs = new List<(int, Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>)>();

                //for (int i = 0; i < nextOrderModels.Count; i++)
                //{
                //    var nextOrderModel = nextOrderModels[i];

                //    var validationNextOrder = nextOrderModel.Validation();
                //    if (validationNextOrder != NextOrderStatus.Valid)
                //    {
                //        validationNextOrders.Add(i, validationNextOrder);
                //    }

                //    var validationKarteInf = nextOrderModel.RsvkrtKarteInf.Validation();
                //    if (validationKarteInf != KarteValidationStatus.Valid)
                //    {
                //        validationKarteInfs.Add((i, validationKarteInf));
                //    }

                //    for (int i1 = 0; i1 < nextOrderModel.RsvkrtByomeis.Count; i1++)
                //    {
                //        var byomei = nextOrderModel.RsvkrtByomeis[i1];
                //        var validationByomei = byomei.Validation();
                //        if (validationKarteInf != KarteValidationStatus.Valid)
                //        {
                //            validationRsvkrtByomeis.Add((i, i1, validationByomei));
                //        }
                //    }

                //    var validationOneOrdInf = NextOrderCommon.CheckValidateOrderInf(inputData.HpId, inputData.PtId, itemCds, ipnCds.Select(ipn => ipn.Item1).ToList(), nextOrderModel, checkOderInfs, checkHokens);

                //    if (validationOneOrdInf.Any())
                //        validationOrdInfs.Add((i, validationOneOrdInf));
                //}
                //if (validationKarteInfs.Count > 0 || validationNextOrders.Count > 0 || validationRsvkrtByomeis.Count > 0 || validationOrdInfs.Count > 0)
                //{
                //    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.Failed, validationNextOrders, validationOrdInfs, validationKarteInfs, validationRsvkrtByomeis);
                //}

                var ipnCds = new List<Tuple<string, string>>();
                foreach (var nextOrder in inputData.NextOrderItems)
                {
                    foreach (var orderInfModel in nextOrder.RsvKrtOrderInfItems)
                    {
                        ipnCds.AddRange(_mstItemRepository.GetCheckIpnCds(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.IpnCd.Trim()).ToList()));
                    }
                }
                var nextOrderModels = inputData.NextOrderItems.Select(n => NextOrderCommon.ConvertNextOrderToModel(inputData.HpId, inputData.PtId, ipnCds, n)).ToList();
                var rsvkrtNo = _nextOrderRepository.Upsert(inputData.UserId, inputData.HpId, inputData.PtId, nextOrderModels);
                return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.Successed, new(), new(), new(), new());
            }
            catch
            {
                return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.Failed, new(), new(), new(), new());
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
                _hpInfRepository.ReleaseResource();
                _insuranceRepository.ReleaseResource();
                _nextOrderRepository.ReleaseResource();
                _patientInfRepository.ReleaseResource();
                _userRepository.ReleaseResource();
            }
        }
    }
}
